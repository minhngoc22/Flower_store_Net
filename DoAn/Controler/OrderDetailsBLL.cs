using DoAn.Controler;
using DoAn.Models;
using System.Data;

namespace DoAn.BLL
{
    public class OrderDetailsBLL
    {
        private OrderDetailsDAL orderDetailsDAL;

        public OrderDetailsBLL()
        {
            orderDetailsDAL = new OrderDetailsDAL();
        }

        // 🟢 Lấy danh sách chi tiết đơn hàng theo mã đơn hàng
        public DataTable GetAll(string orderCode)
        {
            return orderDetailsDAL.GetAll(orderCode);
        }

        // Lấy tổng tiền của đơn hàng theo mã đơn hàng
        public decimal GetTotalPriceByOrderCode(string orderCode)
        {
            return orderDetailsDAL.GetTotalPriceByOrderCode(orderCode);
        }


        // Lấy danh sách trạng thái đơn hàng
        public DataTable GetAllOrderStatuses()
        {
            return orderDetailsDAL.GetAllOrderStatuses();
        }

        // Lấy danh sách trạng thái thanh toán
        public DataTable GetAllPaymentStatuses()
        {
            return orderDetailsDAL.GetAllPaymentStatuses();
        }

        public decimal GetProductPrice(string productCode)
        {
            if (string.IsNullOrEmpty(productCode))
                return 0;

            return orderDetailsDAL.GetProductPrice(productCode);
        }

        //lấy giảm giá của sản phẩm
        public decimal GetProductDiscount(string productCode)
        {
            if (string.IsNullOrEmpty(productCode))
                return 0;
            return orderDetailsDAL.GetProductDiscount(productCode);
        }

        public bool AddOrderDetail(string orderCode, string productId, int quantity, decimal unitPrice, string note)
        {
            return orderDetailsDAL.InsertOrderDetail(orderCode, productId, quantity, unitPrice, note);
        }

        //lấy đơn hàng vừa tạo
        public DataTable GetOrderDetailsByOrderCode(string orderCode)
        {
            return orderDetailsDAL.GetOrderDetailsByOrderCode(orderCode);
        }

        //lấy tên nhân viên
        public string GetEmployeeNameByOrderCodestring(string orderCode)
        {

            return orderDetailsDAL.GetEmployeeNameByOrderCode(orderCode);

        }
    }
}
