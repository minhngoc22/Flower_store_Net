using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Doan.Models;

namespace DoAn.Models
{
    public class SupplierDAL
    {
        // 🟢 Lấy danh sách nhà cung cấp
        public DataTable GetAllSuppliers()
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
        SELECT 
            
            s.supplier_code AS [Mã Nhà Cung Cấp],
            s.supplier_name AS [Tên Nhà Cung Cấp],
            s.phone AS [Số Điện Thoại],
            s.email AS [Email],
            s.address AS [Địa Chỉ],
            s.note AS [Ghi Chú]
        FROM Suppliers s";

                return db.GetDataTable(query);
            }
        }

        // 🟢 Thêm nhà cung cấp
        public bool AddSupplier(string supplierName, string phone, string email, string address, string note)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
        INSERT INTO Suppliers (supplier_name, phone, email, address, note) 
        VALUES (@SupplierName, @Phone, @Email, @Address, @Note)";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@SupplierName", supplierName },
                    { "@Phone", phone },
                    { "@Email", string.IsNullOrEmpty(email) ? DBNull.Value : email },
                    { "@Address", string.IsNullOrEmpty(address) ? DBNull.Value : address },
                    { "@Note", string.IsNullOrEmpty(note) ? DBNull.Value : note }
                };

                return db.ExecuteNonQuery(query, parameters) > 0;
            }
        }

        // 🟢 Cập nhật nhà cung cấp
        public bool UpdateSupplier(string supplier_code, string supplierName, string phone, string email, string address, string note)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
        UPDATE Suppliers 
        SET supplier_name = @SupplierName, 
            phone = @Phone, 
            email = @Email, 
            address = @Address, 
            note = @Note
        WHERE supplier_code = @Supplier_code";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@Supplier_code ", supplier_code },
                    { "@SupplierName", supplierName },
                    { "@Phone", phone },
                    { "@Email", string.IsNullOrEmpty(email) ? DBNull.Value : email },
                    { "@Address", string.IsNullOrEmpty(address) ? DBNull.Value : address },
                    { "@Note", string.IsNullOrEmpty(note) ? DBNull.Value : note }
                };

                return db.ExecuteNonQuery(query, parameters) > 0;
            }
        }

        // 🟢 Xóa nhà cung cấp
        public bool DeleteSupplier(string supplier_code )
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = "DELETE FROM Suppliers WHERE supplier_code  = @Supplier_code";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@Supplier_code",supplier_code }
                };

                return db.ExecuteNonQuery(query, parameters) > 0;
            }
        }

        // 🟢 Tìm kiếm nhà cung cấp theo tên
        public DataTable SearchSuppliersByName(string keyword)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
        SELECT 
            
            s.supplier_code AS [Mã Nhà Cung Cấp],
            s.supplier_name AS [Tên Nhà Cung Cấp],
            s.phone AS [Số Điện Thoại],
            s.email AS [Email],
            s.address AS [Địa Chỉ],
            s.note AS [Ghi Chú]
        FROM Suppliers s
        WHERE s.supplier_name LIKE @Keyword";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@Keyword", "%" + keyword + "%" }
                };

                return db.GetDataTable(query, parameters);
            }
        }

        // 🟢 Lấy danh sách nhà cung cấp theo địa chỉ
        public DataTable GetSuppliersByAddress(string address)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
        SELECT 
            s.supplier_code AS [Mã Nhà Cung Cấp],
            s.supplier_name AS [Tên Nhà Cung Cấp],
            s.phone AS [Số Điện Thoại],
            s.email AS [Email],
            s.address AS [Địa Chỉ],
            s.note AS [Ghi Chú]
        FROM Suppliers s
        WHERE s.address = @Address";

                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@Address", address }
        };

                return db.GetDataTable(query, parameters);
            }
        }

        public DataTable GetAllSupplierAddresses()
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"SELECT DISTINCT address FROM Suppliers ORDER BY address ASC";
                return db.GetDataTable(query);
            }
        }

        // 🟢 Lấy thông tin nhà cung cấp theo mã
        public DataTable GetSupplierByCode(string supplierCode)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
        SELECT 
            s.supplier_code AS [Mã Nhà Cung Cấp],
            s.supplier_name AS [Tên Nhà Cung Cấp],
            s.phone AS [Số Điện Thoại],
            s.email AS [Email],
            s.address AS [Địa Chỉ],
            s.note AS [Ghi Chú]
        FROM Suppliers s
        WHERE s.supplier_code = @SupplierCode";

                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@SupplierCode", supplierCode }
        };

                return db.GetDataTable(query, parameters);
            }
        }
      


    }
}
