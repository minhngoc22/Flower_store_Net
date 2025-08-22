using System;
using System.Data;
using System.Collections.Generic;
using DoAn.Models;
using Microsoft.Data.SqlClient;
using Doan.Models;

namespace DoAn.Models
{
    public class NhanVienDAL
    {
        // 🟢 Lấy danh sách tất cả nhân viên
        // 🟢 Lấy danh sách tất cả nhân viên
        public DataTable GetAllEmployees()
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
                SELECT 
                    e.employee_code AS [Mã Nhân Viên], 
                    e.full_name AS [Họ Tên], 
                    e.position AS [Chức Vụ], 
                    e.phone AS [Số Điện Thoại], 
                    e.email AS [Email],
                    e.salary AS [Lương/giờ], 
                    e.salary_type As [Hệ số lương],
                    e.working_hours AS [Giờ làm/tháng],
                    e.address AS [Địa Chỉ] ,
                    e.start_date AS [Ngày Bắt Đầu]  ,
                    e.note AS [Ghi Chú] ,
                    e.end_date AS [Ngày Kết Thúc]

                FROM Employees e";

                return db.RefreshData(query);
            }
        }


        // 🟢 Lấy nhân viên theo ID

        // 🟢 Lấy nhân viên theo ID
     

        // 🟢 Xóa nhân viên
        public bool DeleteEmployee(string employeecode)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = "DELETE FROM Employees WHERE  employee_code = @EmployeeCode";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@EmployeeCode", employeecode }
                };

                int rowsAffected = db.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
        }

        // 🟢 Tìm kiếm nhân viên theo tên

        // 🟢 Tìm kiếm nhân viên theo tên
        public DataTable SearchEmployeesByName(string keyword)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
                SELECT 
                    e.employee_code AS [Mã Nhân Viên], 
                    e.full_name AS [Họ Tên], 
                    e.position AS [Chức Vụ], 
                    e.phone AS [Số Điện Thoại], 
                    e.email AS [Email],
                    e.salary AS [Lương/giờ], 
