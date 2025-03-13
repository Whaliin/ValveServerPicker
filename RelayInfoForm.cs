using System.Globalization;

namespace ValveServerPicker
{
	public partial class RelayInfoForm : Form
	{
		private readonly System.Windows.Forms.Timer PingUpdateTimer;
		private readonly SteamSDR SDR;

		private void pingUpdateTimer_Tick(object? _, EventArgs e)
		{
			this.PingUpdateTimer.Stop();

			if (this.SDR.AvgPing <= 0)
			{
				this.lbl_AvgPing.Text = "N/A";
			}
			else
			{
				this.lbl_AvgPing.Text = string.Format("{0}ms", this.SDR.AvgPing);
			}

			this.PingUpdateTimer.Start();
		}

		public RelayInfoForm(SteamSDR sdr)
		{
			InitializeComponent();

			this.SDR = sdr;

			// Add the data to the form
			this.txt_Desc.Text = sdr.Desc;

			// Stupid culture hardcode
			string lat = sdr.Geo.lat.ToString("0.000000", CultureInfo.InvariantCulture);
			string lon = sdr.Geo.lon.ToString("0.000000", CultureInfo.InvariantCulture);

			// Invert it because for some reason every web map does it this way. Makes it easier when copying and searching.
			this.txt_Geo.Text = $"{lat}, {lon}";
			this.txt_Partners.Text = sdr.Partners.ToString();
			this.txt_Tier.Text = sdr.Tier.ToString();
			this.txt_Aliases.Text = string.Join(", ", sdr.Aliases);

			// Add the relays to the listbox
			sdr.Relays?.ForEach(
				relay => this.lb_Relays.Items.Add(relay.ToString())
			);

			// Set the form title
			this.Text = $"Relay Info: {sdr.Desc}";

			// Shrink the form to fit the data
			this.AutoSize = true;
			this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

			// Set up the ping update timer
			this.PingUpdateTimer = new()
			{
				Interval = (int)RelayPinger.PingTimeOut.TotalMilliseconds
			};
			this.PingUpdateTimer.Tick += this.pingUpdateTimer_Tick;

			this.PingUpdateTimer.Start();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			this.PingUpdateTimer.Stop();
			this.PingUpdateTimer.Dispose();

			if (disposing && (components != null))
			{
				components.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}
