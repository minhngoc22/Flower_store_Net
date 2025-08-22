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
    public partial class UC_updateND : UserControl
    {
        private string user_code; // Lưu user_code nhận được
        public UC_updateND(string user_code)
        {
            InitializeComponent();
            this.user_code = user_code; // Gán giá trị user_code nhận từ ngoài

            LoadRoles(user_code);  // 🔹 Tải danh sách vai trò và hiển thị đúng vai trò hiện tại
            LoadUserData(user_code); // Load thông tin user
        }

        private void btn_luu_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy dữ liệu từ form
                string user_code = txt_maND.Text.Trim();  // Mã người dùng
                string fullName = txt_tenND.Text.Trim();      // Tên người dùng
                string username = txt_tenDN.Text.Trim();       // Tên đăng nhập
                string newPassword = txt_newPass.Text.Trim(); // Mật khẩu mới (nếu có)
                string role = cbo_phq.SelectedValue?.ToString(); // Phân quyền
                string note = txt_note.Text.Trim();        // Ghi chú

                // Kiểm tra dữ liệu đầu vào
                if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(username))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Gọi UserBLL để cập nhật người dùng
                UserBLL userBLL = new UserBLL();
                bool isUpdated = userBLL.UpdateUser(user_code, fullName, role, note, newPassword);

                if (isUpdated)
                {
                    MessageBox.Show("Cập nhật người dùng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.ParentForm?.Close(); // Đóng form sau khi cập nhật
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


        public void LoadUserData(string user_code)
        {
            UserBLL userBLL = new UserBLL();
            DataRow user = userBLL.GetUserById(user_code);

            if (user != null)
            {
                txt_maND.Text = user["Mã Tài Khoản"].ToString();
                txt_tenND.Text = user["Họ Và Tên"].ToString();
                txt_tenDN.Text = user["Tên Đăng Nhập"].ToString();
                txt_note.Text = user["Ghi Chú"].ToString();

                // Đặt giá trị mặc định cho cbo_phq dựa trên vai trò của user
                string userRole = user["Vai Trò"].ToString();
                if (cbo_phq.Items.Contains(userRole)) // Kiểm tra nếu role hợp lệ
                {
                    cbo_phq.SelectedValue = userRole;
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy người dùng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadRoles(string user_code)
        {
            try
            {
                UserBLL userBLL = new UserBLL();
                string currentRole = userBLL.GetUserRoleById(user_code); // Lấy vai trò hiện tại của user

                DataTable roles = new DataTable();
                roles.Columns.Add("role", typeof(string));

                // Thêm danh sách vai trò theo quyền hiện tại
                if (currentRole == "Admin")
                {
                    roles.Rows.Add("Admin");
                    roles.Rows.Add("Nhân viên");
                    roles.Rows.Add("Khách Hàng");
                }
                else if (currentRole == "Nhân viên")
                {
                    roles.Rows.Add("Nhân viên");
                    roles.Rows.Add("Khách Hàng");
                }
                else if (currentRole == "Khách Hàng")
                {
                    roles.Rows.Add("Khách Hàng"); // Khách hàng không thể thay đổi vai trò
                    cbo_phq.Enabled = false; // Vô hiệu hóa ComboBox
                }

                cbo_phq.DataSource = roles;
                cbo_phq.DisplayMember = "role";
                cbo_phq.ValueMember = "role";
                cbo_phq.DropDownStyle = ComboBoxStyle.DropDownList;

                // Chọn vai trò mặc định theo dữ liệu trong CSDL
                cbo_phq.SelectedValue = currentRole;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách vai trò: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
