using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoAn.Models;
using DoAn.Controler;

namespace DoAn.View.DM
{
    public partial class UC_themDM : UserControl
    {
        private CategoryBLL categoryBLL = new CategoryBLL();
        public UC_themDM()
        {
            InitializeComponent();
        }

        private void btn_them_Click(object sender, EventArgs e)
        {
            string tenDM = txt_tenDM.Text.Trim();
            string ghiChu = txt_note.Text.Trim();

            if (string.IsNullOrEmpty(tenDM))
            {
                MessageBox.Show("Vui lòng nhập tên danh mục!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            CategoryBLL dal = new CategoryBLL();
            bool result = dal.AddCategory(tenDM, ghiChu);

            if (result)
            {
                MessageBox.Show("Thêm danh mục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_tenDM.Clear();
                txt_note.Clear();
            }
            else
            {
                MessageBox.Show("Thêm danh mục thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
