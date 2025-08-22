using System;
using System.Data;
using DoAn.Models;

namespace DoAn.BLL
{
    public class CustomerBLL
    {
        private CustomerDAL customerDAL;

        public CustomerBLL()
        {
            customerDAL = new CustomerDAL();
        }

        // 🟢 Lấy danh sách khách hàng
        public DataTable GetAllCustomers()
        {
            return customerDAL.GetAllCustomers();
        }

        // 🟢 Thêm khách hàng (Kiểm tra dữ liệu trước khi gọi DAL)
        public bool AddCustomer(string customerName, string phone, string email, string address, string note)
        {
           

            return customerDAL.AddCustomer(customerName, phone, email, address, note);
        }

        // 🟢 Cập nhật khách hàng
        public bool UpdateCustomer(string customerCode, string customerName, string phone, string email, string address, string note)
        {
           

            return customerDAL.UpdateCustomer(customerCode, customerName, phone, email, address, note);
        }

        // 🟢 Xóa khách hàng
        public bool DeleteCustomer(string customerCode)
        {
            if (string.IsNullOrWhiteSpace(customerCode))
            {
                throw new ArgumentException("Mã khách hàng không hợp lệ!");
            }

            return customerDAL.DeleteCustomer(customerCode);
        }

        // 🟢 Tìm kiếm khách hàng theo số điện thoại hoặc email
        public DataTable SearchCustomersByPhoneOrEmail(string keyword)
        {
            return customerDAL.SearchCustomers(keyword);
        }

        // 🟢 Lấy khách hàng theo địa chỉ
        public DataTable GetCustomersByAddress(string address)
        {
            return customerDAL.GetCustomersByAddress(address);
        }

        // 🟢 Lấy danh sách địa chỉ khách hàng
        public DataTable GetAllCustomerAddresses()
        {
            return customerDAL.GetAllCustomerAddresses();
        }

        // 🟢 Lấy thông tin khách hàng theo mã
        public DataTable GetCustomerByCode(string customerCode)
        {
            return customerDAL.GetCustomerByCode(customerCode);
        }

        // 🟢 Lấy thông tin khách hàng theo số điện thoại
        public DataTable SearchCustomersByPhone(string phone)
        {
            return customerDAL. SearchCustomersByPhone(phone);
        }
    }
}
