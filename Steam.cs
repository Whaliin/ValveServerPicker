using System.Net;
using System.Net.NetworkInformation;
using System.Text.Json;

namespace ValveServerPicker
{
	public class SteamSDR
	{
		public string Desc { get; set; }
		public (float lat, float lon) Geo { get; set; }
		public int Partners { get; set; }
		public int Tier { get; set; }
		public List<SteamRelay>? Relays { get; set; }
		public string[] Aliases { get; set; }

		public string AliasesString => string.Join(", ", this.Aliases);

		private bool _isBlocked;

		public bool Blocked
		{
			get
			{
				return _isBlocked;
			}
			set
			{
				_isBlocked = value;

				Firewall.UpdateSDRRule(this, value);
			}
		}

		public SteamSDR(JsonProperty jProp)
		{
			this.Desc = jProp.Value.GetProperty("desc").GetString() ?? throw new Exception("Description is null");

			{
				List<string> tAliases = [
					jProp.Name
				];

				if (jProp.Value.TryGetProperty("aliases", out JsonElement aliases))
				{
					aliases.EnumerateArray().ToList()
						.ForEach(
							alias => tAliases.Add(
								alias.GetString() ?? throw new Exception("Alias is null")
							)
						);
				}

				this.Aliases = [.. tAliases];
			}

			JsonElement geo = jProp.Value.GetProperty("geo");

			this.Geo = (geo[1].GetSingle(), geo[0].GetSingle());

			this.Partners = jProp.Value.GetProperty("partners").GetInt32();

			this.Tier = jProp.Value.GetProperty("tier").GetInt32();

			if (jProp.Value.TryGetProperty("relays", out JsonElement relays))
			{
				this.Relays = [];

				relays.EnumerateArray().ToList()
					.ForEach(
						relay => this.Relays.Add(
							new SteamRelay(relay)
						)
					);
			}

			this._isBlocked = Firewall.TryGetSDRRule(this, out _);
		}

		public override string ToString()
		{
			return $"{this.Desc} ({this.AliasesString})";
		}

		/// <summary>
		/// The average ping of all the relays in this SDR, will return 0 on first ping.
		/// </summary>
		public long AvgPing
		{
			get
			{
				if (this.Relays == null)
				{
					return -2;
				}

				long totPing = 0;
				int validRelays = 0;

				foreach (SteamRelay relay in this.Relays.Where(relay => relay.Ping != -1))
				{
					totPing += relay.Ping;
					validRelays++;
				}

				// Avoid division by zero
				if (validRelays == 0)
				{
					return -1;
				}

				return totPing /= validRelays;
			}
		}
	}

	/// <summary>
	/// Self-pinging relay object, will ping the relay at least every second and store the ping.
	/// Will self-dispose if the ping hasn't been read for 2 seconds.
	/// </summary>
	public class RelayPinger : IDisposable
	{
		/// <summary>
		/// The maximum time between pings before the relay is considered unresponsive.
		/// </summary>
		public static readonly TimeSpan PingTimeOut = TimeSpan.FromMilliseconds(1000);
		private static readonly TimeSpan MaxUnreadLife = TimeSpan.FromSeconds(2);

		private DateTime LastRead;
		private readonly IPAddress IP;
		private readonly Ping Pinger;
		private readonly CancellationTokenSource tks;

		// -1 indicates that the ping hasn't been done yet, or is invalid
		private long _ping;
		public long Ping { 
			get 
			{
				this.LastRead = DateTime.Now;
				return this._ping;
			} 
			private set
			{
				this._ping = value;
			} 
		}

		public RelayPinger(SteamRelay relay)
		{
			this.Ping = -1;

			if (!IPAddress.TryParse(relay.IPv4, out IPAddress? parseAddr))
			{
				this.IP = default!;
				this.Pinger = default!;
				this.tks = default!;

				// Invalid IP address, return
				return;
			}

			this.IP = parseAddr;

			this.Pinger = new();
			this.tks = new();

			this.LastRead = DateTime.Now;

			_ = this.PingRelay();
		}

		public async Task PingRelay()
		{
			while (!this.tks.Token.IsCancellationRequested)
			{
				if (DateTime.Now - this.LastRead > MaxUnreadLife)
				{
					this.Ping = -1;
					this.StopPinging();
					return;
				}

				try
				{
					PingReply reply = await this.Pinger.SendPingAsync(this.IP, PingTimeOut, cancellationToken: this.tks.Token);

					this.Ping = reply.Status == IPStatus.Success ? reply.RoundtripTime : -1;
				}
				catch (Exception)
				{
					this.Ping = -1;
				}
			}
		}

		public bool IsDisposed => this.tks.IsCancellationRequested;

		private void StopPinging()
		{
			if (this.tks.IsCancellationRequested)
			{
				return;
			}

			this.tks.Cancel();
			this.tks.Dispose();
			this.Pinger.Dispose();
		}

		public void Dispose()
		{
			this.StopPinging();
			GC.SuppressFinalize(this);
		}

		~RelayPinger() => this.Dispose();
	}

	public class SteamRelay
	{
		public string IPv4 { get; set; }
		public (int Low, int High) PortRange { get; set; }

		public SteamRelay(JsonElement jElem)
		{
			this.IPv4 = jElem.GetProperty("ipv4").GetString() ?? throw new Exception("IPv4 is null");

			JsonElement portRange = jElem.GetProperty("port_range");

			int p1 = portRange[0].GetInt32();
			int p2 = portRange[1].GetInt32();

			this.PortRange = (Math.Min(p1, p2), Math.Max(p1, p2));
		}

		public override string ToString()
		{
			return $"{this.IPv4}:{this.PortRange.Low}-{this.PortRange.High}";
		}

		private RelayPinger? pinger;

		public long Ping
		{
			get
			{
				if (this.pinger == null || this.pinger.IsDisposed)
				{
					this.pinger = new(this);
				}

				return pinger.Ping;
			}
		}

		~SteamRelay() => this.pinger?.Dispose();
	}

	internal class Steam
	{
		static string baseUrl = "https://api.steampowered.com/ISteamApps/GetSDRConfig/v1/?appid=";

		public static void UpdateSDRList(List<SteamSDR> outList, uint appId, bool ignoreRelayLess = false)
		{
			string url = $"{baseUrl}{appId}";

			outList.Clear();

			Firewall.UpdateSDRRuleList();

			using (HttpClient client = new())
			{
				string response;
				try
				{
					response = client.GetStringAsync(url).Result;
				}
				catch (Exception)
				{
					return;
				}
				JsonDocument json = JsonDocument.Parse(response);
				if (json.RootElement.GetProperty("success").GetBoolean() == false)
				{
					throw new Exception("Failed to get SDR data");
				}
				JsonElement pops = json.RootElement.GetProperty("pops");
				foreach (JsonProperty pop in pops.EnumerateObject())
				{
					SteamSDR nSDR = new(pop);

					if (ignoreRelayLess || nSDR.Relays?.Count > 0)
					{
						outList.Add(nSDR);
					}
				}
			}
		}
	}
}
