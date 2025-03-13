using System.Globalization;
using System.Net.NetworkInformation;

namespace ValveServerPicker
{
	public partial class MapView : Form
	{
		/// <summary>
		/// Represents a GeoMenuItem that can toggle the blocked state of an SDR.
		/// </summary>
		private class GeoMenuItem : ToolStripMenuItem
		{
			private readonly SteamSDR sdr;
			private readonly MapView parent;

			public GeoMenuItem(SteamSDR sdr, MapView parent)
			{
				this.parent = parent;
				this.sdr = sdr;

				this.Text = sdr.ToString();
				this.Checked = sdr.Blocked;

				this.Click += this.GeoMenuItem_Click;
			}

			private void GeoMenuItem_Click(object? sender, EventArgs e)
			{
				this.sdr.Blocked = !this.sdr.Blocked;
				this.Checked = this.sdr.Blocked;

				this.parent.TriggerDataUpdate();
			}
		}

		/// <summary>
		/// Wrapper for a Geo point which may contain multiple SDRs, and a screen position.
		/// </summary>
		/// <param name="lat"></param>
		/// <param name="lon"></param>
		private class Geo(float lat, float lon)
		{
			public float lat = lat, lon = lon;
			public Point scrPos;

			public bool InRadius(Geo other, float radius)
			{
				return Math.Abs(this.lat - other.lat) < radius && Math.Abs(this.lon - other.lon) < radius;
			}
		}

		private const float geoClickRadius = 5;
		private const int dotDiameter = 5;

		// If set, trigger data update for this control
		private readonly Control? dataContainer;

		private readonly Dictionary<Geo, List<SteamSDR>> pointToSDRs = [];

		private readonly System.Windows.Forms.Timer pingRedrawTimer = new();

		// Constructor
		public MapView(List<SteamSDR> sdrs, Control? dataContainer = null)
		{
			this.InitializeComponent();

			this.dataContainer = dataContainer;

			pingRedrawTimer.Interval = (int)RelayPinger.PingTimeOut.TotalMilliseconds;
			pingRedrawTimer.Tick += (sender, e) =>
			{
				this.pic_Map.Invalidate();
			};

			this.UpdateSDRs(sdrs);

			this.pingRedrawTimer.Start();
		}

		public void UpdateSDRs(List<SteamSDR> sdrs)
		{
			if (this.IsDisposed)
			{
				return;
			}

			this.pointToSDRs.Clear();
			
			foreach (SteamSDR sdr in sdrs)
			{
				Geo geo = new(sdr.Geo.lat, sdr.Geo.lon);
				// Check if any of the existing geos are within radius
				bool found = false;
				foreach (KeyValuePair<Geo, List<SteamSDR>> kvp in this.pointToSDRs)
				{
					if (geo.InRadius(kvp.Key, 0.5f))
					{
						kvp.Value.Add(sdr);
						found = true;
						break;
					}
				}

				if (!found)
				{
					this.pointToSDRs[geo] = [sdr];
				}
			}

			this.pic_Map.Invalidate();
		}

		public void TriggerDataUpdate()
		{
			this.dataContainer?.Refresh();
			this.pic_Map.Invalidate();
		}

		private static double PointDistance(Point a, Point b)
		{
			return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
		}

