using System;
using System.Data;
using DoAn.Models;

namespace DoAn.BLL
{
    public class UserBLL
    {
        private UserDAL userDAL;

        public UserBLL()
        {
            userDAL = new UserDAL();
        }

        // 🟢 Lấy danh sách tất cả người dùng
        public DataTable GetAllUsers()
        {
            return userDAL.GetAllUsers();
        }

        // 🟢 Lấy thông tin người dùng theo ID
        public DataRow? GetUserById(string user_code)
        {
            return userDAL.GetUserById(user_code);
        }

        // 🟢 Thêm người dùng mới
        public bool AddUser(string username, string password, string fullName,string email, string role, string note)
        {
            return userDAL.AddUser(username, password, fullName,email, role, note);
        }

        // 🟢 Cập nhật thông tin người dùng
        // 🟢 Cập nhật thông tin người dùng
        public bool UpdateUser(string user_code, string fullName, string role, string note, string newPassword)
        {
            return userDAL.UpdateUser(user_code, fullName, role, note, newPassword);
        }



        // 🟢 Xóa người dùng
        public bool DeleteUser(int userId)
        {
            return userDAL.DeleteUser(userId);
        }

        // 🟢 Tìm kiếm người dùng
        public DataTable SearchUsers(string keyword)
        {
            return userDAL.SearchUsers(keyword);
        }

        public DataTable GetAllRoles()
        {
            return userDAL.GetAllRoles();
        }

        public DataTable GetUsersByRole(string role)
        {
            return userDAL.GetUsersByRole(role);
        }

        // 🟢 Lấy vai trò theo ID người dùng
        public string GetUserRoleById(string user_code)
        {
            return userDAL.GetUserRoleById(user_code);
        }// 🟢 Lấy vai trò theo ID người dùng
    

    }
}
