using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAn.View
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void btn_logout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn chắc chắn muốn đăng xuất?",
                                                "Xác nhận",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Form1 form1 = new Form1();
                this.Close();
                form1.Show();

            }
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn chắc chắn muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();// Đóng form nếu chọn Yes
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            pn_Home.Controls.Clear();
            UC_Home2 uC_Home = new UC_Home2();
            pn_Home.Controls.Add(uC_Home);
        }

        private void btn_home_Click(object sender, EventArgs e)
        {
            pn_Home.Controls.Clear();
            UC_Home2 uC_Home = new UC_Home2();
            uC_Home.Dock = DockStyle.Fill;

            pn_Home.Controls.Add(uC_Home);
        }

        private void btn_donhang_Click(object sender, EventArgs e)
        {
            pn_Home.Controls.Clear(); // Xóa tất cả các UserControl cũ trước khi thêm mới

            UC_DonHang uC_DonHang = new UC_DonHang();// Tạo UserControl trang chủ
            uC_DonHang.Dock = DockStyle.Fill; // Căn chỉnh để vừa với Panel

            pn_Home.Controls.Add(uC_DonHang); // Thêm UserControl vào Panel
        }

        private void btn_sanpham_Click(object sender, EventArgs e)
        {
            pn_Home.Controls.Clear();
            UC_SanPham uC_SanPham = new UC_SanPham();
            uC_SanPham.Dock = DockStyle.Fill;
            pn_Home.Controls.Add(uC_SanPham);
        }

        private void btn_khachhang_Click(object sender, EventArgs e)
        {
            pn_Home.Controls.Clear();
            UC_KhachHang uC_KhachHang = new UC_KhachHang();
            uC_KhachHang.Dock = DockStyle.Fill;
            pn_Home.Controls.Add(uC_KhachHang);
        }

        private void btn_nhacc_Click(object sender, EventArgs e)
        {
            pn_Home.Controls.Clear();
            UC_NhaCungCap uC_NhaCungCap = new UC_NhaCungCap();
            uC_NhaCungCap.Dock = DockStyle.Fill;
            pn_Home.Controls.Add(uC_NhaCungCap);
        }
    }
}
