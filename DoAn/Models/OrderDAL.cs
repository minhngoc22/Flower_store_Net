using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Doan.Models;

namespace DoAn.Models
{
    public class OrderDAL
    {
        public DataTable GetAllOrders()
        {
            using (Ketnoisql db = new Ketnoisql())
            {
               

                // Truy vấn tất cả đơn hàng mà không loại trừ NULL
                string query = @"
SELECT 
    o.order_code AS [Mã Đơn Hàng],
    c.customer_code AS [Mã Khách Hàng],
    e.full_name AS [Tên Nhân Viên Xử Lý],
    o.order_date AS [Ngày Đặt Hàng],
    o.total_amount AS [Tổng Tiền],
    o.status AS [Trạng Thái],
    o.payment AS [Thanh Toán],
    o.note AS [Ghi Chú],
    o.shipping_address AS [Địa Chỉ Giao Hàng]
FROM Orders o
JOIN Customers c ON o.customer_id = c.id
LEFT JOIN Employees e ON o.employee_id = e.id"; // Dùng LEFT JOIN để hiển thị cả các trường có NULL

                return db.RefreshData(query);
            }
        }




        public string InsertOrder(string customerPhone, string employeeName = null, string shippingAddress = null)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                // 1️⃣ Tìm ID khách hàng dựa trên số điện thoại
                string queryFindCustomer = "SELECT id, customer_code FROM Customers WHERE phone = @Phone";
                Dictionary<string, object> paramCustomer = new Dictionary<string, object>
        {
            { "@Phone", customerPhone }
        };

                DataTable customerTable = db.GetDataTable(queryFindCustomer, paramCustomer);
                if (customerTable.Rows.Count == 0)
                {
                    Console.WriteLine("❌ Không tìm thấy khách hàng!");
                    return string.Empty;
                }
                int customerId = Convert.ToInt32(customerTable.Rows[0]["id"]);
                string customerCode = customerTable.Rows[0]["customer_code"].ToString();

                // 2️⃣ Nếu tên nhân viên không phải null, tìm ID nhân viên
                int? employeeId = null;
                if (!string.IsNullOrEmpty(employeeName))
                {
                    string queryFindEmployee = "SELECT id FROM Employees WHERE full_name = @FullName";
                    Dictionary<string, object> paramEmployee = new Dictionary<string, object>
            {
                { "@FullName", employeeName }
            };

                    object employeeIdObj = db.ExecuteScalar(queryFindEmployee, paramEmployee);
                    if (employeeIdObj != null)
                    {
                        employeeId = Convert.ToInt32(employeeIdObj);
                    }
                    else
                    {
                        Console.WriteLine("❌ Không tìm thấy nhân viên!");
                        // Bạn có thể chọn không tạo đơn hàng nếu không tìm thấy nhân viên, hoặc tiếp tục tạo đơn hàng mà không có nhân viên.
                        employeeId = null;
                    }
                }

                // 3️⃣ Chèn đơn hàng với giá trị mặc định
                string queryInsertOrder = @"
        INSERT INTO Orders (customer_id, employee_id, total_amount, status, payment, note, shipping_address)
        VALUES (@CustomerId, @EmployeeId, 0, N'Đang xử lý', N'Chưa thanh toán', NULL, @ShippingAddress);
        SELECT SCOPE_IDENTITY();";

                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@CustomerId", customerId },
            { "@EmployeeId", (object)employeeId ?? DBNull.Value }, // Nếu không có nhân viên, đặt NULL
            { "@ShippingAddress", (object)shippingAddress ?? DBNull.Value } // Sử dụng DBNull.Value nếu địa chỉ giao hàng là null
        };

                object orderIdObj = db.ExecuteScalar(queryInsertOrder, parameters);
                if (orderIdObj == null) return string.Empty;
                int orderId = Convert.ToInt32(orderIdObj);

                // 4️⃣ Lấy `order_code` của đơn hàng vừa tạo
                string queryGetOrderCode = "SELECT order_code FROM Orders WHERE id = @OrderId";
                Dictionary<string, object> paramGetOrder = new Dictionary<string, object>
        {
            { "@OrderId", orderId }
        };

                object orderCodeObj = db.ExecuteScalar(queryGetOrderCode, paramGetOrder);
                string orderCode = orderCodeObj?.ToString() ?? string.Empty;

