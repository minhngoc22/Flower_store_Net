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

namespace DoAn.View.NCC
{
    public partial class UC_themNCC : UserControl
    {
        public UC_themNCC()
        {
            InitializeComponent();
        }

        private void btn_them_Click(object sender, EventArgs e)
        {
            // Lấy dữ liệu từ TextBox
            
            string tenNCC = txt_tenNCC.Text.Trim();
            string diaChi = txt_diachi.Text.Trim();
            string sdt = txt_sdt.Text.Trim();
            string email = txt_email.Text.Trim();
            string note = txt_note.Text.Trim();

            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrEmpty(tenNCC) ||
                string.IsNullOrEmpty(diaChi) || string.IsNullOrEmpty(sdt)|| string.IsNullOrEmpty(email))
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
            // Gọi BLL để thêm dữ liệu vào database
            SupplierBLL supplierBLL = new SupplierBLL();
            bool isAdded = supplierBLL.AddSupplier( tenNCC,sdt,email,diaChi,note);

            if (isAdded)
            {
                MessageBox.Show("Thêm nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields(); // Xóa dữ liệu sau khi thêm
            }
            else
            {
                MessageBox.Show("Thêm thất bại! Mã NCC có thể đã tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Hàm xóa dữ liệu sau khi thêm thành công
        private void ClearFields()
        {
            txt_tenNCC.Clear();
            txt_diachi.Clear();
            txt_sdt.Clear();
            txt_email.Clear();
            txt_note.Clear();
        }


    }
}
