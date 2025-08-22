using Doan.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn.Models
{
    class OrderDetailsDAL : Ketnoisql
    {
        // Lấy thông tin chi tiết đơn hàng theo order_code
        public DataTable GetAll(string orderCode)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                // Truy vấn lấy order_id từ bảng Orders dựa vào order_code
                string getOrderIDQuery = @"SELECT id FROM Orders WHERE order_code = @OrderCode";
                Dictionary<string, object> orderIdParam = new Dictionary<string, object>
                {
                    { "@OrderCode", orderCode }
                };

                DataTable orderIdTable = db.GetDataTable(getOrderIDQuery, orderIdParam);

                if (orderIdTable.Rows.Count == 0)
                {
                    // Trả về bảng rỗng nếu không tìm thấy order_id
                    return new DataTable();
                }

                // Lấy giá trị order_id
                int orderId = Convert.ToInt32(orderIdTable.Rows[0]["id"]);

                // Truy vấn chi tiết đơn hàng từ OrderDetails bằng order_id
                // Truy vấn chi tiết đơn hàng từ OrderDetails bằng order_id
                string query = @"
                SELECT 
                    o.order_code AS [Mã Đơn Hàng],
                    e.full_name AS [Tên Nhân Viên], 
                    p.product_name AS [Tên Sản Phẩm],
                    od.product_id AS [Mã Sản Phẩm],
                    od.quantity AS [Số Lượng],
                    od.unit_price AS [Đơn Giá],
                    p.discount AS [Giảm Giá],
                    od.total_price AS [Tổng Tiền],
                    
                    od.note AS [Ghi Chú]
                FROM OrdersDetails od
                JOIN Orders o ON od.order_id = o.id
                LEFT JOIN Employees e ON o.employee_id = e.id  --Dùng LEFT JOIN để hiển thị cả các trường có NULL
                JOIN Products p ON od.product_id = p.id  -- Lấy tên sản phẩm
                WHERE od.order_id = @OrderID";


                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@OrderID", orderId }
                };

                return db.GetDataTable(query, parameters);
            }
        }

        public decimal GetTotalPriceByOrderCode(string orderCode)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                // Truy vấn lấy order_id từ bảng Orders dựa vào order_code
                string getOrderIDQuery = @"SELECT id FROM Orders WHERE order_code = @OrderCode";
                Dictionary<string, object> orderIdParam = new Dictionary<string, object>
        {
            { "@OrderCode", orderCode }
        };

                DataTable orderIdTable = db.GetDataTable(getOrderIDQuery, orderIdParam);

                if (orderIdTable.Rows.Count == 0)
                {
                    return 0; // Không tìm thấy đơn hàng, tổng tiền = 0
                }

                int orderId = Convert.ToInt32(orderIdTable.Rows[0]["id"]);

                // Tính tổng tiền của tất cả sản phẩm trong đơn hàng
                string query = @"SELECT SUM(total_price) AS TotalPrice FROM OrdersDetails WHERE order_id = @OrderID";
                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@OrderID", orderId }
        };

                DataTable resultTable = db.GetDataTable(query, parameters);

                if (resultTable.Rows.Count > 0 && resultTable.Rows[0]["TotalPrice"] != DBNull.Value)
                {
                    return Convert.ToDecimal(resultTable.Rows[0]["TotalPrice"]);
                }

                return 0; // Nếu không có sản phẩm nào, tổng tiền là 0
            }
        }


        public bool InsertOrderDetail(string orderCode, string productId, int quantity, decimal unitPrice, string note)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                // 1. Lấy order_id và trạng thái đơn hàng
                string getOrderStatusQuery = "SELECT id, status FROM Orders WHERE order_code = @OrderCode";
                Dictionary<string, object> orderParams = new Dictionary<string, object>
        {
            { "@OrderCode", orderCode }
        };

                DataTable orderTable = db.GetDataTable(getOrderStatusQuery, orderParams);

                if (orderTable.Rows.Count == 0)
                {
                    return false; // Không tìm thấy đơn hàng
                }

                int orderId = Convert.ToInt32(orderTable.Rows[0]["id"]);
                string orderStatus = orderTable.Rows[0]["status"].ToString();

                // 2. Kiểm tra trạng thái đơn hàng
                if (orderStatus == "Đã hủy" || orderStatus == "Đang giao")
                {
                    return false; // Không thể thêm chi tiết đơn hàng
                }

                // 3. Lấy giảm giá sản phẩm (nếu có)
                decimal discount = GetProductDiscount(productId); // Ví dụ: 10 (%)

                // 4. Tính giá sau giảm
                decimal discountedPrice = unitPrice - (unitPrice * discount / 100);

                // 5. Tính tổng tiền sau giảm giá
                decimal totalPrice = quantity * discountedPrice;

                // 6. Thêm chi tiết vào OrdersDetails
                string insertQuery = @"
        INSERT INTO OrdersDetails (order_id, product_id, quantity, unit_price, total_price, note)
        VALUES (@OrderID, @ProductID, @Quantity, @UnitPrice, @TotalPrice, @Note)";

                Dictionary<string, object> insertParams = new Dictionary<string, object>
        {
            { "@OrderID", orderId },
            { "@ProductID", productId },
            { "@Quantity", quantity },
            { "@UnitPrice", unitPrice },
            { "@TotalPrice", totalPrice },
            { "@Note", note }
        };

                int rowsInserted = db.ExecuteNonQuery(insertQuery, insertParams);

                if (rowsInserted > 0)
                {
                    // 7. Trừ số lượng sản phẩm trong kho
                    string updateProductQuery = @"
            UPDATE Products
            SET stock_quantity = stock_quantity - @Quantity
            WHERE id = @ProductID";

                    Dictionary<string, object> updateProductParams = new Dictionary<string, object>
            {
                { "@Quantity", quantity },
                { "@ProductID", productId }
            };

                    db.ExecuteNonQuery(updateProductQuery, updateProductParams);

                    // 8. Cập nhật tổng tiền của đơn hàng trong bảng Orders
                    // 7. Cập nhật tổng tiền đơn hàng (an toàn với ISNULL để tránh NULL)
                    string updateOrderTotalQuery = @"
UPDATE Orders
SET total_amount = ISNULL((SELECT SUM(total_price) FROM OrdersDetails WHERE order_id = @OrderID), 0)
WHERE id = @OrderID";
                    Dictionary<string, object> updateOrderParams = new Dictionary<string, object>
            {
                { "@OrderID", orderId }
            };
                    db.ExecuteNonQuery(updateOrderTotalQuery, updateOrderParams);

                    return true;
                }
                return false; // Trả về false nếu không có gì thay đổi (không có chi tiết đơn hàng được chèn vào)
            }
        }



        public DataTable GetAllOrderStatuses()
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = "SELECT DISTINCT status AS [Trạng Thái] FROM Orders";
                return db.GetDataTable(query);
            }
        }
        public DataTable GetAllPaymentStatuses()
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = "SELECT DISTINCT payment AS [Thanh Toán] FROM Orders";
                return db.GetDataTable(query);
            }
        }
        //lấy giá sản phẩm
        public decimal GetProductPrice(string productCode)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = "SELECT price FROM Products WHERE id = @ProductCode";
                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@ProductCode", productCode }
        };

                DataTable dt = db.GetDataTable(query, parameters);
                if (dt.Rows.Count > 0)
                {
                    return Convert.ToDecimal(dt.Rows[0]["price"]);
                }
                return 0; // Trả về 0 nếu không tìm thấy sản phẩm
            }
        }

        //lấy giảm giá sản phẩn
        public decimal GetProductDiscount(string productCode)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = "SELECT discount FROM Products WHERE id = @ProductCode";
                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@ProductCode", productCode }
        };
                DataTable dt = db.GetDataTable(query, parameters);
                if (dt.Rows.Count > 0)
                {
                    return Convert.ToDecimal(dt.Rows[0]["discount"]);
                }
                return 0; // Trả về 0 nếu không tìm thấy sản phẩm
            }
        }
        public DataTable GetOrderDetailsByOrderCode(string orderCode)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
       SELECT TOP 1 
    o.order_code AS [Mã Đơn Hàng],
    e.full_name AS [Tên Nhân Viên], 
    p.product_name AS [Tên Sản Phẩm],
    od.product_id AS [Mã Sản Phẩm],
    od.quantity AS [Số Lượng],
    od.unit_price AS [Đơn Giá],
    od.note AS [Ghi Chú]
FROM OrdersDetails od
JOIN Orders o ON od.order_id = o.id
JOIN Employees e ON o.employee_id = e.id
JOIN Products p ON od.product_id = p.id
WHERE o.order_code = @OrderCode
ORDER BY od.id DESC"; // ID lớn nhất


                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@OrderCode", orderCode }
        };

                return db.GetDataTable(query, parameters);
            }
        }

        public string GetEmployeeNameByOrderCode(string orderCode)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
            SELECT e.full_name
            FROM Orders o
            JOIN Employees e ON o.employee_id = e.id
            WHERE o.order_code = @OrderCode
        ";

                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@OrderCode", orderCode }
        };

                DataTable dt = db.GetDataTable(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["full_name"].ToString();
                }

                return string.Empty; // Không tìm thấy
            }
        }


    }
}
