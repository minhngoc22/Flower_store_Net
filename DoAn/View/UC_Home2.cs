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
    public partial class UC_Home2 : UserControl
    {
        private StatisticsBLL statisticsBLL;
        public UC_Home2()
        {
            InitializeComponent();
            statisticsBLL = new StatisticsBLL();
        }

        private void UC_Home2_Load(object sender, EventArgs e)
        {
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;

            // Gọi BLL để lấy dữ liệu
            int totalOrders = statisticsBLL.GetTotalOrders(month, year);
            decimal totalRevenue = statisticsBLL.GetTotalRevenue(month, year);
            int totalProductsSold = statisticsBLL.GetTotalProductsSold(month, year);

            // Hiển thị lên giao diện
            label3.Text = $"{totalOrders} đơn hàng";  // Số đơn hàng
            
            label6.Text = $"{totalProductsSold} sản phẩm"; // Số sản phẩm đã bán

        }
    }
}
