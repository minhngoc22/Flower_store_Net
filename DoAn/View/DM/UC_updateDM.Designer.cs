namespace DoAn.View.DM
{
    partial class UC_updateDM
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            txt_note = new TextBox();
            label8 = new Label();
            btn_them = new Guna.UI2.WinForms.Guna2Button();
            label3 = new Label();
            txt_tenDM = new Guna.UI2.WinForms.Guna2TextBox();
            label1 = new Label();
            txt_maDM = new Guna.UI2.WinForms.Guna2TextBox();
            label2 = new Label();
            SuspendLayout();
            // 
            // txt_note
            // 
            txt_note.Location = new Point(84, 250);
            txt_note.Multiline = true;
            txt_note.Name = "txt_note";
            txt_note.Size = new Size(342, 213);
            txt_note.TabIndex = 65;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Arial", 12F, FontStyle.Bold);
            label8.ForeColor = Color.FromArgb(131, 139, 237);
            label8.Location = new Point(198, 223);
            label8.Name = "label8";
            label8.Size = new Size(87, 24);
            label8.TabIndex = 64;
            label8.Text = "Ghi Chú";
            // 
            // btn_them
            // 
            btn_them.BorderRadius = 10;
            btn_them.CustomizableEdges = customizableEdges1;
            btn_them.DisabledState.BorderColor = Color.DarkGray;
            btn_them.DisabledState.CustomBorderColor = Color.DarkGray;
            btn_them.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btn_them.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btn_them.FillColor = Color.FromArgb(239, 176, 201);
            btn_them.Font = new Font("Arial", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_them.ForeColor = Color.White;
            btn_them.Location = new Point(129, 469);
            btn_them.Name = "btn_them";
            btn_them.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btn_them.Size = new Size(233, 55);
            btn_them.TabIndex = 63;
            btn_them.Text = "Thêm";
            btn_them.Click += btn_them_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Arial", 12F, FontStyle.Bold);
            label3.ForeColor = Color.FromArgb(131, 139, 237);
            label3.Location = new Point(170, 138);
            label3.Name = "label3";
            label3.Size = new Size(147, 24);
            label3.TabIndex = 62;
            label3.Text = "Tên Danh Mục";
            // 
            // txt_tenDM
            // 
            txt_tenDM.BorderRadius = 10;
            txt_tenDM.BorderThickness = 2;
            txt_tenDM.CustomizableEdges = customizableEdges3;
            txt_tenDM.DefaultText = "";
            txt_tenDM.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txt_tenDM.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txt_tenDM.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txt_tenDM.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txt_tenDM.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txt_tenDM.Font = new Font("Segoe UI", 9F);
            txt_tenDM.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txt_tenDM.Location = new Point(129, 166);
            txt_tenDM.Margin = new Padding(3, 4, 3, 4);
            txt_tenDM.Name = "txt_tenDM";
            txt_tenDM.PlaceholderText = "";
            txt_tenDM.SelectedText = "";
            txt_tenDM.ShadowDecoration.CustomizableEdges = customizableEdges4;
            txt_tenDM.Size = new Size(225, 53);
            txt_tenDM.TabIndex = 61;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Arial", 24F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(200, 93, 100);
            label1.Location = new Point(33, 0);
            label1.Name = "label1";
            label1.Size = new Size(460, 46);
            label1.TabIndex = 60;
            label1.Text = "CẬP NHẬT DANH MỤC ";
            label1.Click += label1_Click;
            // 
            // txt_maDM
            // 
            txt_maDM.BorderRadius = 10;
            txt_maDM.BorderThickness = 2;
            txt_maDM.CustomizableEdges = customizableEdges5;
            txt_maDM.DefaultText = "";
            txt_maDM.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txt_maDM.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txt_maDM.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txt_maDM.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txt_maDM.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txt_maDM.Font = new Font("Segoe UI", 9F);
            txt_maDM.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txt_maDM.Location = new Point(129, 81);
            txt_maDM.Margin = new Padding(3, 4, 3, 4);
            txt_maDM.Name = "txt_maDM";
            txt_maDM.PlaceholderText = "";
            txt_maDM.SelectedText = "";
            txt_maDM.ShadowDecoration.CustomizableEdges = customizableEdges6;
            txt_maDM.Size = new Size(225, 53);
            txt_maDM.TabIndex = 61;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Arial", 12F, FontStyle.Bold);
            label2.ForeColor = Color.FromArgb(131, 139, 237);
            label2.Location = new Point(170, 53);
            label2.Name = "label2";
            label2.Size = new Size(139, 24);
            label2.TabIndex = 62;
            label2.Text = "Mã Danh Mục";
            // 
            // UC_updateDM
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightYellow;
            Controls.Add(txt_note);
            Controls.Add(label8);
            Controls.Add(btn_them);
            Controls.Add(label2);
            Controls.Add(label3);
            Controls.Add(txt_maDM);
            Controls.Add(txt_tenDM);
            Controls.Add(label1);
            Name = "UC_updateDM";
            Size = new Size(503, 552);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txt_note;
        private Label label8;
        private Guna.UI2.WinForms.Guna2Button btn_them;
        private Label label3;
        private Guna.UI2.WinForms.Guna2TextBox txt_tenDM;
        private Label label1;
        private Guna.UI2.WinForms.Guna2TextBox txt_maDM;
        private Label label2;
    }
}
