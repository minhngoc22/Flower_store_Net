using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoAn.Controler;

namespace DoAn.View.DM
{
    public partial class UC_updateDM : UserControl
    {
        private string categoryCode;
        private CategoryBLL categoryBLL = new CategoryBLL();

        public UC_updateDM(string catCode)
        {
            InitializeComponent();
            categoryCode = catCode;
            LoadCategoryData();
            txt_maDM.Enabled = false;

        }

        private void LoadCategoryData()
        {
            DataTable dt = categoryBLL.GetCategoryByCode(categoryCode);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                txt_maDM.Text = row["Mã Danh Mục"].ToString();
                txt_tenDM.Text = row["Tên Danh Mục"].ToString();
                txt_note.Text = row["Ghi Chú"].ToString();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btn_them_Click(object sender, EventArgs e)
        {
            string name = txt_tenDM.Text.Trim();
            string note = txt_note.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Tên danh mục không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool result = categoryBLL.UpdateCategory(categoryCode, name, note);

            if (result)
            {
                MessageBox.Show("Cập nhật danh mục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Có thể thêm đoạn: đóng Form nếu đây là một Form chứa UC
                this.Parent?.Dispose(); // hoặc ((Form)this.TopLevelControl).Close();
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
