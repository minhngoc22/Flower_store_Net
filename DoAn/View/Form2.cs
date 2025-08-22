using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAn.View
{
    public partial class Form2 : Form
    {

        public Form2()
        {
            InitializeComponent();


        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn chắc chắn muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();// Đóng form nếu chọn Yes
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            pn_Home.Controls.Clear();
            UC_Home uC_Home = new UC_Home();
            pn_Home.Controls.Add(uC_Home);
        }

        private void btn_home_Click(object sender, EventArgs e)
        {
            pn_Home.Controls.Clear();
            UC_Home uC_Home = new UC_Home();
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

        private void pn_Home_Paint(object sender, PaintEventArgs e)
        {
            int radius = 20; // Độ cong của góc
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias; // Làm mịn

            Rectangle rect = pn_Home.ClientRectangle;
            rect.Width -= 1;
            rect.Height -= 1;

            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90); // Góc trên trái
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90); // Góc trên phải
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90); // Góc dưới phải
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90); // Góc dưới trái
            path.CloseFigure();

            pn_Home.Region = new Region(path); // Cắt panel theo vùng bo góc

            using (Pen pen = new Pen(Color.Black, 2)) // Viền đen dày 2px
            {
                g.DrawPath(pen, path);
            }
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



        private void btn_nho_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btn_nhanvien_Click_1(object sender, EventArgs e)
        {
            pn_Home.Controls.Clear();
            UC_NhanVien uC_NhanVien = new UC_NhanVien();
            uC_NhanVien.Dock = DockStyle.Fill;
            pn_Home.Controls.Add(uC_NhanVien);
        }

        private void btn_user_Click(object sender, EventArgs e)
        {
            pn_Home.Controls.Clear();
            UC_User uC_User = new UC_User();
            uC_User.Dock = DockStyle.Fill;
            pn_Home.Controls.Add(uC_User);

        }

        private void btn_baocao_Click(object sender, EventArgs e)
        {
            pn_Home.Controls.Clear();
            UC_Report uC_Report = new UC_Report();
            uC_Report.Dock = DockStyle.Fill;
            pn_Home.Controls.Add(uC_Report);
        }

        private void btn_danhmuc_Click_1(object sender, EventArgs e)
        {
            pn_Home.Controls.Clear();
            UC_DanhMuc uC_DanhMuc = new UC_DanhMuc();
            uC_DanhMuc.Dock = DockStyle.Fill;
            pn_Home.Controls.Add(uC_DanhMuc);
        }
    }
}
