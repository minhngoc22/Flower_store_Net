using System;
using System.Collections.Generic;
using System.Data;
using BCrypt.Net;  // Thêm thư viện BCrypt
using Doan.Models;
using DoAn.Models;

namespace DoAn.Models
{
    public class UserDAL
    {
        // 🟢 Lấy danh sách tất cả người dùng
        public DataTable GetAllUsers()
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
        SELECT 
            id AS [Mã Người Dùng], 
            user_code AS [Mã Tài Khoản], 
            username AS [Tên Đăng Nhập], 
            password AS [Mật Khẩu],
            full_name AS [Họ Và Tên], 
            role AS [Vai Trò], 
            note AS [Ghi Chú] 
        FROM Users";

                return db.GetDataTable(query);
            }
        }


        // 🟢 Lấy người dùng theo ID
        public DataRow? GetUserById(string user_code)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
        SELECT 
          
            user_code AS [Mã Tài Khoản], 
            username AS [Tên Đăng Nhập], 
 password AS [Mật Khẩu],
            full_name AS [Họ Và Tên], 
            role AS [Vai Trò], 
            note AS [Ghi Chú] 
        FROM Users 
        WHERE user_code = @UserCode";

                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@UserCode", user_code }
        };

                DataTable dt = db.GetDataTable(query, parameters);
                return dt.Rows.Count > 0 ? dt.Rows[0] : null;
            }
        }


        // 🟢 Thêm người dùng mới (user_code tự động sinh)
        // 🟢 Thêm người dùng mới (user_code tự động sinh)
        public bool AddUser(string username, string password, string fullName, string email, string role, string note)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                // Kiểm tra xem vai trò có hợp lệ không
                DataTable rolesTable = GetAllRoles();
                List<string> validRoles = new List<string>();
                foreach (DataRow row in rolesTable.Rows)
                {
                    validRoles.Add(row["role"].ToString());
                }

                if (!validRoles.Contains(role))
                {
                    MessageBox.Show("Vai trò không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // Mã hóa mật khẩu
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                string query = @"
        INSERT INTO Users (username, password, full_name, email, role, note) 
        VALUES (@Username, @Password, @FullName, @Email, @Role, @Note)";

                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@Username", username },
            { "@Password", hashedPassword },  // Lưu mật khẩu đã mã hóa
            { "@FullName", fullName },
            { "@Email", email },
            { "@Role", role },
            { "@Note", string.IsNullOrEmpty(note) ? DBNull.Value : note }
        };

                int rowsAffected = db.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
        }


        // 🟢 Cập nhật thông tin người dùng
        public bool UpdateUser(string user_code, string fullName, string role, string note, string newPassword)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                // Lấy ID từ user_code
                int userId = GetUserIdByCode(user_code);

                if (userId == -1)
                {
                    MessageBox.Show("Không tìm thấy người dùng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                string query = @"
        UPDATE Users 
        SET full_name = @FullName, 
            role = @Role,
            note = @Note";

                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@UserId", userId },
            { "@FullName", fullName },
            { "@Role", role },
            { "@Note", string.IsNullOrEmpty(note) ? DBNull.Value : note }
        };

                // Nếu có nhập mật khẩu mới, thì cập nhật mật khẩu
                if (!string.IsNullOrEmpty(newPassword))
                {
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
                    query += ", password = @Password";
                    parameters.Add("@Password", hashedPassword);
                }

                query += " WHERE id = @UserId";

                int rowsAffected = db.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
        
}
        public int GetUserIdByCode(string user_code)
        {
            using (Ketnoisql db = new Ketnoisql())  // Dùng Ketnoisql thay vì DataProvider
            {
                string query = "SELECT id FROM Users WHERE user_code = @user_code";
                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@user_code", user_code }
        };

                DataTable dt = db.GetDataTable(query, parameters);
                if (dt.Rows.Count > 0)
                {
                    return Convert.ToInt32(dt.Rows[0]["id"]);
                }
                return -1; // Trả về -1 nếu không tìm thấy user
            }
        }


        // 🟢 Xóa người dùng
        public bool DeleteUser(int userId)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = "DELETE FROM Users WHERE id = @UserId";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@UserId", userId }
                };

                int rowsAffected = db.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
        }

        // 🟢 Tìm kiếm người dùng theo username hoặc tên đầy đủ
        public DataTable SearchUsers(string keyword)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
        SELECT 
            id AS [Mã Người Dùng], 
            user_code AS [Mã Tài Khoản], 
            username AS [Tên Đăng Nhập], 
 password AS [Mật Khẩu],
            full_name AS [Họ Và Tên], 
            role AS [Vai Trò], 
            note AS [Ghi Chú] 
        FROM Users 
        WHERE full_name LIKE @Keyword OR username LIKE @Keyword";

                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@Keyword", "%" + keyword + "%" }
        };

                return db.GetDataTable(query, parameters);
            }
        }




        // 🟢 Lấy danh sách tất cả vai trò từ bảng Users
        public DataTable GetAllRoles()
        {
            using (Ketnoisql db = new Ketnoisql()) { 
            string query = "SELECT DISTINCT role FROM Users"; // Lấy danh sách vai trò không trùng lặp
            return db.GetDataTable(query, null);
        }
        }


        // 🟢 Lấy danh sách người dùng theo vai trò
        public DataTable GetUsersByRole(string role)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
        SELECT 
            id AS [Mã Người Dùng], 
            user_code AS [Mã Tài Khoản], 
            username AS [Tên Đăng Nhập], 
 password AS [Mật Khẩu],
            full_name AS [Họ Và Tên], 
            role AS [Vai Trò], 
            note AS [Ghi Chú] 
        FROM Users 
        WHERE role = @Role";

                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@Role", role }
        };

                return db.GetDataTable(query, parameters);
            }
        }

        public string GetUserRoleById(string user_code)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = "SELECT role FROM Users WHERE user_code = @UserCode";

                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@UserCode", user_code }
        };

                DataTable dt = db.GetDataTable(query, parameters);
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["role"].ToString();
                }
                return string.Empty;
            }
        }

        


    }
}
