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

namespace DoAn.View.KH
{
    public partial class UC_updateKh : UserControl
    {
        private string customerCode; // Mã khách hàng cần cập nhật
        public UC_updateKh(string cusCode)
        {
            InitializeComponent();
            customerCode = cusCode;
            LoadCustomerData(); // Tải dữ liệu khách hàng lên form
        }

        //load dữ liệu form lên
        // Phương thức tải dữ liệu khách hàng lên form
        private void LoadCustomerData()
        {
            CustomerBLL customerBLL = new CustomerBLL();
            DataTable dt = customerBLL.GetCustomerByCode(customerCode);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                txt_maKH.Text = row["Mã Khách Hàng"].ToString();
                txt_tenKH.Text = row["Tên Khách Hàng"].ToString();
                txt_sdt.Text = row["Số Điện Thoại"].ToString();
                txt_email.Text = row["Email"].ToString();
                txt_diachi.Text = row["Địa Chỉ"].ToString();
                txt_note.Text = row["Ghi Chú"].ToString();
            }
            else
            {
                MessageBox.Show("Không tìm thấy khách hàng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //sự kiện lưu 
        private void btn_luu_Click(object sender, EventArgs e)
        {
            try
            {
                string maKH = txt_maKH.Text;
                string tenKH = txt_tenKH.Text;
                string sdt = txt_sdt.Text;
                string email = txt_email.Text;
                string diaChi = txt_diachi.Text;
                string note = txt_note.Text;

                // Kiểm tra các trường bắt buộc
                if (string.IsNullOrEmpty(tenKH) || string.IsNullOrEmpty(diaChi) ||
                    string.IsNullOrEmpty(sdt) || string.IsNullOrEmpty(email))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra số điện thoại có hợp lệ hay không (chỉ chứa số)
                if (!long.TryParse(sdt, out _))
                {
                    MessageBox.Show("Số điện thoại không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Gọi BLL để cập nhật thông tin khách hàng
                CustomerBLL customerBLL = new CustomerBLL();
                bool success = customerBLL.UpdateCustomer(maKH, tenKH, sdt, email, diaChi, note);

                if (success)
                {
                    MessageBox.Show("Cập nhật khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.ParentForm?.Close(); // Đóng form sau khi cập nhật thành công
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