		private void handleMapClick(Point mapPos)
		{
			// Get the latitude and longitude of the click
			// Point mapPos = this.pic_Map.PointToClient(clickPoint);

			// Offset by some pixels because this sucks
			mapPos.X -= (int)(geoClickRadius / 2);
			mapPos.Y -= (int)(geoClickRadius / 2);

			// Check if the click is within the map
			if (mapPos.X < 0 || mapPos.Y < 0 || mapPos.X >= this.pic_Map.Width || mapPos.Y >= this.pic_Map.Height)
			{
				return;
			}

			// Update the position label
			{
				float lat = 90.0f - (mapPos.Y / (float)this.pic_Map.Height) * 180.0f;
				float lon = 180.0f - (mapPos.X / (float)this.pic_Map.Width) * 360.0f;

#if DEBUG
				this.lbl_Pos.Text = $"pX: {mapPos.X}, pY: {mapPos.Y}, lat: {lat.ToString(CultureInfo.InvariantCulture)}, lon: {lon.ToString(CultureInfo.InvariantCulture)}";
#else
				this.lbl_Pos.Text = $"{lat.ToString(CultureInfo.InvariantCulture)}, {lon.ToString(CultureInfo.InvariantCulture)}";
#endif
			}

			// SDR selection

			List<SteamSDR> selectedSDRs = [];

			foreach (var kvp in this.pointToSDRs)
			{
				Geo geo = kvp.Key;
				List<SteamSDR> cSdr = kvp.Value;

				if (PointDistance(geo.scrPos, mapPos) < geoClickRadius)
				{
					selectedSDRs.AddRange(cSdr);

#if DEBUG
					foreach (SteamSDR sdr in cSdr)
					{
						this.lbl_Pos.Text = this.lbl_Pos.Text + $"\nSDR: {sdr.Aliases[0]} {geo.scrPos.X}, {geo.scrPos.Y}";
					}
#endif
				}
			}

			// Add them to the context menu
			this.cms_SelectedServers.Items.Clear();

			foreach (SteamSDR sdr in selectedSDRs)
			{
				GeoMenuItem item = new(sdr, this);
				this.cms_SelectedServers.Items.Add(item);
			}

			// Show the context menu
			this.cms_SelectedServers.Show(this, mapPos);
		}

		// Robinson projection constants
		private static readonly float[] robX = [1.0f, 0.9986f, 0.9954f, 0.99f, 0.9822f, 0.973f, 0.96f, 0.9427f, 0.9216f, 0.8962f, 0.8679f, 0.835f, 0.7986f, 0.7597f, 0.7186f, 0.6732f, 0.6213f, 0.5722f, 0.5322f];
		private static readonly float[] robY = [0.0f, 0.062f, 0.124f, 0.186f, 0.248f, 0.31f, 0.372f, 0.434f, 0.4958f, 0.5571f, 0.6176f, 0.6769f, 0.7346f, 0.7903f, 0.8435f, 0.8936f, 0.9394f, 0.9761f, 1.0f];

		private Point GetXY(float lat, float lon)
		{
			// Shift coordinates because of map projection
			lon += -10.2f;
			lat += 0.7f;

			// Get index of latitude
			float absLat = Math.Abs(lat);
			int index = (int)(absLat / 5.0f);
			float latRatio = (absLat % 5.0f) / 5.0f;

			// Interpolate X and Y
			float x = (robX[index] + latRatio * (robX[index + 1] - robX[index])) * (lon / 180.0f);
			float y = (robY[index] + latRatio * (robY[index + 1] - robY[index])) * (lat >= 0 ? 1 : -1);

			// Convert to pixel coordinates
			int mapWidth = this.pic_Map.Width;
			int mapHeight = this.pic_Map.Height;
			int pixelX = (int)((x + 1) * 0.5f * mapWidth);
			int pixelY = (int)((1 - (y + 1) * 0.5f) * mapHeight);

			return new Point(pixelX, pixelY);
		}
		#region resize handling
		private bool inResize = false;

		private void MapView_ResizeBegin(object sender, EventArgs e)
		{
			this.inResize = true;
		}

		private void MapView_ResizeEnd(object sender, EventArgs e)
		{
			this.inResize = false;

			this.pic_Map.Invalidate();
		}
		#endregion

		#region dragging functions
		private Rectangle? dragRect;

		private void pic_Map_MouseDown(object sender, MouseEventArgs e)
		{
			// Start a drag event
			if (e.Button == MouseButtons.Left)
			{
				this.dragRect = new(e.Location, new Size(0, 0));
			}
		}

		private void pic_Map_MouseMove(object sender, MouseEventArgs e)
		{
			if (this.dragRect.HasValue)
			{
				// Update the rectangle
				Point point = e.Location;
				if (this.dragRect.Value.X < point.X)
				{
					if (this.dragRect.Value.Y < point.Y)
					{
						this.dragRect = new(this.dragRect.Value.Location, new Size(point.X - this.dragRect.Value.X, point.Y - this.dragRect.Value.Y));
					}
					else
					{
						this.dragRect = new(this.dragRect.Value.X, point.Y, point.X - this.dragRect.Value.X, this.dragRect.Value.Y - point.Y);
					}
				}
				else
				{
					if (this.dragRect.Value.Y < point.Y)
					{
						this.dragRect = new(point.X, this.dragRect.Value.Y, this.dragRect.Value.X - point.X, point.Y - this.dragRect.Value.Y);
					}
					else
					{
						this.dragRect = new(point, new Size(this.dragRect.Value.X - point.X, this.dragRect.Value.Y - point.Y));
					}
				}

				this.pic_Map.Invalidate();
			}
		}

