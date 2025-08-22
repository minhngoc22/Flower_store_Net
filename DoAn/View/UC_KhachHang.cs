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
using DoAn.View.KH;

namespace DoAn.View
{
    public partial class UC_KhachHang : UserControl
    {
        private CustomerBLL customerBLL;
        private string? selectedCustomerCode; // Lưu mã khách hàng đang được chọn

        public UC_KhachHang()
        {
            InitializeComponent();
            customerBLL = new CustomerBLL();

        }

        //sự kiện reset
        private void btn_reset_Click(object sender, EventArgs e)
        {
            txt_timKH.Clear(); // Xóa nội dung ô tìm kiếm
            LoadCustomerCities(); // Tải danh sách địa chỉ khách hàng
            cbo_diachi.SelectedIndex = 0; // Đặt lại combobox địa chỉ về mặc định
            LoadData(); // Tải lại danh sách khách hàng
            dvg_kh.ClearSelection(); // Bỏ chọn tất cả dòng trong DataGridView
        }

        private void UC_KhachHang_Load(object sender, EventArgs e)
        {

            LoadData(); // Tải dữ liệu khách hàng khi UserControl được tải
            LoadCustomerCities(); // Tải danh sách địa chỉ khách hàng
        }
        // load dữ liệu lên dvg
        // Phương thức tải danh sách khách hàng lên DataGridView
        private void LoadData()
        {
            DataTable customers = customerBLL.GetAllCustomers(); // Lấy dữ liệu từ BLL
            dvg_kh.DataSource = customers; // Gán dữ liệu vào DataGridView
        }

        // Phương thức tải danh sách địa chỉ khách hàng lên combobox
        private void LoadCustomerCities()
        {
            DataTable dt = customerBLL.GetAllCustomerAddresses(); // Lấy dữ liệu từ BLL

            if (dt.Rows.Count > 0)
            {
                DataRow allRow = dt.NewRow();
                allRow["address"] = "Tất cả";
                dt.Rows.InsertAt(allRow, 0);

                cbo_diachi.DataSource = dt;
                cbo_diachi.DisplayMember = "address";
                cbo_diachi.ValueMember = "address";
                cbo_diachi.SelectedIndex = 0;
            }

            LoadData();
        }

        //lọc khách hàng theo địa chỉ được chọn
        private void cbo_diachi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbo_diachi.SelectedValue != null)
            {
                string selectedCity = cbo_diachi.SelectedValue?.ToString() ?? string.Empty;

                if (!string.IsNullOrEmpty(selectedCity))
                {
                    if (selectedCity == "Tất cả")
                    {
                        LoadData(); // Hiển thị tất cả khách hàng
                    }
                    else
                    {
                        LoadCustomersByCity(selectedCity); // Hiển thị khách hàng theo địa chỉ
                    }
                }
            }
        }


        // Lấy danh sách khách hàng theo địa chỉ được chọn
        private void LoadCustomersByCity(string city)
        {
            DataTable dt = customerBLL.GetCustomersByAddress(city);
            dvg_kh.DataSource = dt;
        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            if (dvg_kh.SelectedRows.Count > 0)
            {
                object value = dvg_kh.SelectedRows[0].Cells["Mã Khách Hàng"].Value;
                if (value == null)
                {
                    MessageBox.Show("Không thể lấy mã khách hàng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string? customerCode = value as string;
                if (string.IsNullOrEmpty(customerCode))
                {
                    MessageBox.Show("Mã khách hàng không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DialogResult confirm = MessageBox.Show($"Bạn có chắc muốn xóa khách hàng có mã {customerCode}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.Yes)
                {
                    if (customerBLL.DeleteCustomer(customerCode))
                    {
                        MessageBox.Show($"Xóa khách hàng có mã {customerCode} thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Không thể xóa khách hàng! Hãy kiểm tra lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_tim_Click(object sender, EventArgs e)
        {
            string keyword = txt_timKH.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_timKH.Focus();
                return;
            }

            DataTable dt = customerBLL.SearchCustomersByPhoneOrEmail(keyword);

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy khách hàng nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            dvg_kh.DataSource = dt;
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedCustomerCode))
            {
                MessageBox.Show("Vui lòng chọn 1 khách hàng để cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Form form = new Form();
            UC_updateKh uC = new UC_updateKh(selectedCustomerCode);
            form.Controls.Add(uC);
            uC.Dock = DockStyle.Fill;

            form.StartPosition = FormStartPosition.CenterScreen;
            form.Size = new Size(640, 550);
            form.ShowDialog();
            // Nếu form đóng với kết quả OK, tải lại dữ liệu
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void btn_them_Click(object sender, EventArgs e)
        {
            using (Form form = new Form())
            {
                UC_themKH ucThemKH = new UC_themKH("", false); // isFromOrder = false

                ucThemKH.CustomerAdded += (senderKH, newCustomerCode) =>
                {
                    MessageBox.Show($"Khách hàng {newCustomerCode} đã được thêm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                };

                form.Controls.Add(ucThemKH);
                ucThemKH.Dock = DockStyle.Fill;

                form.StartPosition = FormStartPosition.CenterScreen;
                form.Size = new Size(640, 550);
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.MaximizeBox = false;
                form.MinimizeBox = false;

                form.ShowDialog();
            }
        }

        private void dvg_kh_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dvg_kh.Rows[e.RowIndex];
                selectedCustomerCode = row.Cells["Mã Khách Hàng"].Value?.ToString();
                Console.WriteLine($"Selected Customer Code: {selectedCustomerCode}");
            }
        }
    }
}
