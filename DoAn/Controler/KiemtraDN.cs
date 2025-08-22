using Doan.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using BCrypt.Net;
using DoAn.Models;

namespace DoAn.Controller
{
    class KiemtraDN : Ketnoisql
    {



        public string CheckLogin(string username, string password)
        {
            PassHash passHash = new PassHash();
            passHash.UpdateOldPasswords(); // Cập nhật mật khẩu trước khi kiểm tra đăng nhập

            string query = "SELECT password, role FROM Users WHERE Username = @Username";
            Dictionary<string, object> parameters = new Dictionary<string, object>
    {
        { "@Username", username }
    };

            using (var dt = this.GetDataTable(query, parameters))
            {
                if (dt.Rows.Count == 0) return string.Empty; // Không tìm thấy user

                string hashedPassword = dt.Rows[0]["password"] as string ?? string.Empty;
                string role = dt.Rows[0]["role"] as string ?? string.Empty;

                if (string.IsNullOrEmpty(hashedPassword))
                    return string.Empty;

                // Kiểm tra mật khẩu với BCrypt
                if (BCrypt.Net.BCrypt.Verify(password, hashedPassword))
                    return role; // Đăng nhập thành công, trả về vai trò

                return string.Empty; // Sai mật khẩu
            }
        }


    }
}
