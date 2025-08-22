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
using DoAn.View.SP;
using DoAn.View.User;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace DoAn.View
{

    public partial class UC_User : UserControl
    {
        private UserBLL usersBLL;
        private string? selectedUserId; // ID người dùng đang chọn
        public UC_User()
        {
            InitializeComponent();
            usersBLL = new UserBLL();
        }

        private void UC_User_Load(object sender, EventArgs e)
        {
            LoadUsers(); // Tải danh sách người dùng
            LoadRoles(); // Tải danh sách vai trò
        }

        // 🟢 Load danh sách người dùng lên DataGridView
        private void LoadUsers()
        {
            DataTable user = usersBLL.GetAllUsers();
            dvg_user.DataSource = user;
        }
        // 🟢 Load danh sách vai trò lên ComboBox
        private void LoadRoles()
        {
            DataTable dt = usersBLL.GetAllRoles(); // Lấy danh sách vai trò

            if (dt.Rows.Count > 0)
            {
                // Thêm một dòng "Tất cả" vào danh sách
                DataRow allRow = dt.NewRow();
                allRow["role"] = "Tất cả";
                dt.Rows.InsertAt(allRow, 0);

                // Gán dữ liệu vào ComboBox
                cbo_vitri.DataSource = dt;
                cbo_vitri.DisplayMember = "role";  // Hiển thị tên vai trò
                cbo_vitri.ValueMember = "role";    // Giá trị là tên vai trò
                cbo_vitri.SelectedIndex = 0;       // Chọn dòng đầu tiên mặc định
            }
            LoadUsers();
        }


        private void cbo_vitri_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedRole = cbo_vitri.SelectedValue?.ToString() ?? string.Empty;

            if (!string.IsNullOrEmpty(selectedRole))
            {
                if (selectedRole == "Tất cả")
                {
                    LoadUsers();
                }
                else
                {
                    LoadUsersByRole(selectedRole);
                }
            }
        }

        private void LoadUsersByRole(string role)
        {
            DataTable dt = usersBLL.GetUsersByRole(role);
            dvg_user.DataSource = dt;
        }

        private void dvg_user_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dvg_user.Rows[e.RowIndex];
                selectedUserId = row.Cells["Mã Người Dùng"].Value?.ToString();
                Console.WriteLine($"Selected User ID: {selectedUserId}");
            }
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            txt_tim.Clear(); // Xóa nội dung ô tìm kiếm
            LoadRoles(); // ✅ Nạp lại danh sách địa chỉ từ đầu
            cbo_vitri.SelectedIndex = 0; // Chọn lại "Tất cả"
            LoadUsers(); // ✅ Hiển thị lại toàn bộ danh sách nhân viên
            dvg_user.ClearSelection(); // Bỏ chọn hàng trong DataGridView
        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {

            if (selectedUserId == null)
            {
                MessageBox.Show("Vui lòng chọn người dùng để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa người dùng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                bool isDeleted = usersBLL.DeleteUser(Convert.ToInt32(selectedUserId));

                if (isDeleted)
                {
                    MessageBox.Show("Xóa người dùng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUsers(); // Cập nhật danh sách
                }
                else
                {
                    MessageBox.Show("Lỗi khi xóa người dùng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        }

        private void btn_tim_Click(object sender, EventArgs e)
        {
            string searchText = txt_tim.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                LoadUsers(); // Hiển thị toàn bộ nếu ô tìm kiếm trống
            }
            else
            {
                SearchUserByName(searchText);
            }

        }

        private void SearchUserByName(string name)
        {
            DataTable dt = usersBLL.SearchUsers(name);
            dvg_user.DataSource = dt;
        }




        private void btn_update_Click(object sender, EventArgs e)
        {
            if (dvg_user.SelectedRows.Count > 0) // Kiểm tra có chọn user nào không
            {
                string user_code = dvg_user.SelectedRows[0].Cells[1].Value?.ToString(); // Lấy cột thứ 2 (index = 1)

                if (!string.IsNullOrEmpty(user_code))
                {
                    Form form = new Form();
                    UC_updateND uc = new UC_updateND(user_code); // Truyền user_code vào

                    form.Controls.Add(uc);
                    uc.Dock = DockStyle.Fill; // Hiển thị UC full Form

                    form.StartPosition = FormStartPosition.CenterScreen; // Hiển thị giữa màn hình
                    form.Size = new Size(630, 500); // Kích thước cửa sổ
                    form.ShowDialog(); // Hiển thị Form chứa UC
                }
                else
                {
                    MessageBox.Show("Không thể lấy mã người dùng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một người dùng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_them_Click(object sender, EventArgs e)
        {
            Form form = new Form();

            UC_themND uc = new UC_themND();

            form.Controls.Add(uc);
            uc.Dock = DockStyle.Fill;

            form.StartPosition = FormStartPosition.CenterScreen;
            form.Size = new Size(670, 470);
            form.ShowDialog();
        }
    }
}
