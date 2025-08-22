using System;
using DoAn.DAL;

namespace DoAn.BLL
{
    public class StatisticsBLL
    {
        private readonly StatisticsDAL statisticsDAL;

        public StatisticsBLL()
        {
            statisticsDAL = new StatisticsDAL();
        }

        // 🟢 Lấy tổng số đơn hàng trong tháng
        public int GetTotalOrders(int month, int year)
        {
            return statisticsDAL.GetTotalOrdersInMonth(month, year);
        }

        // 🟢 Lấy tổng doanh thu trong tháng
        public decimal GetTotalRevenue(int month, int year)
        {
            return statisticsDAL.GetTotalRevenueInMonth(month, year);
        }

        // 🟢 Lấy tổng số sản phẩm bán trong tháng
        public int GetTotalProductsSold(int month, int year)
        {
            return statisticsDAL.GetTotalProductsSoldInMonth(month, year);
        }
    }
}
