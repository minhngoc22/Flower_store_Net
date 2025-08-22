using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoAn.Controler;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DoAn.View.DM;
using DoAn.View.NV;

namespace DoAn.View
{
    public partial class UC_DanhMuc : UserControl
    {
        private CategoryBLL categoryBLL = new CategoryBLL();
        private string? selectedCategoryCode; // ✅ Sửa tên biến đúng chính tả: Category
        public UC_DanhMuc()
        {
            InitializeComponent();
            LoadData();

            txt_timNCC.Clear(); // Xóa nội dung ô tìm kiếm
            LoadData(); // Tải lại dữ liệu
            dvg_danhmuc.ClearSelection(); // Bỏ chọn hàng SAU KHI load xong
            selectedCategoryCode = null;// Xóa mã đang chọn

        }

        private void LoadData()
        {
            DataTable dt = categoryBLL.GetAllCategories();
            dvg_danhmuc.DataSource = dt;
        }
        private void btn_them_Click(object sender, EventArgs e)
        {
            Form form = new Form();
            UC_themDM uc = new UC_themDM();
            form.Controls.Add(uc);
            uc.Dock = DockStyle.Fill; // Hiển thị UC full Form

            form.StartPosition = FormStartPosition.CenterScreen; // Hiển thị giữa màn hình
            form.Size = new Size(520, 550); // Kích thước cửa sổ
            form.ShowDialog(); // Hiển thị Form chứa UC
        }


        private void btn_update_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedCategoryCode))
            {
                MessageBox.Show("Vui lòng chọn một danh mục để cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                MessageBox.Show(selectedCategoryCode);
                return;
            }

            Form form = new Form();
            UC_updateDM uC = new UC_updateDM(selectedCategoryCode); // ✅ Truyền mã danh mục
            form.Controls.Add(uC);
            uC.Dock = DockStyle.Fill;

            form.StartPosition = FormStartPosition.CenterScreen;
            form.Size = new Size(550, 600);
            form.ShowDialog();

            LoadData(); // Tải lại dữ liệu sau khi cập nhật
        }

        private void dvg_danhmuc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dvg_danhmuc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dvg_danhmuc.Rows[e.RowIndex].Cells["Mã Danh Mục"].Value != null)
            {
                DataGridViewRow row = dvg_danhmuc.Rows[e.RowIndex];
                selectedCategoryCode = row.Cells["Mã Danh Mục"].Value.ToString();
            }
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {

            txt_timNCC.Clear(); // Xóa nội dung ô tìm kiếm
            LoadData(); // Tải lại dữ liệu
            dvg_danhmuc.ClearSelection(); // Bỏ chọn hàng SAU KHI load xong
            selectedCategoryCode = null; // Xóa mã đang chọn
        }

        private void btn_tim_Click(object sender, EventArgs e)
        {
            string keyword = txt_timNCC.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataTable dt = categoryBLL.SearchCategoriesByName(keyword);
            dvg_danhmuc.DataSource = dt;
        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu chưa chọn danh mục nào
            if (string.IsNullOrEmpty(selectedCategoryCode))
            {
                MessageBox.Show("Vui lòng chọn một danh mục để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Hiển thị hộp thoại xác nhận
            DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa danh mục này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Thực hiện xóa danh mục từ cơ sở dữ liệu
                bool deleteResult = categoryBLL.DeleteCategory(selectedCategoryCode);

                // Kiểm tra kết quả và thông báo
                if (deleteResult)
                {
                    MessageBox.Show("Xóa danh mục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData(); // Tải lại dữ liệu sau khi xóa
                }
                else
                {
                    MessageBox.Show("Xóa danh mục thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
