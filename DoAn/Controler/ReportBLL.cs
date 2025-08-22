using System;
using System.Data;
using Doan.Models;
using DoAn.Models;

namespace DoAn.BLL
{
    public class ReportBLL
    {
        private ReportDAL reportDAL;

        public ReportBLL()
        {
            reportDAL = new ReportDAL();
        }

        // Phương thức lấy báo cáo tổng hợp
        public DataTable GetAllMonthlyReport()
        {
            return reportDAL.GetAllMonthlyReports();
        }

        // Phương thức lấy báo cáo doanh thu theo sản phẩm
        public DataTable GetProductRevenueReport(int month, int year)
        {
            return reportDAL.GetProductRevenueReport(month, year);
        }

        // Phương thức lấy báo cáo doanh thu theo nhân viên
        public DataTable GetEmployeeSalesReport(int month, int year)
        {
            return reportDAL.GetEmployeeSalesReport(month, year);
        }

        // Phương thức lấy báo cáo doanh thu theo khách hàng
        public DataTable GetCustomerRevenueReport(int month, int year)
        {
            return reportDAL.GetCustomerRevenueReport(month, year);
        }

        // Phương thức lấy báo cáo doanh thu theo sản phẩm (dựa trên sản phẩm trong kho)
        // Phương thức lấy báo cáo doanh thu cho tất cả sản phẩm theo tháng và năm
        public DataTable GetAllProductReport(int month, int year)
        {
            return reportDAL.GetAllProductReport(month, year);
        }



        public DataTable GetProductSalesStockReport(int month, int year)
        {
            return reportDAL.GetProductSalesStockReport(month, year);  // Truyền tháng và năm vào DAL
        }


        // Phương thức tạo báo cáo tổng hợp từ DAL
        public DataTable CreateMonthlyReport(int month, int year)
        {
            return reportDAL.CreateMonthlyReport(month, year);  // Gọi phương thức DAL
        }

        public bool DeleteReport(string reportID)
        {
            try
            {
                return reportDAL.DeleteReport(reportID); // Gọi DAL để xóa
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                throw new Exception("Lỗi khi xóa báo cáo.", ex);
            }
        }

        // báo cáo đơn hagf
        public DataTable GetOrdersReport(int month, int year)
        {
            return reportDAL.GetOrdersReport(month, year);  // Truyền tháng và năm vào DAL để lấy báo cáo đơn hàng
        }

    }
}
