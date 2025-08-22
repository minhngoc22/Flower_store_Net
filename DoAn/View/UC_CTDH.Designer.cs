namespace DoAn.View
{
    partial class UC_CTDH
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            label4 = new Label();
            txt_tongtien = new Guna.UI2.WinForms.Guna2TextBox();
            dvg_ctdh = new DataGridView();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)dvg_ctdh).BeginInit();
            SuspendLayout();
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Arial", 12F, FontStyle.Bold);
            label4.ForeColor = Color.FromArgb(131, 139, 237);
            label4.Location = new Point(653, 403);
            label4.Name = "label4";
            label4.Size = new Size(106, 24);
            label4.TabIndex = 96;
            label4.Text = "Tổng Tiền";
            // 
            // txt_tongtien
            // 
            txt_tongtien.BorderRadius = 10;
            txt_tongtien.BorderThickness = 2;
            txt_tongtien.CustomizableEdges = customizableEdges3;
            txt_tongtien.DefaultText = "";
            txt_tongtien.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txt_tongtien.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txt_tongtien.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txt_tongtien.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txt_tongtien.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txt_tongtien.Font = new Font("Segoe UI", 9F);
            txt_tongtien.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txt_tongtien.Location = new Point(765, 391);
            txt_tongtien.Margin = new Padding(3, 4, 3, 4);
            txt_tongtien.Name = "txt_tongtien";
            txt_tongtien.PlaceholderText = "";
            txt_tongtien.SelectedText = "";
            txt_tongtien.ShadowDecoration.CustomizableEdges = customizableEdges4;
            txt_tongtien.Size = new Size(203, 47);
            txt_tongtien.TabIndex = 95;
            // 
            // dvg_ctdh
            // 
            dvg_ctdh.AllowUserToAddRows = false;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(252, 211, 241);
            dvg_ctdh.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dvg_ctdh.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dvg_ctdh.BackgroundColor = Color.FromArgb(239, 214, 249);
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = Color.FromArgb(174, 216, 237);
            dataGridViewCellStyle5.Font = new Font("Times New Roman", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle5.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            dvg_ctdh.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            dvg_ctdh.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = SystemColors.Window;
            dataGridViewCellStyle6.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle6.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
            dvg_ctdh.DefaultCellStyle = dataGridViewCellStyle6;
            dvg_ctdh.EnableHeadersVisualStyles = false;
            dvg_ctdh.Location = new Point(15, 72);
            dvg_ctdh.Name = "dvg_ctdh";
            dvg_ctdh.ReadOnly = true;
            dvg_ctdh.RowHeadersVisible = false;
            dvg_ctdh.RowHeadersWidth = 20;
            dvg_ctdh.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dvg_ctdh.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dvg_ctdh.Size = new Size(980, 312);
            dvg_ctdh.TabIndex = 94;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Arial", 24F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(200, 93, 100);
            label1.Location = new Point(309, 10);
            label1.Name = "label1";
            label1.Size = new Size(416, 46);
            label1.TabIndex = 97;
            label1.Text = "CHI TIẾT ĐƠN HÀNG";
            // 
            // UC_CTDH
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightYellow;
            Controls.Add(label1);
            Controls.Add(label4);
            Controls.Add(txt_tongtien);
            Controls.Add(dvg_ctdh);
            Name = "UC_CTDH";
            Size = new Size(1024, 462);
            Load += UC_CTDH_Load;
            ((System.ComponentModel.ISupportInitialize)dvg_ctdh).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label4;
        private Guna.UI2.WinForms.Guna2TextBox txt_tongtien;
        private DataGridView dvg_ctdh;
        private Label label1;
    }
}
