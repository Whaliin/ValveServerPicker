namespace ValveServerPicker
{
	public partial class MainForm : Form
	{
		private MapView? mapView;
		private List<SteamSDR> currentSDRs;
		private BindingSource gridDataBinder;

		public MainForm()
		{
			this.currentSDRs = [];
			this.gridDataBinder = new()
			{
				DataSource = this.currentSDRs
			};

			this.InitializeComponent();
			this.dgv_SDR.AutoGenerateColumns = false;

			// Set the app selector data binding
			BindingSource bs = new()
			{
				DataSource = AppDictionary.Apps.Keys.ToList()
			};
			this.cb_AppSelector.DataSource = bs;

			// Set up data bindings for the grid
			this.dgv_SDR.DataSource = this.gridDataBinder;

			// Set the column data bindings
			this.dgv_SDR.Columns["gC_Desc"].DataPropertyName = "Desc";
			this.dgv_SDR.Columns["gC_Aliases"].DataPropertyName = "AliasesString";
			this.dgv_SDR.Columns["gC_Blocked"].DataPropertyName = "Blocked";

			this.toolTips.SetToolTip(this.cb_IgnoreRelayLess, "Include SDRs with no relays");
			this.toolTips.SetToolTip(this.cb_AppSelector, "Select an app to view SDRs for");
			this.toolTips.SetToolTip(this.num_AppId, "Enter an app ID to view SDRs for");
			this.toolTips.SetToolTip(this.btn_MapView, "Open the map view");
			this.toolTips.SetToolTip(this.btn_BlockAll, "Block all SDR relays");
			this.toolTips.SetToolTip(this.btn_UnblockAll, "Unblock all SDR relays");
			this.toolTips.SetToolTip(this.btn_DeleteAllRules, "Delete all SDR rules from the firewall");
		}

		/// <summary>
		/// Updates the SDR data based on the selected app ID.
		/// </summary>
		/// <returns></returns>
		private void UpdateSDRs()
		{
			// Block the UI
			this.Enabled = false;
			// this.SuspendLayout();

			uint selectedAppId = (uint)this.num_AppId.Value;
			Steam.UpdateSDRList(this.currentSDRs, selectedAppId, this.cb_IgnoreRelayLess.Checked);

			this.gridDataBinder.DataSource = this.currentSDRs;
			this.dgv_SDR.DataSource = this.gridDataBinder;

			// Reset bindings
			// Fixes the issue where the grid doesn't update properly when switching appID
			this.gridDataBinder.ResetBindings(true);

			int blockedCount = this.currentSDRs.Count(sdr => sdr.Blocked);
			this.lbl_SDRInfo.Text = $"SDRs: {this.currentSDRs.Count}\n{blockedCount} blocked";

			// Update mapview if it is open
			this.mapView?.UpdateSDRs(this.currentSDRs);

			// this.ResumeLayout();

			this.Enabled = true;
		}

		private void cb_AppSelector_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Get the value of cb
			string selectedApp = (this.cb_AppSelector.SelectedItem?.ToString()) ?? throw new Exception("SelectedIndex is null");

			// Get the value of the selected app
			uint selectedAppValue = AppDictionary.Apps[selectedApp];

			// If the value is 0, enable the textbox
			this.num_AppId.Enabled = selectedAppValue == 0;

			// Set the value of the selected app
			// The SDR update is triggered by changing the value of the num_AppId control
			this.num_AppId.Value = selectedAppValue;
		}

		private void num_AppId_ValueChanged(object sender, EventArgs e)
		{
			this.UpdateSDRs();
		}

		#region Side Button Event Handlers

		private void btn_MapView_Click(object sender, EventArgs e)
		{
			// Open map view
			if (this.mapView == null || this.mapView.IsDisposed)
			{
				this.mapView = new(this.currentSDRs, this.dgv_SDR);
				this.mapView.Show();
			}
			else
			{
				// Focus the window
				this.mapView.Focus();
			}
		}

		private void btn_BlockAll_Click(object sender, EventArgs e)
		{
			int sdrCount = this.currentSDRs.Count;

			DialogResult result = MessageBox.Show(
				$"This will block all {sdrCount} SDR relays from the active list, are you sure?",
				"Block All SDR Relays",
				MessageBoxButtons.YesNo
			);

			if (result == DialogResult.Yes)
			{
				this.currentSDRs.ForEach(sdr => sdr.Blocked = true);

				this.UpdateSDRs();
			}
		}

		private void btn_UnblockAll_Click(object sender, EventArgs e)
		{
			int sdrCount = this.currentSDRs.Count;

			DialogResult result = MessageBox.Show(
				$"This will unblock all {sdrCount} SDR relays from the active list, are you sure?",
				"Unblock All SDR Relays",
				MessageBoxButtons.YesNo
			);

			if (result == DialogResult.Yes)
			{
				this.currentSDRs.ForEach(sdr => sdr.Blocked = false);
				this.UpdateSDRs();
			}
		}

		private void btn_DeleteAllRules_Click(object sender, EventArgs e)
		{
			DialogResult result = MessageBox.Show(
				"This will delete all SDR rules from the firewall, are you sure?",
				"Delete All SDR Rules",
				MessageBoxButtons.YesNo
			);

			if (result == DialogResult.Yes)
			{
				Firewall.DeleteAllSDRRules();
				this.UpdateSDRs();
			}
		}

		#endregion

		private void dgv_SDR_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			// Get the selected cells
			var cells = this.dgv_SDR.SelectedCells;

			if (cells.Count == 0)
			{
				return;
			}

			bool sdrUpdated = false;

			foreach (DataGridViewCell cell in cells)
			{
				object? item = cell.OwningRow.DataBoundItem;

				if (item is SteamSDR sdr)
				{
					// Check the column index of the cell
					if (cell.ColumnIndex == this.dgv_SDR.Columns["gC_Blocked"].Index)
					{
						sdr.Blocked = !sdr.Blocked;
						sdrUpdated = true;
					}
					else if (cell.ColumnIndex == this.dgv_SDR.Columns["gC_Info"].Index)
					{
						RelayInfoForm infoForm = new(sdr);
						infoForm.Show();
					}
				}
				else
				{
					throw new Exception($"Item is not correct type: {item?.GetType()}");
				}
			}

			if (sdrUpdated)
			{
				this.SuspendLayout();

				// Update the SDR data
				this.dgv_SDR.Refresh();

				// Update the map view
				this.mapView?.UpdateSDRs(this.currentSDRs);

				this.ResumeLayout();
			}
		}

		private void cb_IgnoreRelayLess_CheckedChanged(object sender, EventArgs e)
		{
			this.UpdateSDRs();
		}
	}
}
