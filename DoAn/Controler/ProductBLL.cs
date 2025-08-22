
using DoAn.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn.Controler
{
    class ProductBLL
    {
        private ProductDAL productDAL = new ProductDAL();

        // Lấy tất cả sản phẩm
        public DataTable GetAllProducts()
        {
            return productDAL.GetAllProducts();
        }
        public DataTable GetAllCategories()
        {
            return productDAL.GetAllCategories();
        }

        // Lấy sản phẩm theo danh mục
        public DataTable GetProductsByCategory(int categoryId)
        {
            return productDAL.GetProductsByCategory(categoryId);
        }

        // Tìm kiếm sản phẩm theo tên
        public DataTable SearchProductsByName(string keyword)
        {
            return productDAL.SearchProductsByName(keyword);
        }


        // Xóa sản phẩm theo ID
        public bool DeleteProduct(string productId)
        {
            return productDAL.DeleteProduct(productId); // Gọi hàm từ DAL
        }

        
        // Lấy danh sách tất cả nhà cung cấp
        public DataTable GetAllSuppliers()
        {
            return productDAL.GetAllSuppliers(); // Gọi phương thức từ ProductDAL
        }

        public DataTable GetProductByID(string productID)
        {
            return productDAL.GetProductByID(productID);
        }

        public bool UpdateProduct(string productId, string productName, string color,decimal costPrice, decimal price, decimal discount, int stockQuantity, string unit,int categoryId, int supplierId, string imagePath, string note, DateTime importDate)
        {
            if (string.IsNullOrWhiteSpace(productId) || string.IsNullOrWhiteSpace(productName))
            {
                throw new ArgumentException("Mã sản phẩm và tên sản phẩm không được để trống.");
            }
            return productDAL.UpdateProduct(productId, productName,color,costPrice, price, discount, stockQuantity,unit, categoryId, supplierId, imagePath, note,importDate);
        }

        public bool InsertProduct(string productId, string productName,string color, decimal costPrice, decimal price, int stockQuantity,string unit, int categoryId, int supplierId, string imagePath, string note, DateTime importDate)
        {
            if (string.IsNullOrWhiteSpace(productId) || string.IsNullOrWhiteSpace(productName))
            {
                throw new ArgumentException("Mã sản phẩm và tên sản phẩm không được để trống.");
            }
            return productDAL.InsertProduct(productId, productName,color,costPrice, price, stockQuantity,unit, categoryId, supplierId, imagePath, note,importDate);
        }

        public string GetClosestProductID(string searchKeyword)
        {
            // Gọi DAL để lấy mã sản phẩm khớp nhất
            ProductDAL dal = new ProductDAL();
            DataTable result = dal.SearchProductIDClosest(searchKeyword);

            // Nếu có kết quả trả về, lấy mã sản phẩm đầu tiên
            if (result.Rows.Count > 0)
            {
                return result.Rows[0]["Mã Sản Phẩm"].ToString();
            }

            return null; // Trả về null nếu không tìm thấy mã sản phẩm
        }

    }
}
