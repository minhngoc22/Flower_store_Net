using System;
using System.Collections.Generic;
using System.Data;
using BCrypt.Net;
using Doan.Models;

namespace DoAn.Models
{
    class PassHash : Ketnoisql
    {
        // Phương thức cập nhật mật khẩu cũ thành mật khẩu mã hóa
        public void UpdateOldPasswords()
        {
            string query = "SELECT id, password FROM Users";
            DataTable dt = this.GetDataTable(query, null);

            foreach (DataRow row in dt.Rows)
            {
                int userId = Convert.ToInt32(row["id"]);
                string oldPassword = row["password"].ToString();

                // Kiểm tra nếu mật khẩu chưa được mã hóa
                if (!oldPassword.StartsWith("$2"))
                {
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(oldPassword);
                    string updateQuery = "UPDATE Users SET password = @HashedPassword WHERE id = @UserID";
                    Dictionary<string, object> parameters = new Dictionary<string, object>
                    {
                        { "@HashedPassword", hashedPassword },
                        { "@UserID", userId }
                    };
                    this.ExecuteNonQuery(updateQuery, parameters);
                }
            }
            Console.WriteLine("Cập nhật mật khẩu thành công!");
        }
    }
}