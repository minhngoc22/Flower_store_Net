using System;
using System.Collections.Generic;
using System.Data;
using Doan.Models;
using DoAn.Models;

namespace DoAn.DAL
{
    public class StatisticsDAL
    {
        // 🟢 Lấy tổng số đơn hàng trong tháng
        public int GetTotalOrdersInMonth(int month, int year)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
                SELECT COUNT(*) 
                FROM Orders 
                WHERE MONTH(order_date) = @Month AND YEAR(order_date) = @Year";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@Month", month },
                    { "@Year", year }
                };

                return Convert.ToInt32(db.ExecuteScalar(query, parameters));
            }
        }

        // 🟢 Lấy tổng doanh thu trong tháng
        public decimal GetTotalRevenueInMonth(int month, int year)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
                SELECT SUM(total_amount) 
                FROM Orders 
                WHERE MONTH(order_date) = @Month AND YEAR(order_date) = @Year";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@Month", month },
                    { "@Year", year }
                };

                object result = db.ExecuteScalar(query, parameters);
                return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
        }

        // 🟢 Lấy tổng số sản phẩm bán trong tháng
        public int GetTotalProductsSoldInMonth(int month, int year)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
                SELECT SUM(quantity) 
                FROM OrdersDetails od
                JOIN Orders o ON od.order_id = o.id
                WHERE MONTH(o.order_date) = @Month AND YEAR(o.order_date) = @Year";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@Month", month },
                    { "@Year", year }
                };

                object result = db.ExecuteScalar(query, parameters);
                return result != DBNull.Value ? Convert.ToInt32(result) : 0;
            }
        }
    }
}
