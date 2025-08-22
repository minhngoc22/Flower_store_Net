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
using DoAn.View.User;
using DoAn.BLL;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DoAn.View.NV
{
    public partial class UC_themNV : UserControl
    {
        public UC_themNV()
        {
            InitializeComponent();

            LoadPositions();
        }

        private void LoadPositions()
        {
            NhanVienDAL dal = new NhanVienDAL();
            DataTable positions = dal.GetSortedPositions();

            cbo_vitri.DataSource = positions;
            cbo_vitri.DisplayMember = "position"; // Hiển thị tên vị trí
            cbo_vitri.ValueMember = "position";   // Lấy giá trị vị trí
        }

        private void btn_them_Click(object sender, EventArgs e)
        {
            string fullName = txt_tenNV.Text.Trim();
            string phone = txt_sdt.Text.Trim();
            string email = txt_email.Text.Trim();
            string address = txt_diachi.Text.Trim();
            decimal salary;
            string position = cbo_vitri.SelectedValue.ToString(); // Sử dụng giá trị đã chọn từ cbo_vitri
            string note = txt_note.Text.Trim(); // Ghi chú
            DateTime startDate;

            // Ngày bắt đầu làm việc, nếu không có thì sử dụng ngày hiện tại
            if (!DateTime.TryParse(txt_startDate.Text.Trim(), out startDate))
            {
                startDate = DateTime.Now; // Nếu không có giá trị, lấy ngày hiện tại
            }

            // 🟢 Kiểm tra số lương có hợp lệ không
            if (string.IsNullOrWhiteSpace(txt_luong.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập lương!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txt_luong.Text.Trim(), System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.CultureInfo.InvariantCulture, out salary) || salary < 0)
            {
                MessageBox.Show("Lương không hợp lệ! Hãy nhập số dương hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 🟢 Kiểm tra dữ liệu nhập có rỗng không
            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(position))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 🟢 Lấy giá trị từ các CheckBox cho salaryType
            string salaryType = "";

            // Kiểm tra các checkbox
            if (chk_giờ.Checked)
            {
                salaryType = "hourly";
            }
            else if (chk_thang.Checked)
            {
                salaryType = "monthly";
            }
            else
            {
                MessageBox.Show("Vui lòng chọn kiểu lương (Ngày hoặc Tháng).", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string username = txt_tenDN.Text.Trim();  // txt_tdn: Tên đăng nhập
            string password = txt_pass.Text.Trim();   // txt_mk: Mật khẩu
            string role = "Nhân viên"; // cbo_phq: Quyền hạn

            // 🟢 Gọi hàm thêm nhân viên
            NhanVienDAL nvDAL = new NhanVienDAL();
            bool success = nvDAL.AddEmployee(fullName, phone, email, address, salary, position, note, startDate, salaryType);

            if (success)
            {
                MessageBox.Show("Thêm nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Chỉ thêm người dùng nếu username và password có dữ liệu
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    UserBLL userBLL = new UserBLL();
                    bool isAdded = userBLL.AddUser(username, password, fullName, email, role, note);

                    if (isAdded)
                    {
                        MessageBox.Show("Tạo tài khoản người dùng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Tạo tài khoản người dùng thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                ClearFields();
            }
            else
            {
                MessageBox.Show("Thêm nhân viên thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void ClearFields()
        {
            txt_tenNV.Clear();
            txt_sdt.Clear();
            txt_email.Clear();
            txt_diachi.Clear();
            txt_luong.Clear();
            //   txt_vitri.Clear();
            txt_note.Clear();
            txt_startDate.Clear();
            txt_pass.Clear();
            txt_tenDN.Clear();
            
        }

        private void txt_starDate_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbo_vitri_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
