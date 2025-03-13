namespace ValveServerPicker
{
	partial class MapView
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapView));
			this.pic_Map = new PictureBox();
			this.lbl_Pos = new Label();
			this.cms_SelectedServers = new ContextMenuStrip(this.components);
			((System.ComponentModel.ISupportInitialize)this.pic_Map).BeginInit();
			this.SuspendLayout();
			// 
			// pic_Map
			// 
			this.pic_Map.BackColor = SystemColors.Control;
			this.pic_Map.BackgroundImage = Properties.Resources.map;
			this.pic_Map.BackgroundImageLayout = ImageLayout.Stretch;
			this.pic_Map.BorderStyle = BorderStyle.Fixed3D;
			this.pic_Map.Dock = DockStyle.Fill;
			this.pic_Map.InitialImage = null;
			this.pic_Map.Location = new Point(0, 0);
			this.pic_Map.Name = "pic_Map";
			this.pic_Map.Size = new Size(800, 450);
			this.pic_Map.TabIndex = 1;
			this.pic_Map.TabStop = false;
			this.pic_Map.WaitOnLoad = true;
			this.pic_Map.Paint += this.pic_Map_Paint;
			this.pic_Map.MouseDown += this.pic_Map_MouseDown;
			this.pic_Map.MouseMove += this.pic_Map_MouseMove;
			this.pic_Map.MouseUp += this.pic_Map_MouseUp;
			// 
			// lbl_Pos
			// 
			this.lbl_Pos.AutoSize = true;
			this.lbl_Pos.Location = new Point(12, 8);
			this.lbl_Pos.Name = "lbl_Pos";
			this.lbl_Pos.Size = new Size(25, 15);
			this.lbl_Pos.TabIndex = 2;
			this.lbl_Pos.Text = "0, 0";
			// 
			// cms_SelectedServers
			// 
			this.cms_SelectedServers.Name = "contextMenuStrip1";
			this.cms_SelectedServers.Size = new Size(61, 4);
			// 
			// MapView
			// 
			this.AutoScaleDimensions = new SizeF(7F, 15F);
			this.AutoScaleMode = AutoScaleMode.Font;
			this.ClientSize = new Size(800, 450);
			this.Controls.Add(this.lbl_Pos);
			this.Controls.Add(this.pic_Map);
			this.Icon = Properties.Resources.ico;
			this.Name = "MapView";
			this.Text = "Map View";
			this.ResizeBegin += this.MapView_ResizeBegin;
			this.ResizeEnd += this.MapView_ResizeEnd;
			((System.ComponentModel.ISupportInitialize)this.pic_Map).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		#endregion

		private PictureBox pic_Map;
		private Label lbl_Pos;
		private ContextMenuStrip cms_SelectedServers;
	}
}