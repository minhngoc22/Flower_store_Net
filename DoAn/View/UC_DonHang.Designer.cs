namespace DoAn.View
{
    partial class UC_DonHang
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            label1 = new Label();
            cbo_trangthai = new Guna.UI2.WinForms.Guna2ComboBox();
            label2 = new Label();
            btn_update = new Guna.UI2.WinForms.Guna2Button();
            btn_chitiet = new Guna.UI2.WinForms.Guna2Button();
            btn_reset = new Guna.UI2.WinForms.Guna2Button();
            dvg_dh = new DataGridView();
            contextMenu_ChiTietDH = new ContextMenuStrip(components);
            toolStripMenuItem1 = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)dvg_dh).BeginInit();
            contextMenu_ChiTietDH.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Arial", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(200, 93, 100);
            label1.Location = new Point(15, 0);
            label1.Name = "label1";
            label1.Size = new Size(209, 46);
            label1.TabIndex = 0;
            label1.Text = "Đơn Hàng";
            // 
            // cbo_trangthai
            // 
            cbo_trangthai.BackColor = Color.Transparent;
            cbo_trangthai.BorderRadius = 10;
            cbo_trangthai.CustomizableEdges = customizableEdges1;
            cbo_trangthai.DrawMode = DrawMode.OwnerDrawFixed;
            cbo_trangthai.DropDownStyle = ComboBoxStyle.DropDownList;
            cbo_trangthai.FocusedColor = Color.FromArgb(94, 148, 255);
            cbo_trangthai.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cbo_trangthai.Font = new Font("Segoe UI", 10F);
            cbo_trangthai.ForeColor = Color.FromArgb(68, 88, 112);
            cbo_trangthai.ItemHeight = 30;
            cbo_trangthai.Location = new Point(28, 93);
            cbo_trangthai.Name = "cbo_trangthai";
            cbo_trangthai.ShadowDecoration.CustomizableEdges = customizableEdges2;
            cbo_trangthai.Size = new Size(196, 36);
            cbo_trangthai.TabIndex = 2;
            cbo_trangthai.SelectedIndexChanged += cbo_trangthai_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Arial", 12F, FontStyle.Bold);
            label2.ForeColor = Color.FromArgb(131, 139, 237);
            label2.Location = new Point(21, 66);
            label2.Name = "label2";
            label2.Size = new Size(203, 24);
            label2.TabIndex = 3;
            label2.Text = "Trạng thái đơn hàng";
            // 
            // btn_update
            // 
            btn_update.BorderRadius = 10;
            btn_update.CustomizableEdges = customizableEdges3;
            btn_update.DisabledState.BorderColor = Color.DarkGray;
            btn_update.DisabledState.CustomBorderColor = Color.DarkGray;
            btn_update.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btn_update.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btn_update.FillColor = Color.FromArgb(239, 176, 201);
            btn_update.Font = new Font("Arial", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_update.ForeColor = Color.White;
            btn_update.Location = new Point(261, 82);
            btn_update.Name = "btn_update";
            btn_update.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btn_update.Size = new Size(192, 47);
            btn_update.TabIndex = 4;
            btn_update.Text = "Cập nhật";
            btn_update.Click += btn_update_Click;
            // 
            // btn_chitiet
            // 
            btn_chitiet.BorderRadius = 10;
            btn_chitiet.CustomizableEdges = customizableEdges5;
            btn_chitiet.DisabledState.BorderColor = Color.DarkGray;
            btn_chitiet.DisabledState.CustomBorderColor = Color.DarkGray;
            btn_chitiet.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btn_chitiet.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btn_chitiet.FillColor = Color.FromArgb(239, 176, 201);
            btn_chitiet.Font = new Font("Arial", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_chitiet.ForeColor = Color.White;
            btn_chitiet.Location = new Point(462, 82);
            btn_chitiet.Name = "btn_chitiet";
            btn_chitiet.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btn_chitiet.Size = new Size(192, 47);
            btn_chitiet.TabIndex = 4;
            btn_chitiet.Text = "Chi tiết";
            btn_chitiet.Click += btn_chitiet_Click;
            // 
            // btn_reset
            // 
            btn_reset.BorderRadius = 10;
            btn_reset.CustomizableEdges = customizableEdges7;
            btn_reset.DisabledState.BorderColor = Color.DarkGray;
            btn_reset.DisabledState.CustomBorderColor = Color.DarkGray;
            btn_reset.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btn_reset.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btn_reset.FillColor = Color.FromArgb(239, 176, 201);
            btn_reset.Font = new Font("Arial", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_reset.ForeColor = Color.White;
            btn_reset.Location = new Point(660, 82);
            btn_reset.Name = "btn_reset";
            btn_reset.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btn_reset.Size = new Size(192, 47);
            btn_reset.TabIndex = 10;
            btn_reset.Text = "Refresh";
            btn_reset.Click += btn_reset_Click;
            // 
            // dvg_dh
            // 
            dvg_dh.AllowUserToAddRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(252, 211, 241);
            dvg_dh.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dvg_dh.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dvg_dh.BackgroundColor = Color.FromArgb(239, 214, 249);
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(174, 216, 237);
            dataGridViewCellStyle2.Font = new Font("Times New Roman", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dvg_dh.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dvg_dh.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dvg_dh.DefaultCellStyle = dataGridViewCellStyle3;
            dvg_dh.EnableHeadersVisualStyles = false;
            dvg_dh.Location = new Point(28, 135);
            dvg_dh.Name = "dvg_dh";
            dvg_dh.ReadOnly = true;
            dvg_dh.RowHeadersVisible = false;
            dvg_dh.RowHeadersWidth = 20;
            dvg_dh.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dvg_dh.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dvg_dh.Size = new Size(858, 318);
            dvg_dh.TabIndex = 24;
            dvg_dh.CellClick += dvg_dh_CellClick;
            dvg_dh.MouseClick += dvg_dh_MouseClick;
            // 
            // contextMenu_ChiTietDH
            // 
            contextMenu_ChiTietDH.ImageScalingSize = new Size(20, 20);
            contextMenu_ChiTietDH.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1 });
            contextMenu_ChiTietDH.Name = "contextMenuStrip1";
            contextMenu_ChiTietDH.Size = new Size(163, 28);
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(162, 24);
            toolStripMenuItem1.Text = "Xem Chi Tiết";
            toolStripMenuItem1.Click += toolStripMenuItem1_Click;
            // 
            // UC_DonHang
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightYellow;
            Controls.Add(dvg_dh);
            Controls.Add(btn_reset);
            Controls.Add(btn_chitiet);
            Controls.Add(btn_update);
            Controls.Add(label2);
            Controls.Add(cbo_trangthai);
            Controls.Add(label1);
            Name = "UC_DonHang";
            Size = new Size(931, 540);
            Load += UC_DonHang_Load;
            ((System.ComponentModel.ISupportInitialize)dvg_dh).EndInit();
            contextMenu_ChiTietDH.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Guna.UI2.WinForms.Guna2ComboBox cbo_trangthai;
        private Label label2;
        private Guna.UI2.WinForms.Guna2Button btn_update;
        private Guna.UI2.WinForms.Guna2Button btn_chitiet;
        private Guna.UI2.WinForms.Guna2Button btn_reset;
        private DataGridView dvg_dh;
        private ContextMenuStrip contextMenu_ChiTietDH;
        private ToolStripMenuItem toolStripMenuItem1;
    }
}
