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

namespace DoAn.View
{
    public partial class UC_Home : UserControl
    {
        private StatisticsBLL statisticsBLL;
        public UC_Home()
        {
            InitializeComponent();
            statisticsBLL = new StatisticsBLL();
            LoadStatistics();
        }

        private void UC_Home_Load(object sender, EventArgs e)
        {
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;

            // Gọi BLL để lấy dữ liệu
            int totalOrders = statisticsBLL.GetTotalOrders(month, year);
            decimal totalRevenue = statisticsBLL.GetTotalRevenue(month, year);
            int totalProductsSold = statisticsBLL.GetTotalProductsSold(month, year);

            // Hiển thị lên giao diện
            label3.Text = $"{totalOrders} đơn hàng";  // Số đơn hàng
            label4.Text = totalRevenue.ToString("C0", new System.Globalization.CultureInfo("vi-VN")); // Doanh thu VNĐ
            label6.Text = $"{totalProductsSold} sản phẩm"; // Số sản phẩm đã bán

        }
    }
}
