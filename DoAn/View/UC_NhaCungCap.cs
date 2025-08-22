using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoAn.BLL;
using DoAn.View.NV;
using DoAn.View.NCC;

namespace DoAn.View
{
    public partial class UC_NhaCungCap : UserControl
    {
        private SupplierBLL supplierBLL;
        private string? selectedSupplierCode; // Biến lưu ID sản phẩm
        public UC_NhaCungCap()
        {
            InitializeComponent();
            supplierBLL = new SupplierBLL();  // Khởi tạo SupplierBLL
        }

        private void UC_NhaCungCap_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadSupplierAddresses();
        }

        private void LoadData()
        {
            DataTable suppliers = supplierBLL.GetAllSuppliers();
            dvg_ncc.DataSource = suppliers;
        }

        // 🟢 Load danh sách địa chỉ lên ComboBox
        private void LoadSupplierAddresses()
        {
            DataTable dt = supplierBLL.GetAllSupplierAddresses(); // Lấy danh sách địa chỉ nhà cung cấp

            if (dt.Rows.Count > 0)
            {
                // Thêm lựa chọn "Tất cả" vào danh sách địa chỉ
                DataRow allRow = dt.NewRow();
                allRow["address"] = "Tất cả";
                dt.Rows.InsertAt(allRow, 0); // Chèn vào vị trí đầu tiên

                cbo_diachi.DataSource = dt;
                cbo_diachi.DisplayMember = "address"; // Hiển thị địa chỉ
                cbo_diachi.ValueMember = "address";   // Giá trị khi chọn

                // 🔹 Mặc định chọn "Tất cả"
                cbo_diachi.SelectedIndex = 0;
            }

            LoadData(); // ✅ Hiển thị danh sách nhà cung cấp ngay khi form load
        }



        private void cbo_diachi_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cbo_diachi.SelectedValue != null)
            {
                string selectedAddress = cbo_diachi.SelectedValue?.ToString() ?? string.Empty;


                if (!string.IsNullOrEmpty(selectedAddress))
                {
                    if (selectedAddress == "Tất cả")
                    {
                        LoadData(); // Hiển thị toàn bộ nhà cung cấp
                    }
                    else
                    {
                        LoadSuppliersByAddress(selectedAddress);
                    }
                }
            }
        }
        private void LoadSuppliersByAddress(string address)
        {
            DataTable dt = supplierBLL.GetSuppliersByAddress(address);
            dvg_ncc.DataSource = dt;
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            txt_timNCC.Clear(); // Xóa nội dung ô tìm kiếm
            LoadSupplierAddresses(); // ✅ Nạp lại danh sách địa chỉ từ đầu
            cbo_diachi.SelectedIndex = 0; // Chọn lại "Tất cả"
            LoadData(); // ✅ Hiển thị lại toàn bộ danh sách nhân viên
            dvg_ncc.ClearSelection(); // Bỏ chọn hàng trong DataGridView
        }

        private void btn_tim_Click(object sender, EventArgs e)
        {
            string keyword = txt_timNCC.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Vui lòng nhập tên nhân viên để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_timNCC.Focus();
                return;
            }

            DataTable dt = supplierBLL.SearchSuppliersByName(keyword);

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy nhân viên nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            dvg_ncc.DataSource = dt; // Vẫn cập nhật danh sách để hiển thị trống nếu không có kết quả
        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            if (dvg_ncc.SelectedRows.Count > 0)
            {
                object value = dvg_ncc.SelectedRows[0].Cells["Mã Nhà Cung Cấp"].Value;
                if (value == null)
                {
                    MessageBox.Show("Không thể lấy mã nhân viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string? nccCode = value as string;
                if (string.IsNullOrEmpty(nccCode))
                {
                    MessageBox.Show("Mã nhân viên không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DialogResult confirm = MessageBox.Show($"Bạn có chắc muốn xóa nhân viên có mã {nccCode}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.Yes)
                {
                    if (supplierBLL.DeleteSupplier(nccCode))
                    {
                        MessageBox.Show($"Xóa nhân viên có mã {nccCode} thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Không thể xóa nhân viên! Hãy kiểm tra lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedSupplierCode))
            {
                MessageBox.Show("Vui lòng chọn 1 nhà cung cấp để cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
               
                return;
            }
            Form form = new Form();
            UC_updateNCC uC = new UC_updateNCC(selectedSupplierCode);
            form.Controls.Add(uC);
            uC.Dock = DockStyle.Fill; // Hiển thị UC full Form

            form.StartPosition = FormStartPosition.CenterScreen; // Hiển thị giữa màn hình
            form.Size = new Size(640, 550); // Kích thước cửa sổ
            form.ShowDialog(); // Hiển thị Form chứa UC
        }

        private void btn_them_Click(object sender, EventArgs e)
        {
            Form form = new Form();
            UC_themNCC uc = new UC_themNCC();
            form.Controls.Add(uc);
            uc.Dock = DockStyle.Fill; // Hiển thị UC full Form

            form.StartPosition = FormStartPosition.CenterScreen; // Hiển thị giữa màn hình
            form.Size = new Size(605, 470); // Kích thước cửa sổ
            form.ShowDialog(); // Hiển thị Form chứa UC
        }

        private void dvg_ncc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Đảm bảo không click vào tiêu đề
            {
                DataGridViewRow row = dvg_ncc.Rows[e.RowIndex];
                selectedSupplierCode = row.Cells["Mã Nhà Cung Cấp"].Value?.ToString(); // Cột chứa mã nhà cung cấp
                Console.WriteLine($"Selected Supplier Code: {selectedSupplierCode}"); // Debug
            }
        }
    }
}
