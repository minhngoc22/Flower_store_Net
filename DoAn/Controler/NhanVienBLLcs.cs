using Doan.Models;
using DoAn.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace DoAn.Controler
{
    class NhanVienBLL
    {
        private NhanVienDAL nhanVienDAL = new NhanVienDAL();

        // Lấy danh sách tất cả nhân viên
        public DataTable GetAllEmployees()
        {
            return nhanVienDAL.GetAllEmployees();
        }

        // Tìm kiếm nhân viên theo tên
        public DataTable SearchEmployeeByName(string name)
        {
            return nhanVienDAL.SearchEmployeesByName(name);
        }


        // Xóa nhân viên theo ID
        public bool DeleteEmployee(string employeeId)
        {
            return nhanVienDAL.DeleteEmployee(employeeId);
        }


        public DataTable GetSortedEmployeeAddresses()
        {
            return nhanVienDAL.GetSortedEmployeeAddresses();
        }
        public DataTable GetEmployeesByAddress(string address)
        {
            return nhanVienDAL.GetEmployeesByAddress(address);
        }

        // 🟢 Thêm nhân viên thông qua DAL
        // 🟢 Thêm nhân viên thông qua DAL
        public bool AddEmployee(string fullName, string phone, string email, string address, decimal salary, string position, string note, DateTime startDate, string salaryType)
        {
            return nhanVienDAL.AddEmployee(fullName, phone, email, address, salary, position, note, startDate, salaryType);
        }


        //lấy nhân viên theo code
        public DataTable GetEmployeeByCode(string employeeCode)
        {
            return nhanVienDAL.GetEmployeeByCode(employeeCode);
        }

        public bool UpdateEmployee(string employeeCode, string fullName, string phone, string email, string address, decimal salary, decimal workingHours, string position, string note, DateTime? endDate, string salaryType)
        {
            return nhanVienDAL.UpdateEmployee(employeeCode, fullName, phone, email, address, salary,workingHours, position, note,endDate,salaryType);
        }


        // 🟢 Lấy danh sách nhân viên theo vị trí
        public DataTable GetEmployeesByPosition()
        {
            return nhanVienDAL.GetSortedPositions();
        }
    }
}
