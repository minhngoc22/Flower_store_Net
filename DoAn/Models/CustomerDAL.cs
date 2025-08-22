using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Doan.Models;

namespace DoAn.Models
{
    public class CustomerDAL
    {
        // 🟢 Lấy danh sách khách hàng

        public DataTable GetAllCustomers()
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
        SELECT 
            c.customer_code AS [Mã Khách Hàng],
            c.full_name AS [Tên Khách Hàng],
            c.phone AS [Số Điện Thoại],
            c.email AS [Email],
            c.address AS [Địa Chỉ],
            c.note AS [Ghi Chú]
        FROM Customers c";
                return db.RefreshData(query);
            }

        }

        // 🟢 Thêm khách hàng
        public bool AddCustomer(string fullName, string phone, string email, string address, string note)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
        INSERT INTO Customers (full_name, phone, email, address, note) 
        VALUES (@FullName, @Phone, @Email, @Address, @Note)";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@FullName", fullName },
                    { "@Phone", phone },
                    { "@Email", string.IsNullOrEmpty(email) ? DBNull.Value : email },
                    { "@Address", string.IsNullOrEmpty(address) ? DBNull.Value : address },
                    { "@Note", string.IsNullOrEmpty(note) ? DBNull.Value : note }
                };

                return db.ExecuteNonQuery(query, parameters) > 0;
            }
        }

        // 🟢 Cập nhật khách hàng
        public bool UpdateCustomer(string customer_code, string fullName, string phone, string email, string address, string note)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
        UPDATE Customers 
        SET full_name = @FullName, 
            phone = @Phone, 
            email = @Email, 
            address = @Address, 
            note = @Note
        WHERE customer_code = @Customer_code";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@Customer_code", customer_code },
                    { "@FullName", fullName },
                    { "@Phone", phone },
                    { "@Email", string.IsNullOrEmpty(email) ? DBNull.Value : email },
                    { "@Address", string.IsNullOrEmpty(address) ? DBNull.Value : address },
                    { "@Note", string.IsNullOrEmpty(note) ? DBNull.Value : note }
                };

                return db.ExecuteNonQuery(query, parameters) > 0;
            }
        }

        // 🟢 Xóa khách hàng
        public bool DeleteCustomer(string customer_code)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = "DELETE FROM Customers WHERE customer_code = @Customer_code";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@Customer_code", customer_code }
                };

                return db.ExecuteNonQuery(query, parameters) > 0;
            }
        }

        // 🟢 Tìm kiếm khách hàng theo số điện thoại hoặc email
        // 🟢 Tìm kiếm khách hàng theo tên, số điện thoại hoặc email
        public DataTable SearchCustomers(string keyword)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
    SELECT 
        c.customer_code AS [Mã Khách Hàng],
        c.full_name AS [Tên Khách Hàng],
        c.phone AS [Số Điện Thoại],
        c.email AS [Email],
        c.address AS [Địa Chỉ],
        c.note AS [Ghi Chú]
    FROM Customers c
    WHERE c.full_name LIKE @Keyword 
        OR c.phone LIKE @Keyword 
        OR c.email LIKE @Keyword";

                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@Keyword", "%" + keyword + "%" }
        };

                return db.GetDataTable(query, parameters);
            }
        }


        // 🟢 Lấy danh sách khách hàng theo địa chỉ (ĐÃ SỬA LỖI)
        public DataTable GetCustomersByAddress(string address)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
        SELECT 
            c.customer_code AS [Mã Khách Hàng],
            c.full_name AS [Tên Khách Hàng],
            c.phone AS [Số Điện Thoại],
            c.email AS [Email],
            c.address AS [Địa Chỉ],
            c.note AS [Ghi Chú]
        FROM Customers c
        WHERE c.address = @Address";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@Address", address }
                };

                return db.GetDataTable(query, parameters);
            }
        }

        // 🟢 Lấy danh sách địa chỉ khách hàng (ĐÃ SỬA LỖI)
        public DataTable GetAllCustomerAddresses()
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"SELECT DISTINCT address FROM Customers ORDER BY address ASC";
                return db.GetDataTable(query);
            }
        }

        // 🟢 Lấy thông tin khách hàng theo mã
        public DataTable GetCustomerByCode(string customerCode)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
        SELECT 
            c.customer_code AS [Mã Khách Hàng],
            c.full_name AS [Tên Khách Hàng],
            c.phone AS [Số Điện Thoại],
            c.email AS [Email],
            c.address AS [Địa Chỉ],
            c.note AS [Ghi Chú]
        FROM Customers c
        WHERE c.customer_code = @CustomerCode";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@CustomerCode", customerCode }
                };

                return db.GetDataTable(query, parameters);
            }
        }

        public DataTable SearchCustomersByPhone(string phone)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
            SELECT 
                c.customer_code AS [Mã Khách Hàng],
                c.full_name AS [Tên Khách Hàng],
                c.phone AS [Số Điện Thoại],
                c.email AS [Email],
                c.address AS [Địa Chỉ],
                c.note AS [Ghi Chú]
            FROM Customers c
            WHERE c.phone LIKE @Phone";

                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@Phone", "%" + phone + "%" }
        };

                return db.GetDataTable(query, parameters);
            }
        }

    }
}