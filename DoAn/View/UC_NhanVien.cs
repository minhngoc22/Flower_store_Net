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
using DoAn.View.NV;

namespace DoAn.View
{
    public partial class UC_NhanVien : UserControl
    {
        private NhanVienBLL nvBLL = new NhanVienBLL();
        private string? selectedEmployeeCode; // Biến lưu ID sản phẩm
        public UC_NhanVien()
        {
            InitializeComponent();
            dvg_nv.ClearSelection(); // Bỏ chọn hàng trong DataGridView
            selectedEmployeeCode = null; // Khởi tạo biến ID nhân viên
        }

        private void UC_NhanVien_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadEmployeeAddresses();
        }
        private void LoadData()
        {
            // Lấy dữ liệu từ database
            DataTable dt = nvBLL.GetAllEmployees();



            // Gán DataTable vào DataGridView
            dvg_nv.DataSource = dt;

        }
        private void LoadEmployeeAddresses()
        {
            DataTable dt = nvBLL.GetSortedEmployeeAddresses();

            if (dt.Rows.Count > 0)
            {
                // Thêm lựa chọn "Tất cả" vào danh sách địa chỉ
                DataRow allRow = dt.NewRow();
                allRow["address"] = "Tất cả";
                dt.Rows.InsertAt(allRow, 0); // Chèn vào vị trí đầu tiên


                cbo_diachi.DataSource = dt;
                cbo_diachi.DisplayMember = "address";
                cbo_diachi.ValueMember = "address";

                // 🔹 Mặc định chọn "Tất cả"
                cbo_diachi.SelectedIndex = 0;
            }
            LoadData(); // ✅ Đảm bảo dữ liệu nhân viên hiển thị ngay khi form vừa load
        }


        private void cbo_diachi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbo_diachi.SelectedValue != null)
            {
                string? selectedAddress = cbo_diachi.SelectedValue?.ToString();
                if (!string.IsNullOrEmpty(selectedAddress))
                {
                    if (selectedAddress == "Tất cả")
                    {
                        LoadData();
                    }
                    else
                    {
                        LoadEmployeesByAddress(selectedAddress);
                    }
                }

            }
        }
        private void LoadEmployeesByAddress(string address)
        {
            DataTable dt = nvBLL.GetEmployeesByAddress(address);
            dvg_nv.DataSource = dt;
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {

            txt_tim.Clear(); // 1. Xóa nội dung ô tìm kiếm
            cbo_diachi.SelectedIndex = 0; // 2. Đặt lại Combobox địa chỉ về mặc định (Tất cả)
            LoadEmployeeAddresses(); // 3. Nạp lại danh sách địa chỉ (sau khi đã set SelectedIndex)
            LoadData(); // 4. Tải lại toàn bộ danh sách nhân viên
            dvg_nv.ClearSelection(); // 5. Bỏ chọn tất cả dòng trong DataGridView
            selectedEmployeeCode = null; // 6. Xóa mã nhân viên đang chọn
        }

        private void btn_tim_Click(object sender, EventArgs e)
        {
            string keyword = txt_tim.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Vui lòng nhập tên nhân viên để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_tim.Focus();
                return;
            }

            DataTable dt = nvBLL.SearchEmployeeByName(keyword);

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy nhân viên nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            dvg_nv.DataSource = dt; // Vẫn cập nhật danh sách để hiển thị trống nếu không có kết quả
        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            if (dvg_nv.SelectedRows.Count > 0)
            {
                object value = dvg_nv.SelectedRows[0].Cells["Mã Nhân Viên"].Value;
                if (value == null)
                {
                    MessageBox.Show("Không thể lấy mã nhân viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string? nvCode = value as string;
                if (string.IsNullOrEmpty(nvCode))
                {
                    MessageBox.Show("Mã nhân viên không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DialogResult confirm = MessageBox.Show($"Bạn có chắc muốn xóa nhân viên có mã {nvCode}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.Yes)
                {
                    if (nvBLL.DeleteEmployee(nvCode))
                    {
                        MessageBox.Show($"Xóa nhân viên có mã {nvCode} thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btn_them_Click(object sender, EventArgs e)
        {
            Form form = new Form();
            UC_themNV uc = new UC_themNV();
            form.Controls.Add(uc);
            uc.Dock = DockStyle.Fill; // Hiển thị UC full Form

            form.StartPosition = FormStartPosition.CenterScreen; // Hiển thị giữa màn hình
            form.Size = new Size(915, 696); // Kích thước cửa sổ
            form.ShowDialog(); // Hiển thị Form chứa UC
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedEmployeeCode))
            {
                MessageBox.Show("Vui lòng chọn 1 nhân viên để cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                MessageBox.Show(selectedEmployeeCode);
                return;
            }
            Form form = new Form();
            UC_updateNV uC = new UC_updateNV(selectedEmployeeCode);
            form.Controls.Add(uC);
            uC.Dock = DockStyle.Fill; // Hiển thị UC full Form

            form.StartPosition = FormStartPosition.CenterScreen; // Hiển thị giữa màn hình
            form.Size = new Size(940, 705); // Kích thước cửa sổ
            form.ShowDialog(); // Hiển thị Form chứa UC
        }

        private void dvg_nv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Đảm bảo không click vào tiêu đề
            {
                DataGridViewRow row = dvg_nv.Rows[e.RowIndex];

                selectedEmployeeCode = row.Cells["Mã Nhân Viên"].Value.ToString();

                // Lưu dữ liệu tạm để truyền sang UserControl
            }
        }
    }
}
