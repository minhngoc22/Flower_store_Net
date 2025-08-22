using System;
using System.Data;
using System.Collections.Generic;
using Doan.Models;
using Microsoft.Data.SqlClient;

namespace DoAn.Models
{
    public class ReportDAL
    {


        // Phương thức lấy báo cáo tổng hợp
        // Phương thức lấy tất cả báo cáo tổng hợp (không lọc theo tháng và năm)
        public DataTable GetAllMonthlyReports()
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                try
                {
                    string query = @"
                SELECT 
                    report_code AS [Mã Báo Cáo],
                    report_month AS [Tháng],
                    report_year AS [Năm],
                    total_orders AS [Tổng Đơn Hàng],
                    total_products_sold AS [Sản Phẩm Đã Bán],
                    total_revenue AS [Tổng Doanh Thu],
                    total_cost AS [Tổng Chi Phí],
                    total_profit AS [Lợi Nhuận],
                    note AS [Ghi Chú],
                    created_at AS [Ngày Tạo]
                FROM Reports
                 ORDER BY report_year ASC, report_month ASC"; // Sắp xếp theo năm, tháng từ cũ đến mới

                    return db.GetDataTable(query);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi truy vấn GetAllMonthlyReports: {ex.Message}");
                    return null;
                }
            }
        }




        // Phương thức lấy báo cáo doanh thu theo sản phẩm
        public DataTable GetProductRevenueReport(int month, int year)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
                    EXEC GenerateProductRevenueReport @month, @year";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@month", month },
                    { "@year", year }
                };

                return db.GetDataTable(query, parameters);
            }
        }

        // Phương thức lấy báo cáo doanh thu theo nhân viên
        public DataTable GetEmployeeSalesReport(int month, int year)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
        SELECT 
            e.employee_code AS [Mã Nhân Viên],
            e.full_name AS [Tên Nhân Viên],
            e.position AS [Chức Vụ],
            COUNT(o.id) AS [Số Lượng Đơn Hàng],
            SUM(od.total_price) AS [Tổng Doanh Thu]
        FROM Orders o
        JOIN Employees e ON o.employee_id = e.id
        JOIN OrdersDetails od ON o.id = od.order_id
        WHERE MONTH(o.order_date) = @month AND YEAR(o.order_date) = @year
        GROUP BY e.employee_code, e.full_name, e.position
        ORDER BY [Tổng Doanh Thu] DESC";

                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@month", month },
            { "@year", year }
        };

                return db.GetDataTable(query, parameters);
            }
        }

        

        public DataTable GetCustomerRevenueReport(int month, int year)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
            SELECT 
                c.full_name AS [Tên Khách Hàng],
                COUNT(o.id) AS [Số Lượng Đơn Hàng],
                SUM(od.total_price) AS [Tổng Doanh Thu]
            FROM Orders o
            JOIN Customers c ON o.customer_id = c.id
            JOIN OrdersDetails od ON o.id = od.order_id
            WHERE MONTH(o.order_date) = @month AND YEAR(o.order_date) = @year
            GROUP BY c.full_name
            ORDER BY [Tổng Doanh Thu] DESC";

                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@month", month },
            { "@year", year }
        };

                return db.GetDataTable(query, parameters);
            }
        }
        // Phương thức lấy báo cáo kho cho tất cả sản phẩm theo tháng và năm
        public DataTable GetAllProductReport(int month, int year)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
        SELECT 
            p.id AS [Mã Sản Phẩm],
            p.product_name AS [Tên Sản Phẩm], 
            SUM(od.quantity) AS [Số Lượng Bán], 
            p.cost_price AS [Giá Nhập],
            p.price AS [Giá Bán],   
            SUM(od.total_price) AS [Tổng Doanh Thu],
            -- Tính Lợi Nhuận = (Tổng Doanh Thu - Tổng Chi Phí)
            SUM(od.quantity) * (p.price - p.cost_price) AS [Lợi Nhuận]
        FROM OrdersDetails od
        JOIN Products p ON od.product_id = p.id
        JOIN Orders o ON od.order_id = o.id
        WHERE MONTH(o.order_date) = @month AND YEAR(o.order_date) = @year
        GROUP BY p.id, p.product_name, p.cost_price, p.price
        ORDER BY p.id"; //sắp xếp theo Mã Sản Phẩm
        
        Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@month", month },
            { "@year", year }
        };

                return db.GetDataTable(query, parameters);
            }
        }




        // Phương thức lấy báo cáo sản phẩm: mã, tên, số lượng đã bán, số lượng còn trong kho, ngày nhập
        public DataTable GetProductSalesStockReport(int month, int year)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
        SELECT 
            p.id AS [Mã Sản Phẩm], 
            p.product_name AS [Tên Sản Phẩm], 
            COALESCE(SUM(od.quantity), 0) AS [Số Lượng Bán],  -- Nếu không có đơn hàng, trả về 0
            p.stock_quantity AS [Số Lượng Tồn Kho],
            p.import_date AS [Ngày Nhập]
        FROM Products p
        LEFT JOIN OrdersDetails od ON p.id = od.product_id
        LEFT JOIN Orders o ON od.order_id = o.id
        WHERE (MONTH(o.order_date) = @month AND YEAR(o.order_date) = @year) OR o.order_date IS NULL
        GROUP BY p.id, p.product_name, p.stock_quantity, p.import_date
        ORDER BY [Số Lượng Bán] DESC";

                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@month", month },
            { "@year", year }
        };

                return db.GetDataTable(query, parameters);
            }
        }


        // Phương thức lấy báo cáo tổng hợp
        public DataTable CreateMonthlyReport(int month, int year)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
            EXEC GenerateMonthlyReport @month, @year";

                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@month", month },
            { "@year", year }
        };

                DataTable result = db.GetDataTable(query, parameters);

                // Kiểm tra dữ liệu trả về
                if (result.Rows.Count == 0)
                {
                    Console.WriteLine($"Không có dữ liệu cho tháng {month}, năm {year}");
                }

                return result;
            }
        }

        //xoa
        public bool DeleteReport(string reportID)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = "DELETE FROM Reports WHERE report_code = @ReportID";
                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@ReportID", reportID }
        };

                int rowsAffected = db.ExecuteNonQuery(query, parameters); // Thực thi câu lệnh DELETE

                return rowsAffected > 0; // Nếu xóa thành công, trả về true
            }
        }


        // Phương thức lấy báo cáo đơn hàng theo tháng/năm
        // Phương thức lấy báo cáo đơn hàng theo tháng/năm
        // Phương thức lấy báo cáo đơn hàng theo tháng/năm
        public DataTable GetOrdersReport(int month, int year)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
        SELECT 
            o.order_code AS [Mã Đơn Hàng],
            o.order_date AS [Ngày Đặt],
            c.full_name AS [Khách Hàng],
            e.full_name AS [Nhân Viên Bán Hàng],
            SUM(od.quantity) AS [Tổng Số Lượng],
            SUM(od.total_price) AS [Tổng Tiền]
        FROM Orders o
        JOIN OrdersDetails od ON o.id = od.order_id
        JOIN Customers c ON c.id = o.customer_id
        JOIN Employees e ON e.id = o.employee_id
        WHERE MONTH(o.order_date) = @month AND YEAR(o.order_date) = @year
        GROUP BY o.order_code, o.order_date, c.full_name, e.full_name
        ORDER BY o.order_date ASC";  // Sắp xếp theo ngày đặt

                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@month", month },
            { "@year", year }
        };

                return db.GetDataTable(query, parameters);
            }
        }



    }
}
