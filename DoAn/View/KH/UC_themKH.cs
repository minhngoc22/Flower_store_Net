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
using System.Text.RegularExpressions;
using DoAn.View.DH;
using DoAn.View.User;
using DoAn.Models;

namespace DoAn.View.KH
{
    public partial class UC_themKH : UserControl

    {
        private bool isFromOrder; // Biến kiểm tra gọi từ đâu
        public event EventHandler<string> CustomerAdded; // Sự kiện khi thêm khách hàng thành công

        private string customerInfo;
        // Constructor mới nhận thông tin khách hàng
        public UC_themKH(string customerInfo = " ", bool isFromOrder = false)
        {
            InitializeComponent();
            this.customerInfo = customerInfo;
            this.isFromOrder = isFromOrder; // Lưu trạng thái gọi từ UC_themDH hay UC_kh

            // Gán thông tin vào ô nhập liệu nếu là số điện thoại hoặc email
            if (Regex.IsMatch(customerInfo, @"^\d+$")) // Nếu là số điện thoại
            {
                txt_sdt.Text = customerInfo;
            }
            else if (Regex.IsMatch(customerInfo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) // Nếu là email
            {
                txt_email.Text = customerInfo;
            }
        }


        private void btn_them_Click(object sender, EventArgs e)
        {

            // Lấy dữ liệu từ các ô nhập liệu
            string tenKH = txt_tenKH.Text.Trim();
            string diaChi = txt_diachi.Text.Trim();
            string sdt = txt_sdt.Text.Trim();
            string email = txt_email.Text.Trim();
            string note = txt_note.Text.Trim();

            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrEmpty(tenKH) ||
                string.IsNullOrEmpty(diaChi) || string.IsNullOrEmpty(sdt) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra số điện thoại có hợp lệ không
            if (!long.TryParse(sdt, out _))
            {
                MessageBox.Show("Số điện thoại không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kiểm tra định dạng email
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Email không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Gọi BLL để thêm khách hàng vào database
            CustomerBLL customerBLL = new CustomerBLL();
            bool isAdded = customerBLL.AddCustomer(tenKH, sdt, email, diaChi, note);

            if (isAdded)
            {
                MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Hỏi người dùng có muốn tạo tài khoản không
                DialogResult result = MessageBox.Show("Bạn có muốn tạo tài khoản cho khách hàng này không?", "Xác nhận",
                                                      MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // Mở form thêm user và truyền thông tin khách hàng
                    Form form = new Form();

                    UC_themND uc = new UC_themND(tenKH, email);

                    form.Controls.Add(uc);
                    uc.Dock = DockStyle.Fill;

                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.Size = new Size(670, 470);
                    form.ShowDialog();
                }

                ClearFields();
                ((Form)this.Parent).Close(); // Đóng form sau khi thêm xong

                // Gửi sự kiện CustomerAdded
                CustomerAdded?.Invoke(this, sdt);
            }
            else
            {
                MessageBox.Show("Thêm thất bại! Khách hàng có thể đã tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Hàm xóa dữ liệu sau khi thêm thành công
        private void ClearFields()
        {
            txt_tenKH.Clear();
            txt_diachi.Clear();
            txt_sdt.Clear();
            txt_email.Clear();
            txt_note.Clear();
        }

        private void txt_tenKH_TextChanged(object sender, EventArgs e)
        {
            string keyword = txt_tenKH.Text.Trim();

            if (keyword.Length >= 10) // chỉ tìm khi nhập từ 2 ký tự trở lên
            {
                CustomerDAL dal = new CustomerDAL();
                DataTable result = dal.SearchCustomers(keyword);

                if (result.Rows.Count > 0)
                {
                    // Hiển thị thông tin của khách hàng đầu tiên khớp
                    DataRow row = result.Rows[0];
                    txt_tenKH.Text = row["Tên Khách Hàng"].ToString();
                    txt_sdt.Text = row["Số Điện Thoại"].ToString();
                    txt_email.Text = row["Email"].ToString();
                    txt_diachi.Text = row["Địa Chỉ"].ToString();
                    txt_note.Text = row["Ghi Chú"].ToString();
                }
                else
                {
                    // Nếu không có khách nào khớp, xóa các ô còn lại
                    txt_sdt.Clear();
                    txt_email.Clear();
                    txt_diachi.Clear();
                    txt_note.Clear();
                }
            }
            else
            {
                // Nếu chưa nhập đủ ký tự thì clear
                txt_sdt.Clear();
                txt_email.Clear();
                txt_diachi.Clear();
                txt_note.Clear();
            }

        }

        private void btn_them_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void txt_sdt_TextChanged(object sender, EventArgs e)
        {
            string phone = txt_sdt.Text.Trim();

            if (phone.Length >= 10) // chỉ tìm khi nhập từ 3 ký tự trở lên
            {
                CustomerDAL dal = new CustomerDAL();
                DataTable result = dal.SearchCustomersByPhone(phone);

                if (result.Rows.Count > 0)
                {
                    DataRow row = result.Rows[0];
                    txt_tenKH.Text = row["Tên Khách Hàng"].ToString();
                    txt_sdt.Text = row["Số Điện Thoại"].ToString(); // không bắt buộc
                    txt_email.Text = row["Email"].ToString();
                    txt_diachi.Text = row["Địa Chỉ"].ToString();
                    txt_note.Text = row["Ghi Chú"].ToString();
                }
                else
                {
                    txt_tenKH.Clear();
                    txt_email.Clear();
                    txt_diachi.Clear();
                    txt_note.Clear();
                }
            }
            else
            {
                txt_tenKH.Clear();
                txt_email.Clear();
                txt_diachi.Clear();
                txt_note.Clear();
            }
        }

        private void btn_muahang_Click(object sender, EventArgs e)
        {
            string sdt = txt_sdt.Text.Trim();  // Số điện thoại nhập từ người dùng
            string diaChiGiaoHang = null;      // Địa chỉ giao hàng mặc định là null

            if (string.IsNullOrEmpty(sdt))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra xem khách hàng có tồn tại không
            CustomerBLL customerBLL = new CustomerBLL();
            DataTable dtCustomer = customerBLL.SearchCustomersByPhone(sdt);  // Tìm khách hàng theo số điện thoại

            if (dtCustomer.Rows.Count > 0)
            {
                string customerCode = dtCustomer.Rows[0]["Mã Khách Hàng"].ToString();  // Lấy mã khách hàng từ kết quả tìm kiếm
                MessageBox.Show($"Khách hàng {customerCode}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Truyền số điện thoại (sdt) vào AddOrder để tạo đơn hàng
                TaoDonHang(sdt, customerCode, diaChiGiaoHang);  // Truyền số điện thoại và mã khách hàng vào hàm
            }
            else
            {
                // Nếu khách hàng chưa tồn tại, mở form thêm khách hàng
                using (Form form = new Form())
                {
                    UC_themKH ucThemKH = new UC_themKH(sdt, true);  // Chuyển số điện thoại vào form thêm khách hàng
                    ucThemKH.CustomerAdded += (senderKH, newCustomerCode) =>
                    {
                        MessageBox.Show($"Khách hàng {newCustomerCode} đã được thêm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        TaoDonHang(sdt, newCustomerCode, diaChiGiaoHang);  // Truyền số điện thoại và mã khách hàng mới vào hàm
                    };

                    form.Controls.Add(ucThemKH);
                    ucThemKH.Dock = DockStyle.Fill;
                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.Size = new Size(600, 550);
                    form.FormBorderStyle = FormBorderStyle.FixedDialog;
                    form.MaximizeBox = false;
                    form.MinimizeBox = false;

                    form.ShowDialog();
                }
            }
        }

        // Hàm tạo đơn hàng
        private void TaoDonHang(string sdt, string customerCode, string diaChiGiaoHang)
        {
            // Tạo đơn hàng với số điện thoại (sdt) và mã khách hàng (customerCode)
            OrderBLL orderBLL = new OrderBLL();
            string tenNV = null;  // Không chọn nhân viên, để NULL

            // Sử dụng số điện thoại (sdt) để tạo đơn hàng
            string maDH = orderBLL.AddOrder(sdt, tenNV, diaChiGiaoHang);  // Truyền số điện thoại vào AddOrder

            if (!string.IsNullOrEmpty(maDH))
            {
                MessageBox.Show($"Tạo đơn hàng thành công!\nMã đơn: {maDH}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Mở form chi tiết đơn hàng và truyền mã khách hàng vào chi tiết đơn hàng
                UC_themchitietDH uc = new UC_themchitietDH(maDH, customerCode);  // Truyền mã khách hàng vào chi tiết đơn hàng
                Form form = new Form();
                form.Controls.Add(uc);
                uc.Dock = DockStyle.Fill;

                form.StartPosition = FormStartPosition.CenterScreen;
                form.Size = new Size(1250, 730);
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.MaximizeBox = false;
                form.MinimizeBox = false;

                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("Tạo đơn hàng thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
