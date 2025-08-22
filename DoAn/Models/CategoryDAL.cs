using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Doan.Models;

namespace DoAn.Models
{
    public class CategoryDAL
    {
        // 🟢 Lấy toàn bộ danh mục
        public DataTable GetAllCategories()
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
                SELECT 
                    category_code AS [Mã Danh Mục],
                    category_name AS [Tên Danh Mục],
                    note AS [Ghi Chú]
                FROM Categories";

                return db.GetDataTable(query);
            }
        }

        // 🟢 Thêm danh mục mới
        public bool AddCategory(string categoryName, string note)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
                INSERT INTO Categories (category_name, note)
                VALUES (@CategoryName, @Note)";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@CategoryName", categoryName },
                    { "@Note", string.IsNullOrEmpty(note) ? DBNull.Value : note }
                };

                return db.ExecuteNonQuery(query, parameters) > 0;
            }
        }

        // 🟢 Cập nhật danh mục
        public bool UpdateCategory(string category_code, string categoryName, string note)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
                UPDATE Categories
                SET category_name = @CategoryName,
                    note = @Note
                WHERE category_code = @CategoryCode";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@CategoryCode", category_code },
                    { "@CategoryName", categoryName },
                    { "@Note", string.IsNullOrEmpty(note) ? DBNull.Value : note }
                };

                return db.ExecuteNonQuery(query, parameters) > 0;
            }
        }

        // 🟢 Xóa danh mục
        public bool DeleteCategory(string category_code)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = "DELETE FROM Categories WHERE category_code = @CategoryCode";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@CategoryCode", category_code }
                };

                return db.ExecuteNonQuery(query, parameters) > 0;
            }
        }

        // 🟢 Tìm kiếm danh mục theo tên
        public DataTable SearchCategoriesByName(string keyword)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
                SELECT 
                    category_code AS [Mã Danh Mục],
                    category_name AS [Tên Danh Mục],
                    note AS [Ghi Chú]
                FROM Categories
                WHERE category_name LIKE @Keyword";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@Keyword", "%" + keyword + "%" }
                };

                return db.GetDataTable(query, parameters);
            }
        }

        // 🟢 Lấy thông tin danh mục theo mã
        public DataTable GetCategoryByCode(string category_code)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
                SELECT 
                    category_code AS [Mã Danh Mục],
                    category_name AS [Tên Danh Mục],
                    note AS [Ghi Chú]
                FROM Categories
                WHERE category_code = @CategoryCode";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@CategoryCode", category_code }
                };

                return db.GetDataTable(query, parameters);
            }
        }
    }
}
