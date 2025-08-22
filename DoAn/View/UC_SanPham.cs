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
using DoAn.Models;
using DoAn.View.SP;
using System.Diagnostics;
using System.IO;
using System.Net; // Thêm thư viện này


namespace DoAn.View
{
    public partial class UC_SanPham : UserControl
    {
        private ProductBLL productBLL = new ProductBLL();
        private string selectedProductID; // Biến lưu ID sản phẩm
        public UC_SanPham()
        {
            InitializeComponent();
        }



        private void LoadData()
        {
            // Lấy dữ liệu từ database
            DataTable dt = productBLL.GetAllProducts();

            dvg_sp.ClearSelection(); // Bỏ chọn hàng trong DataGridView

            // Gán DataTable vào DataGridView
            dvg_sp.DataSource = dt;
            
        }


        // Hàm tạo ảnh mặc định








        private void LoadCategories()
        {
            
            DataTable dt = productBLL.GetAllCategories();

            // Thêm mục "Tất cả" vào danh sách danh mục
            DataRow newRow = dt.NewRow();
            newRow["id"] = 0; // ID = 0 sẽ đại diện cho tất cả
            newRow["category_name"] = "Tất cả";
            dt.Rows.InsertAt(newRow, 0); // Chèn vào đầu danh sách

            if (dt.Rows.Count > 0)
            {
                cbo_danhmuc.DataSource = dt;
                cbo_danhmuc.DisplayMember = "category_name";
                cbo_danhmuc.ValueMember = "id";
                cbo_danhmuc.SelectedIndex = 0; // Mặc định chọn "Tất cả"
            }
        }






        private void UC_SanPham_Load(object sender, EventArgs e)
        {
            LoadData(); // Gọi hàm load dữ liệu khi UserControl khởi động
            LoadCategories();
        }

        private void cbo_danhmuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbo_danhmuc.SelectedValue != null && int.TryParse(cbo_danhmuc.SelectedValue.ToString(), out int categoryId))
            {
                if (categoryId == 0) // Nếu chọn "Tất cả"
                {
                    LoadData(); // Hiển thị tất cả sản phẩm
                }
                else
                {
                    LoadProductsByCategory(categoryId); // Hiển thị sản phẩm theo danh mục
                }
            }
        }

        private void LoadProductsByCategory(int categoryId)
        {
            ProductBLL productBLL = new ProductBLL();
            DataTable dt = productBLL.GetProductsByCategory(categoryId);

            dvg_sp.DataSource = dt; // Hiển thị danh sách sản phẩm lên DataGridView

        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            txt_timSP.Clear(); // Xóa nội dung ô tìm kiếm
            cbo_danhmuc.SelectedIndex = 0; // Chọn lại "Tất cả"
            LoadData(); // Hiển thị lại toàn bộ sản phẩm
            dvg_sp.ClearSelection(); // Bỏ chọn hàng trong DataGridView
            selectedProductID = null; // Xóa ID sản phẩm đã chọn trước đó
        }

        private void btn_timSP_Click(object sender, EventArgs e)
        {
            string keyword = txt_timSP.Text.Trim(); // Lấy nội dung nhập vào ô tìm kiếm

            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Vui lòng nhập từ khóa để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_timSP.Focus(); // Đưa con trỏ vào ô tìm kiếm
                return; // Dừng việc tiếp tục xử lý
            }

            // Nếu có từ khóa thì tiến hành tìm kiếm
            DataTable dt = productBLL.SearchProductsByName(keyword);

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy sản phẩm nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            dvg_sp.DataSource = dt; // Hiển thị danh sách tìm được
        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            if (dvg_sp.SelectedRows.Count > 0) // Kiểm tra nếu có hàng được chọn
            {
                // Kiểm tra giá trị không null trước khi lấy dữ liệu
                object value = dvg_sp.SelectedRows[0].Cells["Mã Sản Phẩm"].Value;
                if (value == null)
                {
                    MessageBox.Show("Không thể lấy mã sản phẩm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string? productId = value as string;
                if (string.IsNullOrEmpty(productId))
                {
                    MessageBox.Show("Không thể lấy mã sản phẩm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                ProductBLL productBLL = new ProductBLL();

                DialogResult confirm = MessageBox.Show("Bạn có chắc muốn xóa sản phẩm này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.Yes)
                {
                    if (!string.IsNullOrEmpty(productId))
                    {
                        if (productBLL.DeleteProduct(productId))
                        {
                            MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show("Không thể xóa sản phẩm! Hãy kiểm tra lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Mã sản phẩm không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_them_Click(object sender, EventArgs e)
        {
            Form form = new Form();
            UC_themSP uc = new UC_themSP();

            form.Controls.Add(uc);
            uc.Dock = DockStyle.Fill; // Hiển thị UC full Form

            form.StartPosition = FormStartPosition.CenterScreen; // Hiển thị giữa màn hình
            form.Size = new Size(1293, 655); // Kích thước cửa sổ
            form.ShowDialog(); // Hiển thị Form chứa UC
            LoadData(); // Tải lại dữ liệu sau khi thêm sản phẩm

            txt_timSP.Clear(); // Xóa nội dung ô tìm kiếm
            cbo_danhmuc.SelectedIndex = 0; // Chọn lại "Tất cả"
           // LoadData(); // Hiển thị lại toàn bộ sản phẩm
            dvg_sp.ClearSelection(); // Bỏ chọn hàng trong DataGridView
            selectedProductID = null; // Xóa ID sản phẩm đã chọn trước đó
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedProductID))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm để cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Chuyển sang giao diện cập nhật sản phẩm
            Form form = new Form();
            UC_update uC = new UC_update(selectedProductID);

            form.Controls.Add(uC);
            uC.Dock = DockStyle.Fill;

            form.StartPosition = FormStartPosition.CenterScreen;
            form.Size = new Size(1170, 670);
            form.ShowDialog();

            LoadData(); // Tải lại dữ liệu sau khi thêm sản phẩm

            txt_timSP.Clear(); // Xóa nội dung ô tìm kiếm
            cbo_danhmuc.SelectedIndex = 0; // Chọn lại "Tất cả"
         //   LoadData(); // Hiển thị lại toàn bộ sản phẩm
            dvg_sp.ClearSelection(); // Bỏ chọn hàng trong DataGridView
            selectedProductID = null; // Xóa ID sản phẩm đã chọn trước đó
        }

        private void dvg_sp_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Đảm bảo không click vào tiêu đề
            {
                DataGridViewRow row = dvg_sp.Rows[e.RowIndex];

                selectedProductID = row.Cells["Mã Sản Phẩm"].Value.ToString();
               
                // Lưu dữ liệu tạm để truyền sang UserControl
            }

           
            

        }
    }
}
