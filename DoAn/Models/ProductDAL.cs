using System;
using System.Data;
using Doan.Models;
using Microsoft.Data.SqlClient;

namespace DoAn.Models
{
    public class ProductDAL
    {
        public DataTable GetAllProducts()
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
                SELECT 
                    p.id AS [Mã Sản Phẩm], 
                    p.product_name AS [Tên Sản Phẩm], 
                    c.category_name AS [Danh Mục], 
                    p.color AS [Màu Sắc],
                    p.cost_price AS [Giá Nhập],  -- Add cost_price here
                    p.price AS [Giá Bán], 
                    p.discount AS [Giảm Giá],
                    p.stock_quantity AS [Số Lượng], 
                    p.unit AS [Đơn Vị],
                    s.supplier_name AS [Nhà Cung Cấp],
                    p.import_date AS [Ngày Nhập]
               --    p.note AS [Ghi Chú] -- dài quá nên bỏ
                FROM Products p
                INNER JOIN Categories c ON p.category_id = c.id
                INNER JOIN Suppliers s ON p.supplier_id = s.id";

                return db.GetDataTable(query);
            }
        }

        public DataTable GetAllCategories()
        {
            using (Ketnoisql db = new Ketnoisql())  // Sử dụng lớp kết nối CSDL
            {
                string query = "SELECT id, category_name FROM Categories";
                return db.GetDataTable(query); // Gọi phương thức lấy dữ liệu từ Ketnoisql
            }
        }

        public DataTable GetProductsByCategory(int categoryId)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
                SELECT 
                    p.id AS [Mã Sản Phẩm], 
                    p.product_name AS [Tên Sản Phẩm], 
                    c.category_name AS [Danh Mục], 
                    p.color AS [Màu Sắc],
                    p.cost_price AS [Giá Nhập],  -- Add cost_price here
                    p.price AS [Giá Bán], 
                    p.discount AS [Giảm Giá],
                    p.stock_quantity AS [Số Lượng], 
                    p.unit AS [Đơn Vị],
                    s.supplier_name AS [Nhà Cung Cấp],
    p.import_date AS [Ngày Nhập]
               --    p.note AS [Ghi Chú] -- dài quá nên bỏ
                FROM Products p
                INNER JOIN Categories c ON p.category_id = c.id
                INNER JOIN Suppliers s ON p.supplier_id = s.id
                WHERE p.category_id = @CategoryId";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@CategoryId", categoryId }
                };

                return db.GetDataTable(query, parameters);
            }
        }

        public DataTable SearchProductsByName(string keyword)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
                SELECT 
                    p.id AS [Mã Sản Phẩm], 
                    p.product_name AS [Tên Sản Phẩm], 
                    c.category_name AS [Danh Mục], 
                    p.color AS [Màu Sắc],   
                    p.cost_price AS [Giá Nhập],  -- Add cost_price here
                    p.price AS [Giá Bán], 
                    p.discount AS [Giảm Giá],
                    p.stock_quantity AS [Số Lượng], 
                    p.unit AS [Đơn Vị],
                    s.supplier_name AS [Nhà Cung Cấp],
    p.import_date AS [Ngày Nhập]
               --    p.note AS [Ghi Chú] -- dài quá nên bỏ
                FROM Products p
                INNER JOIN Categories c ON p.category_id = c.id
                INNER JOIN Suppliers s ON p.supplier_id = s.id
                WHERE p.product_name LIKE @keyword";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@keyword", "%" + keyword + "%" }
                };

                return db.GetDataTable(query, parameters);
            }
        }

        public bool DeleteProduct(string productId)
        {
            using (Ketnoisql db = new Ketnoisql()) // Sử dụng lớp Ketnoisql
            {
                string query = "DELETE FROM Products WHERE id = @ProductId";

                // Tạo Dictionary chứa tham số truyền vào
                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@ProductId", productId }
        };

                // Gọi phương thức thực thi từ Ketnoisql (giả sử có phương thức ExecuteNonQuery)
                int rowsAffected = db.ExecuteNonQuery(query, parameters);

                return rowsAffected > 0;
            }
        }

        public DataTable GetAllSuppliers()
        {
            using (Ketnoisql db = new Ketnoisql())  // Sử dụng lớp kết nối CSDL
            {
                string query = "SELECT id, supplier_name FROM Suppliers";
                return db.GetDataTable(query); // Gọi phương thức lấy dữ liệu từ Ketnoisql
            }
        }

        public DataTable GetProductByID(string productID)
        {
            using (Ketnoisql db = new Ketnoisql())  // Sử dụng lớp kết nối CSDL
            {
                string query = @"
        SELECT 
            p.id AS [Mã Sản Phẩm], 
            p.product_name AS [Tên Sản Phẩm], 
            c.category_name AS [Danh Mục], 
            p.color AS [Màu Sắc],
            p.cost_price AS [Giá Nhập],  -- Add cost_price here
            p.price AS [Giá Bán], 
            p.discount AS [Giảm Giá],
            p.stock_quantity AS [Số Lượng], 
            p.unit AS [Đơn Vị],
            s.supplier_name AS [Nhà Cung Cấp], 
            p.image_path AS [Hình Ảnh],
            p.import_date AS [Ngày Nhập],
            p.note AS [Ghi Chú]
        FROM Products p
        INNER JOIN Categories c ON p.category_id = c.id
        INNER JOIN Suppliers s ON p.supplier_id = s.id
        WHERE p.id = @ProductID";

                // Dictionary chứa tham số truyền vào
                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@ProductID", productID }
        };

                return db.GetDataTable(query, parameters);
            }
        }

        public bool InsertProduct(string productId, string productName, string color, decimal costPrice, decimal price,  int stockQuantity,string unit, int categoryId, int supplierId, string imagePath, string note, DateTime importDate)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
        INSERT INTO Products 
        (id, product_name, category_id, color, cost_price, price, stock_quantity, unit,supplier_id, image_path, note, import_date)
        VALUES 
        (@ProductId, @ProductName, @CategoryId, @Color, @CostPrice, @Price, @StockQuantity,@Unit, @SupplierId, @ImagePath, @Note, @ImportDate)";

                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@ProductId", productId },
            { "@ProductName", productName },
            { "@CategoryId", categoryId },
            { "@Color", color },
            { "@CostPrice", costPrice },
            { "@Price", price },
            { "@StockQuantity", stockQuantity },
            { "@Unit", unit },
            { "@SupplierId", supplierId },
            { "@ImagePath", imagePath },
            { "@Note", note },
            { "@ImportDate", importDate }
        };

                int rowsAffected = db.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
        }



        // Modify UpdateProduct method to include cost_price before price
        public bool UpdateProduct(string productId, string productName, string color, decimal costPrice, decimal price, decimal discount, int stockQuantity,string unit, int categoryId, int supplierId, string imagePath, string note, DateTime importDate)
        {
            using (Ketnoisql db = new Ketnoisql())
            {
                string query = @"
        UPDATE Products 
        SET 
            product_name = @ProductName, 
            color = @Color,
            cost_price = @CostPrice,
            price = @Price, 
            discount = @Discount,
            stock_quantity = @StockQuantity, 
            unit = @Unit,
            category_id = @CategoryId, 
            supplier_id = @SupplierId, 
            image_path = @ImagePath,
            note = @Note,
            import_date = @ImportDate
        WHERE id = @ProductId";

                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@ProductId", productId },
            { "@ProductName", productName },
            { "@Color", color },
            { "@CostPrice", costPrice },
            { "@Price", price },
            { "@Discount", discount },
            { "@StockQuantity", stockQuantity },
            { "@Unit", unit },
            { "@CategoryId", categoryId },
            { "@SupplierId", supplierId },
            { "@ImagePath", imagePath },
            { "@Note", note },
            { "@ImportDate", importDate }
        };

                int rowsAffected = db.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
        }

        public DataTable SearchProductIDClosest(string searchKeyword)
        {
            using (Ketnoisql db = new Ketnoisql())  // Sử dụng lớp kết nối CSDL
            {
                string query = @"
            SELECT 
                p.id AS [Mã Sản Phẩm]
            FROM Products p
            WHERE p.id LIKE @SearchKeyword
            ORDER BY LEN(p.id) ASC";  // Sắp xếp theo độ dài mã sản phẩm, có thể thay đổi tùy theo yêu cầu

                Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@SearchKeyword", searchKeyword + "%" }
            };

                return db.GetDataTable(query, parameters);
            }
        }






    }
}