		private void pic_Map_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				// Check if the drag was just a click
				Point point = e.Location;

				if (this.dragRect == null || (
					this.dragRect.HasValue &&
					this.dragRect.Value.Size.Width < 5
					&& this.dragRect.Value.Size.Height < 5
					))
				{
					this.handleMapClick(point);
				}
				else
				{
					this.UseWaitCursor = true;

					bool didUpdate = false;
					foreach (KeyValuePair<Geo, List<SteamSDR>> kvp in this.pointToSDRs)
					{
						Geo geo = kvp.Key;
						if (this.dragRect.Value.Contains(geo.scrPos))
						{
							kvp.Value.ForEach(sdr => sdr.Blocked = !sdr.Blocked);
							didUpdate = true;
						}
					}

					if (didUpdate)
					{
						GC.Collect();

						this.TriggerDataUpdate();
					}

					this.UseWaitCursor = false;
				}

				this.dragRect = null;
			}
		}
		#endregion

		/// <summary>
		/// Paints the map and the SDRs on it.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pic_Map_Paint(object sender, PaintEventArgs e)
		{
			if (this.inResize)
			{
				// Access the pings so they do not expire
				foreach (List<SteamSDR> sdrList in this.pointToSDRs.Values)
				{
					sdrList.ForEach(sdr =>
					{
						// Access the ping
						_ = sdr.AvgPing;
					});
				}

				return;
			}

			this.pingRedrawTimer.Stop();

			Graphics g = e.Graphics;
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			// Draw the geo points
			foreach (KeyValuePair<Geo, List<SteamSDR>> kvp in this.pointToSDRs)
			{
				Geo geo = kvp.Key;
				List<SteamSDR> sdrs = kvp.Value;
				// Calculate the x and y
				Point pos = this.GetXY(geo.lat, geo.lon);

				// Set the screen position
				geo.scrPos.X = pos.X;
				geo.scrPos.Y = pos.Y;

				// If all are disabled, draw red, if one or more is disabled, draw yellow, if all are enabled, draw green
				Brush brush;

				int blockedCount = 0;
				// NOTE: This is an average of the average ping.
				int pingAvg = 0;
				int enabledCount = 0;

				foreach (SteamSDR sdr in sdrs)
				{
					if (sdr.Blocked)
					{
						blockedCount++;
					}
					else
					{
						enabledCount++;
						pingAvg += (int)sdr.AvgPing;
					}
				}

				if (pingAvg != 0)
				{
					pingAvg /= enabledCount;
				}

				if (blockedCount == 0)
				{
					brush = Brushes.Green;
				}
				else if (blockedCount == sdrs.Count)
				{
					brush = Brushes.Red;
				}
				else
				{
					brush = Brushes.Orange;
				}

				g.FillEllipse(brush, pos.X - dotDiameter / 2, pos.Y - dotDiameter / 2, dotDiameter, dotDiameter);

				string text = pingAvg >= 0 ? $"{pingAvg}ms" : "N/A";
				Brush pingBrush = pingAvg >= 0 ? Brushes.Black : Brushes.Red;

				SizeF v = g.MeasureString(text, this.Font);
				g.DrawString(text, this.Font, pingBrush, pos.X - v.Width / 2, pos.Y + (v.Height / 4));
			}

			// Draw the dragRect
			if (this.dragRect.HasValue)
			{
				// Point point = this.PointToClient(MousePosition);

				g.DrawRectangle(Pens.Black, this.dragRect.Value);
			}

			// Restart the ping timer
			this.pingRedrawTimer.Start();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			this.pingRedrawTimer.Stop();
			this.pingRedrawTimer.Dispose();

			if (disposing && (components != null))
			{
				components.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}
