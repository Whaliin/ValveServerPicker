namespace ValveServerPicker
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.lbl_AppId = new Label();
			this.tableLayoutPanel2 = new TableLayoutPanel();
			this.cb_AppSelector = new ComboBox();
			this.num_AppId = new NumericUpDown();
			this.tableLayoutPanel3 = new TableLayoutPanel();
			this.btn_BlockAll = new Button();
			this.btn_UnblockAll = new Button();
			this.btn_MapView = new Button();
			this.btn_DeleteAllRules = new Button();
			this.lbl_SDRInfo = new Label();
			this.cb_IgnoreRelayLess = new CheckBox();
			this.dgv_SDR = new DataGridView();
			this.gC_Blocked = new DataGridViewCheckBoxColumn();
			this.gC_Desc = new DataGridViewTextBoxColumn();
			this.gC_Aliases = new DataGridViewTextBoxColumn();
			this.gC_Info = new DataGridViewButtonColumn();
			this.steamSDRBindingSource = new BindingSource(this.components);
			this.toolTips = new ToolTip(this.components);
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.num_AppId).BeginInit();
			this.tableLayoutPanel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.dgv_SDR).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.steamSDRBindingSource).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
			this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.lbl_AppId, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.dgv_SDR, 1, 1);
			this.tableLayoutPanel1.Dock = DockStyle.Fill;
			this.tableLayoutPanel1.Location = new Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new Size(684, 461);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// lbl_AppId
			// 
			this.lbl_AppId.AutoSize = true;
			this.lbl_AppId.Dock = DockStyle.Fill;
			this.lbl_AppId.Location = new Point(3, 0);
			this.lbl_AppId.Name = "lbl_AppId";
			this.lbl_AppId.Size = new Size(104, 35);
			this.lbl_AppId.TabIndex = 1;
			this.lbl_AppId.Text = "Application ID";
			this.lbl_AppId.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
			this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
			this.tableLayoutPanel2.Controls.Add(this.cb_AppSelector, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.num_AppId, 1, 0);
			this.tableLayoutPanel2.Dock = DockStyle.Fill;
			this.tableLayoutPanel2.Location = new Point(113, 3);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Size = new Size(568, 29);
			this.tableLayoutPanel2.TabIndex = 2;
			// 
			// cb_AppSelector
			// 
			this.cb_AppSelector.Dock = DockStyle.Fill;
			this.cb_AppSelector.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cb_AppSelector.FormattingEnabled = true;
			this.cb_AppSelector.Location = new Point(3, 3);
			this.cb_AppSelector.Name = "cb_AppSelector";
			this.cb_AppSelector.Size = new Size(278, 23);
			this.cb_AppSelector.TabIndex = 1;
			this.cb_AppSelector.SelectedIndexChanged += this.cb_AppSelector_SelectedIndexChanged;
			// 
			// num_AppId
			// 
			this.num_AppId.Dock = DockStyle.Fill;
			this.num_AppId.Location = new Point(287, 3);
			this.num_AppId.Maximum = new decimal(new int[] { -1, 0, 0, 0 });
			this.num_AppId.Name = "num_AppId";
			this.num_AppId.Size = new Size(278, 23);
			this.num_AppId.TabIndex = 2;
			this.num_AppId.ValueChanged += this.num_AppId_ValueChanged;
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.AutoSize = true;
			this.tableLayoutPanel3.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel3.ColumnCount = 1;
			this.tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			this.tableLayoutPanel3.Controls.Add(this.btn_BlockAll, 0, 4);
			this.tableLayoutPanel3.Controls.Add(this.btn_UnblockAll, 0, 3);
			this.tableLayoutPanel3.Controls.Add(this.btn_MapView, 0, 2);
			this.tableLayoutPanel3.Controls.Add(this.btn_DeleteAllRules, 0, 5);
			this.tableLayoutPanel3.Controls.Add(this.lbl_SDRInfo, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.cb_IgnoreRelayLess, 0, 0);
			this.tableLayoutPanel3.Dock = DockStyle.Fill;
			this.tableLayoutPanel3.Location = new Point(3, 38);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 6;
			this.tableLayoutPanel3.RowStyles.Add(new RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			this.tableLayoutPanel3.RowStyles.Add(new RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new RowStyle());
			this.tableLayoutPanel3.Size = new Size(104, 420);
			this.tableLayoutPanel3.TabIndex = 5;
			// 
			// btn_BlockAll
			// 
			this.btn_BlockAll.Dock = DockStyle.Fill;
			this.btn_BlockAll.Location = new Point(3, 365);
			this.btn_BlockAll.Name = "btn_BlockAll";
			this.btn_BlockAll.Size = new Size(98, 23);
			this.btn_BlockAll.TabIndex = 1;
			this.btn_BlockAll.Text = "Block All";
			this.btn_BlockAll.UseVisualStyleBackColor = true;
			this.btn_BlockAll.Click += this.btn_BlockAll_Click;
			// 
			// btn_UnblockAll
			// 
			this.btn_UnblockAll.Dock = DockStyle.Fill;
			this.btn_UnblockAll.Location = new Point(3, 336);
			this.btn_UnblockAll.Name = "btn_UnblockAll";
			this.btn_UnblockAll.Size = new Size(98, 23);
			this.btn_UnblockAll.TabIndex = 2;
			this.btn_UnblockAll.Text = "Unblock All";
			this.btn_UnblockAll.UseVisualStyleBackColor = true;
			this.btn_UnblockAll.Click += this.btn_UnblockAll_Click;
			// 
			// btn_MapView
			// 
			this.btn_MapView.Dock = DockStyle.Fill;
			this.btn_MapView.Location = new Point(3, 307);
			this.btn_MapView.Name = "btn_MapView";
			this.btn_MapView.Size = new Size(98, 23);
			this.btn_MapView.TabIndex = 3;
			this.btn_MapView.Text = "Map View";
			this.btn_MapView.UseVisualStyleBackColor = true;
			this.btn_MapView.Click += this.btn_MapView_Click;
			// 
			// btn_DeleteAllRules
			// 
			this.btn_DeleteAllRules.Dock = DockStyle.Fill;
			this.btn_DeleteAllRules.Location = new Point(3, 394);
			this.btn_DeleteAllRules.Name = "btn_DeleteAllRules";
			this.btn_DeleteAllRules.Size = new Size(98, 23);
			this.btn_DeleteAllRules.TabIndex = 4;
			this.btn_DeleteAllRules.Text = "Cleanup";
			this.btn_DeleteAllRules.UseVisualStyleBackColor = true;
			this.btn_DeleteAllRules.Click += this.btn_DeleteAllRules_Click;
			// 
			// lbl_SDRInfo
			// 
			this.lbl_SDRInfo.AutoSize = true;
			this.lbl_SDRInfo.Dock = DockStyle.Fill;
			this.lbl_SDRInfo.Location = new Point(3, 25);
			this.lbl_SDRInfo.Name = "lbl_SDRInfo";
			this.lbl_SDRInfo.Size = new Size(98, 279);
			this.lbl_SDRInfo.TabIndex = 5;
			this.lbl_SDRInfo.Text = "SDR Info:";
			// 
			// cb_IgnoreRelayLess
			// 
			this.cb_IgnoreRelayLess.AutoSize = true;
			this.cb_IgnoreRelayLess.Dock = DockStyle.Fill;
			this.cb_IgnoreRelayLess.Location = new Point(3, 3);
			this.cb_IgnoreRelayLess.Name = "cb_IgnoreRelayLess";
			this.cb_IgnoreRelayLess.Size = new Size(98, 19);
			this.cb_IgnoreRelayLess.TabIndex = 6;
			this.cb_IgnoreRelayLess.Text = "No Relays";
			this.cb_IgnoreRelayLess.UseVisualStyleBackColor = true;
			this.cb_IgnoreRelayLess.CheckedChanged += this.cb_IgnoreRelayLess_CheckedChanged;
			// 
			// dgv_SDR
			// 
			this.dgv_SDR.AllowUserToAddRows = false;
			this.dgv_SDR.AllowUserToDeleteRows = false;
			this.dgv_SDR.AllowUserToResizeRows = false;
			this.dgv_SDR.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgv_SDR.Columns.AddRange(new DataGridViewColumn[] { this.gC_Blocked, this.gC_Desc, this.gC_Aliases, this.gC_Info });
			this.dgv_SDR.Dock = DockStyle.Fill;
			this.dgv_SDR.Location = new Point(113, 38);
			this.dgv_SDR.Name = "dgv_SDR";
			this.dgv_SDR.ReadOnly = true;
			this.dgv_SDR.RowHeadersVisible = false;
			this.dgv_SDR.Size = new Size(568, 420);
			this.dgv_SDR.TabIndex = 6;
			this.dgv_SDR.CellClick += this.dgv_SDR_CellClick;
			// 
			// gC_Blocked
			// 
			this.gC_Blocked.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
			this.gC_Blocked.HeaderText = "Blocked";
			this.gC_Blocked.Name = "gC_Blocked";
			this.gC_Blocked.ReadOnly = true;
			this.gC_Blocked.Resizable = DataGridViewTriState.False;
			this.gC_Blocked.ToolTipText = "A checked box indicates that this SDR will be blocked in the firewall";
			this.gC_Blocked.Width = 55;
			// 
			// gC_Desc
			// 
			this.gC_Desc.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			this.gC_Desc.HeaderText = "Description";
			this.gC_Desc.Name = "gC_Desc";
			this.gC_Desc.ReadOnly = true;
			this.gC_Desc.ToolTipText = "The description (name) of the SDR";
			this.gC_Desc.Width = 92;
			// 
			// gC_Aliases
			// 
			this.gC_Aliases.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			this.gC_Aliases.HeaderText = "Aliases";
			this.gC_Aliases.Name = "gC_Aliases";
			this.gC_Aliases.ReadOnly = true;
			this.gC_Aliases.ToolTipText = "Aliases associated with this SDR";
			this.gC_Aliases.Width = 68;
			// 
			// gC_Info
			// 
			this.gC_Info.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
			this.gC_Info.HeaderText = "Info";
			this.gC_Info.Name = "gC_Info";
			this.gC_Info.ReadOnly = true;
			this.gC_Info.Text = "View";
			this.gC_Info.ToolTipText = "Press the button to view additional information for the SDR";
			this.gC_Info.UseColumnTextForButtonValue = true;
			this.gC_Info.Width = 34;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new SizeF(7F, 15F);
			this.AutoScaleMode = AutoScaleMode.Font;
			this.ClientSize = new Size(684, 461);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Icon = Properties.Resources.ico;
			this.MinimumSize = new Size(350, 200);
			this.Name = "MainForm";
			this.Text = "Valve Server Picker";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.num_AppId).EndInit();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.dgv_SDR).EndInit();
			((System.ComponentModel.ISupportInitialize)this.steamSDRBindingSource).EndInit();
			this.ResumeLayout(false);
		}

		#endregion

		private TableLayoutPanel tableLayoutPanel1;
		private Label lbl_AppId;
		private TableLayoutPanel tableLayoutPanel2;
		private ComboBox cb_AppSelector;
		private NumericUpDown num_AppId;
		private BindingSource steamSDRBindingSource;
		private TableLayoutPanel tableLayoutPanel3;
		private Button btn_BlockAll;
		private Button btn_UnblockAll;
		private DataGridView dgv_SDR;
		private Button btn_MapView;
		private Button btn_DeleteAllRules;
		private DataGridViewCheckBoxColumn gC_Blocked;
		private DataGridViewTextBoxColumn gC_Desc;
		private DataGridViewTextBoxColumn gC_Aliases;
		private DataGridViewButtonColumn gC_Info;
		private Label lbl_SDRInfo;
		private CheckBox cb_IgnoreRelayLess;
		private ToolTip toolTips;
	}
}
