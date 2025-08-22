using System;
using System.Data;
using System.Collections.Generic;
using DoAn.Models;

namespace DoAn.BLL
{
    public class OrderBLL
    {
        private OrderDAL orderDAL;

        public OrderBLL()
        {
            orderDAL = new OrderDAL();
        }

        // Lấy danh sách đơn hàng
        public DataTable GetAllOrders()
        {
            return orderDAL.GetAllOrders();
        }

        // Thêm đơn hàng
        public string AddOrder(string customerPhone, string employeeName, string shippingAddress = null)
        {
            return orderDAL.InsertOrder(customerPhone, employeeName,shippingAddress);
        }

        // Cập nhật đơn hàng
        public bool UpdateOrder(string orderCode, decimal total_amount, string status, string employeeId, string payment, string note, string shippingAddress)
        {
            return orderDAL.UpdateOrder(orderCode,total_amount, status, employeeId, payment, note,shippingAddress);
        }

        // Xóa đơn hàng
        public bool DeleteOrder(int orderId)
        {
            return orderDAL.DeleteOrder(orderId);
        }

        public DataTable GetAllOrderStatuses()
        {
            return orderDAL.GetAllOrderStatuses();
        }

        public DataTable GetOrderByStatus(string address)
        {
            return orderDAL.GetOrderByStatus(address);
        }

        // Gọi phương thức từ DAL để lấy đơn hàng theo mã
        public DataTable GetOrderByCode(string orderCode)
        {
            return orderDAL.GetOrderByCode(orderCode);
        }

        public DataTable GetOrderStatusList()
        {
            return orderDAL.GetOrderStatusList();
        }

        public DataTable GetEmployeeList()
        {
            return orderDAL.GetEmployees();
        }

        public DataTable GetPaymentMethods()
        {
            return orderDAL.GetPaymentMethods();
        }

        // 🟢 Gọi hàm DAL để lấy khách hàng theo số điện thoại hoặc email
        public DataTable GetCustomerByPhoneOrEmail(string input)
        {
            return orderDAL.GetCustomerByPhoneOrEmail(input);
        }

        // 🟢 Lấy danh sách chi tiết đơn hàng theo mã đơn hàng
        public DataTable GetOrderDetailsByOrderCode(string orderCode)
        {
            return orderDAL.GetOrderByCode(orderCode);
        }
        public DataTable GetOrderStatus(string orderCode)
        {
            return orderDAL.GetOrderStatus(orderCode);
        }

    }
}