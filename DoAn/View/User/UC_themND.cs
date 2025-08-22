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

namespace DoAn.View.User
{
    public partial class UC_themND : UserControl
    {
        public UC_themND(string fullName, string email)
        {
            InitializeComponent();
            LoadRole(); // Gọi khi form load

            txt_tenND.Text = fullName;
            txt_email.Text = email;
        }
        public UC_themND()
        {
            InitializeComponent();
            LoadRole(); // Gọi khi form load

            
        }

        private void LoadRole()
        {
            UserBLL userBLL = new UserBLL();
            DataTable dtPositions = userBLL.GetAllRoles();

            if (dtPositions.Rows.Count > 0)
            {
                cbo_phq.DataSource = dtPositions;
                cbo_phq.DisplayMember = "role";  // Hiển thị role (chức vụ)
                cbo_phq.ValueMember = "role";    // Giá trị lưu
            }
        }

        private void btn_them_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy dữ liệu từ form
                string username = txt_tenDN.Text.Trim();  // txt_tdn: Tên đăng nhập
                string password = txt_mk.Text.Trim();   // txt_mk: Mật khẩu
                string fullName = txt_tenND.Text.Trim();   // txt_ht: Họ và tên
                string email = txt_email.Text.Trim();   // txt_email: Email
                string role = cbo_phq.SelectedValue?.ToString(); // cbo_phq: Quyền hạn
                string note = txt_note.Text.Trim();   // txt_ghichu: Ghi chú

                // Kiểm tra dữ liệu đầu vào
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Gọi UserBLL để thêm người dùng
                UserBLL userBLL = new UserBLL();
                bool isAdded = userBLL.AddUser(username, password, fullName,email, role, note);

                if (isAdded)
                {
                    MessageBox.Show("Thêm người dùng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields(); // Xóa dữ liệu nhập sau khi thêm thành công
                }
                else
                {
                    MessageBox.Show("Thêm người dùng thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ClearFields()
        {
            txt_tenDN.Clear();
            txt_mk.Clear();
            txt_tenND.Clear();
            txt_email.Clear();
            txt_note.Clear();
            cbo_phq.SelectedIndex = -1; // Bỏ chọn quyền hạn
        }
    }
}
