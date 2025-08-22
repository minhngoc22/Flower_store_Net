
using DoAn.Controller;
using DoAn.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;


namespace DoAn
{
    public partial class Form1 : Form
    {
        private KiemtraDN kt;

        public Form1()
        {
            InitializeComponent();
            kt = new KiemtraDN();

        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_login_Click(object sender, EventArgs e)
        {


            string username = txt_ten.Text.Trim();
            string password = txt_pass.Text.Trim();
            bool isValid = true;

            // Kiểm tra input
            if (string.IsNullOrEmpty(username))
            {
                errorProvider1.SetError(txt_ten, "Vui lòng nhập tên đăng nhập!");
                isValid = false;
            }
            if (string.IsNullOrEmpty(password))
            {
                errorProvider2.SetError(txt_pass, "Vui lòng nhập mật khẩu!");
                isValid = false;
            }

            if (!isValid) return; // Nếu có lỗi, không tiếp tục

            // Gọi CheckLogin để lấy vai trò của user
            string role = kt.CheckLogin(username, password);

            if (!string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Hide();
                if (role == "Admin")
                {
                    new Form2().Show(); // Mở form user thông thường
                }
                else
                {
                    new Form3().Show();

                }
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu sai!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        // Khai báo biến trạng thái hiển thị mật khẩu
        private bool isPasswordVisible = false;

       
        private void pic_ht_Click(object sender, EventArgs e)
        {
            // Đảo trạng thái
            isPasswordVisible = !isPasswordVisible;

            // Cập nhật hiển thị mật khẩu
            txt_pass.UseSystemPasswordChar = !isPasswordVisible;

            // Đổi icon con mắt
            pic_ht.Image = isPasswordVisible
     ? Image.FromFile(@"E:\HK6\LapTrinh\Doan\DoAn\images\icon1.png") // Đang hiện mật khẩu
     : Image.FromFile(@"E:\HK6\LapTrinh\Doan\DoAn\images\icon2.png"); // Đang ẩn mật khẩu


        }
    }
}