                // ✅ Trả về mã đơn hàng và mã khách hàng
                return orderCode;
            }
        }



        // 🟢 Cập nhật đơn hàng
        public bool UpdateOrder(string orderCode,decimal total_amount, string status, string employeeId, string payment, string note, string shippingAddress)
        {
            string query = @"
        UPDATE Orders 
        SET status = @Status, 
            employee_id = @EmployeeId, 
            total_amount = @TotalAmount,
            payment = @Payment, 
            note = @Note,
shipping_address = @ShippingAddress
        WHERE order_code = @OrderCode";

            Dictionary<string, object> parameters = new Dictionary<string, object>
    {
        { "@OrderCode", orderCode },
        { "@TotalAmount", total_amount },
        { "@Status", status },
        { "@EmployeeId", employeeId },
        { "@Payment", payment },
        { "@Note", note },
                { "@ShippingAddress", shippingAddress }
    };

            Ketnoisql db = new Ketnoisql();
            return db.ExecuteNonQuery(query, parameters) > 0;
        }


        // 🟢 Xóa đơn hàng
        public bool DeleteOrder(int orderId)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = "DELETE FROM Orders WHERE id = @OrderId";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@OrderId", orderId }
                };

                return db.ExecuteNonQuery(query, parameters) > 0;
            }
        }
        //lấy danh sách trạng thái

        public DataTable GetAllOrderStatuses()
        {
            using (Ketnoisql db = new Ketnoisql()) // Sử dụng lớp kết nối CSDL
            {
                string query = @"
        SELECT DISTINCT status 
        FROM Orders
        ORDER BY status ASC"; // Sắp xếp trạng thái theo thứ tự ABC

                return db.GetDataTable(query); // Gọi phương thức lấy dữ liệu từ Ketnoisql
            }
        }

        // Lấy danh sách nhân viên, không bao gồm những người có vị trí là "nhân viên giao hàng" hoặc "nhân viên kho"
        // và có end_date là NULL
        public DataTable GetEmployees()
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
            SELECT id, full_name 
            FROM Employees 
            WHERE position NOT IN ('Nhân viên giao hàng', 'Nhân viên kho') 
            AND end_date IS NULL
            ORDER BY full_name ASC";
                return db.GetDataTable(query);
            }
        }

        //lấy danh sách thanh toán
        public DataTable GetPaymentMethods()
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
SELECT DISTINCT payment
FROM Orders 
WHERE payment IS NOT NULL
ORDER BY payment ASC"; // Lấy danh sách phương thức thanh toán duy nhất

                return db.GetDataTable(query);
            }
        }

        public DataTable GetOrderStatusList()
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
        SELECT DISTINCT status
        FROM Orders
        WHERE status IS NOT NULL
        ORDER BY status ASC"; // Lấy danh sách trạng thái đơn hàng duy nhất

                return db.GetDataTable(query);
            }
        }


        public DataTable GetOrderByCode(string orderCode)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
        SELECT 
            o.order_code AS [Mã Đơn Hàng],
            e.full_name AS [Tên Nhân Viên Xử Lý],
            o.total_amount AS [Tổng Tiền],
            o.status AS [Trạng Thái],
            o.payment AS [Thanh Toán],
            o.note AS [Ghi Chú],
            o.shipping_address AS [Địa Chỉ]
        FROM Orders o
        LEFT JOIN Employees e ON o.employee_id = e.id
        WHERE o.order_code = @OrderCode"; // Dùng LEFT JOIN để đảm bảo đơn hàng vẫn được trả về mặc dù không có chi tiết.

                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@OrderCode", orderCode }
        };

                return db.GetDataTable(query, parameters);
            }
        }


        // 🟢 Tìm khách hàng theo số điện thoại hoặc email
        public DataTable GetCustomerByPhoneOrEmail(string input)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
                SELECT customer_code, full_name, phone, email
                FROM Customers
                WHERE phone = @Input OR email = @Input";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@Input", input }
                };

                return db.GetDataTable(query, parameters);
            }
        }

        public DataTable GetOrderByStatus(string status)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
        SELECT 
    o.order_code AS [Mã Đơn Hàng],
    c.customer_code AS [Mã Khách Hàng],
    e.full_name AS [Tên Nhân Viên Xử Lý],
    o.order_date AS [Ngày Đặt Hàng],
    o.total_amount AS [Tổng Tiền],
    o.status AS [Trạng Thái],
    o.payment AS [Thanh Toán],
    o.note AS [Ghi Chú],
    o.shipping_address AS [Địa Chỉ Giao Hàng]
FROM Orders o
JOIN Customers c ON o.customer_id = c.id
LEFT JOIN Employees e ON o.employee_id = e.id

        WHERE o.status = @Status";

                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@Status", status }
        };

                return db.GetDataTable(query, parameters);
            }
        }
        public void UpdateTotalAmount(string orderCode)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
        UPDATE Orders
        SET total_amount = (
            SELECT SUM(od.quantity * od.unit_price)
            FROM OrderDetails od
            WHERE od.order_id = (SELECT id FROM Orders WHERE order_code = @OrderCode)
        )
        WHERE order_code = @OrderCode";

                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@OrderCode", orderCode }
        };

                db.ExecuteNonQuery(query, parameters);
            }
        }

        public int GetOrderIdByCode(string orderCode)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = "SELECT id FROM Orders WHERE order_code = @OrderCode";
                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@OrderCode", orderCode }
        };

                object result = db.ExecuteScalar(query, parameters);
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        public DataTable GetOrderStatus(string orderCode)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = "SELECT status FROM Orders WHERE order_code = @OrderCode";
                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@OrderCode", orderCode }
        };

                // Lấy danh sách kết quả từ truy vấn
                List<Dictionary<string, object>> result = db.ExecuteQuery(query, parameters);

                // Chuyển đổi danh sách thành DataTable
                DataTable dataTable = new DataTable();
                if (result.Count > 0)
                {
                    // Tạo cột dựa trên khóa của dictionary
                    foreach (var key in result[0].Keys)
                    {
                        dataTable.Columns.Add(key);
                    }

                    // Thêm hàng dữ liệu vào DataTable
                    foreach (var row in result)
                    {
                        DataRow dataRow = dataTable.NewRow();
                        foreach (var key in row.Keys)
                        {
                            dataRow[key] = row[key];
                        }
                        dataTable.Rows.Add(dataRow);
                    }
                }

                return dataTable;
            }
        }







    }
}