e.salary_type As [Hệ số lương],
                    e.working_hours AS [Giờ làm/tháng],
                    e.address AS [Địa Chỉ] ,
 e.start_date AS [Ngày Bắt Đầu]  ,
                    e.note AS [Ghi Chú] ,
                    e.end_date AS [Ngày Kết Thúc]

                FROM Employees e
                WHERE e.full_name LIKE @keyword";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@keyword", "%" + keyword + "%" }
                };

                return db.GetDataTable(query, parameters);
            }
        }

        public DataTable GetSortedEmployeeAddresses()
        {
            using (Ketnoisql db = new Ketnoisql()) // Sử dụng lớp kết nối CSDL
            {
                string query = @"
        SELECT DISTINCT address 
        FROM Employees
        ORDER BY address ASC"; // Sắp xếp theo địa chỉ

                return db.GetDataTable(query); // Gọi phương thức lấy dữ liệu từ Ketnoisql
            }
        }

        public DataTable GetEmployeesByAddress(string address)
        {
            using (Ketnoisql db = new Ketnoisql()) // Sử dụng lớp kết nối CSDL
            {
                string query = @"
        SELECT 
            e.employee_code AS [Mã Nhân Viên], 
            e.full_name AS [Họ Tên], 
            e.position AS [Chức Vụ], 
            e.phone AS [Số Điện Thoại], 
            e.email AS [Email],
            e.salary AS [Lương], 
e.salary_type As [Hệ số lương],
  e.working_hours AS [Giờ làm/tháng],
            e.address AS [Địa Chỉ] ,
 e.start_date AS [Ngày Bắt Đầu]  ,
                    e.note AS [Ghi Chú] ,
                    e.end_date AS [Ngày Kết Thúc]
        FROM Employees e
        WHERE e.address = @Address";

                // Tạo tham số truyền vào
                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@Address", address }
        };

                return db.GetDataTable(query, parameters);
            }
        }

        // 🟢 Thêm nhân viên mới
        public bool AddEmployee(string fullName, string phone, string email, string address, decimal salary, string position, string note, DateTime startDate, string salaryType)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
        INSERT INTO Employees (full_name, phone, email, address, salary, position, note, start_date, salary_type) 
        VALUES (@FullName, @Phone, @Email, @Address, @Salary, @Position, @Note, @StartDate, @SalaryType)";

                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@FullName", fullName },
            { "@Phone", phone },
            { "@Email", string.IsNullOrEmpty(email) ? DBNull.Value : email },
            { "@Address", string.IsNullOrEmpty(address) ? DBNull.Value : address },
            { "@Salary", salary },
            { "@Position", position },
            { "@Note", string.IsNullOrEmpty(note) ? DBNull.Value : note },
            { "@StartDate", startDate },
            { "@SalaryType", string.IsNullOrEmpty(salaryType) ? DBNull.Value : salaryType }
        };

                int rowsAffected = db.ExecuteNonQuery(query, parameters);

                return rowsAffected > 0;
            }
        }

        // 🟢 Lấy nhân viên theo ID
        public DataTable GetEmployeeByCode(string employeeCode)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
                SELECT 
                    e.employee_code AS [Mã Nhân Viên], 
                    e.full_name AS [Họ Và Tên], 
                    e.phone AS [Số Điện Thoại], 
                    e.email AS [Email], 
                    e.address AS [Địa Chỉ], 
                    e.salary AS [Lương/giờ], 
                e.salary_type AS [Hệ số lương],
                    e.working_hours AS [Giờ làm trong tháng], 
                    e.position AS [Chức Vụ], 
                    e.note AS [Ghi Chú],
                    e.end_date AS [Ngày Kết Thúc]
                FROM Employees e
                WHERE e.employee_code = @EmployeeCode";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@EmployeeCode", employeeCode }
                };

                return db.GetDataTable(query, parameters);
            }
        }

        // 🟢 Cập nhật thông tin nhân viên
        // 🟢 Cập nhật thông tin nhân viên
        public bool UpdateEmployee(string employeeCode, string fullName, string phone, string email, string address, decimal salary, decimal workingHours, string position, string note, DateTime? endDate, string salaryType)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                // Cập nhật thông tin nhân viên bao gồm salary_type
                string query = @"
        UPDATE Employees 
        SET full_name = @FullName, 
            phone = @Phone, 
            email = @Email, 
            address = @Address, 
            salary = @Salary, 
            salary_type = @SalaryType,
            working_hours = @WorkingHours, 
            position = @Position, 
            note = @Note, 
            end_date = @EndDate
        WHERE employee_code = @EmployeeCode";

                // Thêm tham số cho câu lệnh SQL
                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@EmployeeCode", employeeCode },
            { "@FullName", fullName },
            { "@Phone", phone },
            { "@Email", string.IsNullOrEmpty(email) ? DBNull.Value : email },
            { "@Address", string.IsNullOrEmpty(address) ? DBNull.Value : address },
            { "@Salary", salary },
            { "@SalaryType", salaryType }, // Thêm tham số cho salary_type
            { "@WorkingHours", workingHours },
            { "@Position", position },
            { "@Note", string.IsNullOrEmpty(note) ? DBNull.Value : note },
            { "@EndDate", endDate.HasValue ? endDate.Value : (object)DBNull.Value }
        };

                // Thực thi câu lệnh SQL
                int rowsAffected = db.ExecuteNonQuery(query, parameters);

                return rowsAffected > 0;
            }
        }


        // 🟢 Lấy danh sách vai trò
        public DataTable GetSortedPositions()
        {
            using (Ketnoisql db = new Ketnoisql()) // Sử dụng lớp kết nối CSDL
            {
                string query = @"
        SELECT DISTINCT position 
        FROM Employees
        WHERE position IS NOT NULL
        ORDER BY position ASC"; // Sắp xếp theo vị trí (chức vụ)

                return db.GetDataTable(query); // Gọi phương thức lấy dữ liệu từ Ketnoisql
            }
        }

    }
}
