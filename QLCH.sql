-- Tạo database
CREATE DATABASE QLCHHoa;
GO

USE QLCHHoa;
GO 

-- Tạo bảng Users với ràng buộc CHECK và DEFAULT cho cột role
CREATE TABLE Users (
    id INT IDENTITY(1,1) PRIMARY KEY, 
    user_code VARCHAR(10) NULL,
    username VARCHAR(50) NOT NULL, 
    email VARCHAR(100) NOT NULL UNIQUE,  -- Thêm cột email
    password VARCHAR(255) NOT NULL, 
    full_name NVARCHAR(100) NOT NULL,
    role VARCHAR(20) NOT NULL DEFAULT 'Khách Hàng' CHECK (role IN ('Admin', 'Nhân viên', 'Khách Hàng')),
    note NVARCHAR(255) NULL
);

ALTER TABLE Users
ADD ResetPasswordToken VARCHAR(255) NULL,
    ResetPasswordExpiry DATETIME NULL;


-- Tự động tạo user_code
CREATE TRIGGER trg_AutoGenerateUserCode
ON Users
AFTER INSERT
AS
BEGIN
    UPDATE Users
    SET user_code = 'USER-' + RIGHT('000' + CAST(id AS VARCHAR(3)), 3)
    WHERE user_code IS NULL;
END;
-- Chèn dữ liệu vào bảng Users
-- Chèn dữ liệu vào bảng Users
INSERT INTO Users (username, email, password, full_name, role, note) VALUES
('admin1', 'admin1@example.com', '123456', N'Nguyễn Hoàng Minh', 'Admin', N'Quản lý chính'),
('staff1', 'staff1@example.com', '123456', N'Lê Thị Hoa', 'Nhân viên', N'Nhân viên bán hoa'),
('staff2', 'staff2@example.com', '123456', N'Trần Thanh Tùng', 'Nhân viên', N'Nhân viên bán phụ kiện'),
('admin2', 'admin2@example.com', '123456', N'Phạm Văn Long', 'Admin', N'Phụ trách hệ thống'),
('khach1', 'khach1@example.com', '123456', N'Nguyễn Văn Khách', 'Khách Hàng', N'Khách đăng ký tài khoản');

CREATE TABLE Employees (
    id INT IDENTITY(1,1) PRIMARY KEY,
    employee_code VARCHAR(20) NULL,
    full_name NVARCHAR(100) NOT NULL,
    user_id INT NULL,
    phone VARCHAR(15) NOT NULL,
    email VARCHAR(100) NULL,
    address NVARCHAR(255) NULL,
    salary DECIMAL(10,2) NOT NULL,
    working_hours DECIMAL(4,2) NOT NULL DEFAULT 0.0,
    position NVARCHAR(50) NOT NULL,
    note NVARCHAR(255) NULL,
    start_date DATE NOT NULL,
    end_date DATE NULL,
    salary_type VARCHAR(10) NOT NULL DEFAULT 'monthly' CHECK (salary_type IN ('hourly', 'monthly')),
    CONSTRAINT chk_WorkingHours_Max CHECK (working_hours <= 248.0),
    FOREIGN KEY (user_id) REFERENCES Users(id) ON DELETE SET NULL
);

-- Trigger for auto-generating employee code
CREATE TRIGGER trg_AutoGenerateEmployeeCode
ON Employees
AFTER INSERT
AS
BEGIN
    UPDATE Employees
    SET employee_code = 'EMP-' + RIGHT('000' + CAST(id AS VARCHAR(3)), 3)
    WHERE employee_code IS NULL;
END;





-- Liên kết user_id với nhân viên
DECLARE @UserId1 INT = (SELECT id FROM Users WHERE email = 'staff1@example.com');
DECLARE @UserId2 INT = (SELECT id FROM Users WHERE email = 'staff2@example.com');

-- Chèn dữ liệu mẫu vào bảng Employees
INSERT INTO Employees (
    full_name, phone, email, address,
    salary, working_hours, position, note,
    user_id, start_date, end_date
)
VALUES
(N'Lê Thị Hoa', '0912345678', 'staff1@example.com', N'Hà Nội',
 50000, 8.0, N'Nhân viên bán hoa', NULL,
 @UserId1, '2023-01-10', NULL),

(N'Trần Thanh Tùng', '0923456789', 'staff2@example.com', N'Đà Nẵng',
 55000, 7.5, N'Nhân viên bán phụ kiện', NULL,
 @UserId2, '2022-11-05', NULL),

(N'Phạm Thị Bích', '0934567890', 'bich@gmail.com', N'TP. HCM',
 70000, 8.0, N'Quản lý cửa hàng', NULL,
 NULL, '2021-06-01', NULL),

(N'Hoàng Minh Tuấn', '0945678901', 'tuan@gmail.com', N'Hải Phòng',
 60000, 7.0, N'Nhân viên kho', NULL,
 NULL, '2022-09-15', NULL);



-- Bảng Customers
CREATE TABLE Customers (
    id INT IDENTITY(1,1) PRIMARY KEY, 
	user_id INT  NULL,
    customer_code VARCHAR(10)  NULL, 
    full_name NVARCHAR(100) NOT NULL, 
    phone VARCHAR(15) NOT NULL, 
    email VARCHAR(100) NULL, 
    address NVARCHAR(255) NULL,
    note NVARCHAR(255) NULL,
	FOREIGN KEY (user_id) REFERENCES Users(id) ON DELETE SET NULL
);

-- Tự động tạo customer_code
CREATE TRIGGER trg_AutoGenerateCustomerCode
ON Customers
AFTER INSERT
AS
BEGIN
    UPDATE Customers
    SET customer_code = 'CUS-' + RIGHT('000' + CAST(id AS VARCHAR(3)), 3)
    WHERE customer_code IS NULL;
END;

-- Liên kết user_id với khách hàng
DECLARE @UserId3 INT = (SELECT id FROM Users WHERE email = 'khach1@example.com');

-- Chèn dữ liệu vào bảng Customers
INSERT INTO Customers (full_name, phone, email, address, note, user_id) VALUES
(N'Nguyễn Văn Khách', '0987654321', 'khach1@example.com', N'Hà Nội', N'Khách hàng mới', @UserId3),
(N'Trần Thị Hạnh', '0945678901', 'hanh@gmail.com', N'Hà Nội', NULL, NULL),
(N'Nguyễn Quang Duy', '0956789012', 'duy@gmail.com', N'TP. HCM', NULL, NULL),
(N'Lê Hoàng Hải', '0967890123', 'hai@gmail.com', N'Đà Nẵng', NULL, NULL),
(N'Phạm Thị Ngọc', '0978901234', 'ngoc@gmail.com', N'Cần Thơ', NULL, NULL);


-- Bảng Categories
CREATE TABLE Categories (
    id INT IDENTITY(1,1) PRIMARY KEY,
    category_code VARCHAR(10)  NULL,
    category_name NVARCHAR(100) NOT NULL,
    note NVARCHAR(255) NULL
);

-- Tự động tạo category_code
CREATE TRIGGER trg_AutoGenerateCategoryCode
ON Categories
AFTER INSERT
AS
BEGIN
    UPDATE Categories
    SET category_code = 'CAT-' + RIGHT('000' + CAST(id AS VARCHAR(3)), 3)
    WHERE category_code IS NULL;
END;

-- Chèn dữ liệu vào bảng Categories (Hoa và Phụ kiện)
INSERT INTO Categories (category_name, note) VALUES
(N'Hoa Tươi', N'Các loại hoa tươi nhập khẩu'),
(N'Bó Hoa', N'Bó hoa tươi dành cho các dịp đặc biệt'),
(N'Lẵng Hoa', N'Lẵng hoa trang trí sự kiện'),
(N'Phụ Kiện', N'Phụ kiện trang trí cho hoa');

CREATE TABLE Suppliers (
    id INT IDENTITY(1,1) PRIMARY KEY,
    supplier_code VARCHAR(10)  NULL,
    supplier_name NVARCHAR(255) NOT NULL,
    phone VARCHAR(15) UNIQUE NOT NULL,
    email VARCHAR(100) UNIQUE NULL,
    address NVARCHAR(255) NULL,
    note NVARCHAR(255) NULL
);

-- Tự động tạo mã nhà cung cấp
CREATE TRIGGER trg_AutoGenerateSupplierCode
ON Suppliers
AFTER INSERT
AS
BEGIN
    UPDATE Suppliers
    SET supplier_code = 'SUP-' + RIGHT('000' + CAST(id AS VARCHAR(3)), 3)
     WHERE supplier_code IS NULL OR supplier_code = '';
END;

INSERT INTO Suppliers (supplier_name, phone, email, address, note) VALUES
(N'Công ty Hoa Tươi ABC', '0901122334', 'contact@hoatuoiabc.com', N'Hà Nội', N'Chuyên cung cấp hoa tươi nhập khẩu'),
(N'Công ty Hoa & Phụ Kiện XYZ', '0912233445', 'sales@hoaxyz.com', N'TP. HCM', N'Nhà cung cấp hoa và phụ kiện uy tín'),
(N'Công ty Phụ Kiện Hoa Sen', '0923344556', 'support@hoasen.com', N'Đà Nẵng', N'Cung cấp giấy gói và giỏ hoa'),
(N'Cửa hàng Hoa Xinh', '0934455667', 'info@hoaxinh.vn', N'Hải Phòng', N'Chuyên bán sỉ hoa trang trí');

CREATE TABLE Products (
    id VARCHAR(10) PRIMARY KEY,
    product_name NVARCHAR(255) NOT NULL,
    category_id INT NOT NULL,
    cost_price DECIMAL(10,2) NOT NULL DEFAULT 0,
    price DECIMAL(10,2) NOT NULL,
    stock_quantity INT NOT NULL,
	unit NVARCHAR(50) NULL;  -- Đơn vị tính như "bó", "lẵng", "cái", "hộp", "kg",...
    supplier_id INT NOT NULL,
    image_path NVARCHAR(255) NULL,
    note NVARCHAR(MAX) NULL,
    discount DECIMAL(5,2) DEFAULT 0,
    color NVARCHAR(100) NULL,
    import_date DATE NULL, -- 🆕 Thêm cột ngày nhập hàng

    FOREIGN KEY (category_id) REFERENCES Categories(id),
    FOREIGN KEY (supplier_id) REFERENCES Suppliers(id)
);

--ALTER TABLE Products
--ADD unit NVARCHAR(50) NULL;  -- Đơn vị tính như "bó", "lẵng", "cái", "hộp", "kg",...

-- Cập nhật dữ liệu đã có thêm đơn vị tính
UPDATE Products SET unit = N'Bó' WHERE id = 'P001';
UPDATE Products SET unit = N'Lẵng' WHERE id = 'P002';
UPDATE Products SET unit = N'Giỏ' WHERE id = 'P003';
UPDATE Products SET unit = N'Tấm' WHERE id = 'P004';



-- Inserting data into the Products table including the new cost_price column
INSERT INTO Products (id, product_name, category_id, cost_price, price, stock_quantity, supplier_id, image_path, note, discount, color) 
VALUES
('P001', N'Bó hoa hồng đỏ', 2, 150000, 250000, 20, 1, '/images/sanpham1.jpg', N'Bó hoa hồng nhập khẩu từ Hà Lan', 10, N'Đỏ'),
('P002', N'Lẵng hoa lan hồ điệp', 3, 500000, 750000, 15, 2, '/images/sanpham2.jpg', N'Lan hồ điệp cao cấp từ Đà Lạt', 5, N'Trắng, Tím'),
('P003', N'Giỏ hoa hướng dương', 2, 200000, 300000, 10, 3, '/images/sanpham3.jpg', N'Hoa hướng dương tươi sáng, phù hợp tặng sinh nhật', 15, N'Vàng'),
('P004', N'Giấy gói hoa cao cấp', 4, 30000, 50000, 100, 3, '/images/sanpham4.jpg', N'Giấy gói hoa nhập khẩu từ Nhật Bản', 0, NULL);

UPDATE Products
SET import_date = '2025-04-01'
WHERE id = 'P001';

UPDATE Products
SET import_date = '2025-04-03'
WHERE id = 'P002';

UPDATE Products
SET import_date = '2025-04-05'
WHERE id = 'P003';

UPDATE Products
SET import_date = '2025-04-07'
WHERE id = 'P004';


-- Bảng Orders
CREATE TABLE Orders (
    id INT IDENTITY(1,1) PRIMARY KEY,
    order_code VARCHAR(30)  NULL,
    customer_id INT NOT NULL,
    employee_id INT  NULL,
    order_date DATETIME DEFAULT GETDATE(),
    total_amount DECIMAL(10,2) NOT NULL DEFAULT 0,
    status NVARCHAR(50) CHECK (status IN (N'Đang xử lý', N'Hoàn thành', N'Đang giao', N'Đã hủy')) NOT NULL DEFAULT N'Đang xử lý',
    payment NVARCHAR(50) CHECK (payment IN (N'Chưa thanh toán', N'Đã thanh toán')) NOT NULL DEFAULT N'Chưa thanh toán',
    note NVARCHAR(255) NULL,
	shipping_address NVARCHAR(255) NULL,
    FOREIGN KEY (customer_id) REFERENCES Customers(id),
    FOREIGN KEY (employee_id) REFERENCES Employees(id)
);


CREATE TRIGGER trg_AutoGenerateOrderCode
ON Orders
AFTER INSERT
AS
BEGIN
    UPDATE Orders
    SET order_code = 'ORD-' + CONVERT(VARCHAR, GETDATE(), 112) + '-' + RIGHT('000' + CAST(id AS VARCHAR(3)), 3)
    WHERE order_code IS NULL OR order_code = '';
END;


INSERT INTO Orders (customer_id, employee_id, total_amount, status, payment, note, shipping_address)
VALUES
(1, 2, 500000, N'Hoàn thành', N'Đã thanh toán', N'Giao hàng đúng hẹn', N'123 Đường ABC, Hà Nội'),
(2, 3, 350000, N'Đang giao', N'Chưa thanh toán', N'Khách yêu cầu giao vào chiều', N'456 Đường XYZ, TP. HCM'),
(3, 4, 400000, N'Đang xử lý', N'Đã thanh toán', N'Gói quà sinh nhật', N'789 Đường DEF, TP. HCM'),
(4, 1, 600000, N'Đã hủy', N'Chưa thanh toán', N'Khách hủy đơn vì đổi ý', N'101 Đường GHI, Hà Nội');



CREATE TABLE OrdersDetails (
    id INT IDENTITY(1,1) PRIMARY KEY, 
    order_id INT NOT NULL, 
    product_id VARCHAR(10) NOT NULL, 
    quantity INT NOT NULL, 
    unit_price DECIMAL(10,2) NOT NULL, 
    total_price DECIMAL(10,2) NOT NULL, 
    note NVARCHAR(255) NULL,
    FOREIGN KEY (order_id) REFERENCES Orders(id) ON DELETE CASCADE,
    FOREIGN KEY (product_id) REFERENCES Products(id) ON DELETE CASCADE
);

INSERT INTO OrdersDetails (order_id, product_id, quantity, unit_price, total_price, note) 
VALUES 
(1, 'P001', 2, 250000, 2 * 250000, N'Bó hoa hồng đỏ tặng khách hàng VIP'),
(1, 'P003', 1, 180000, 1 * 180000, N'Bình cắm hoa thủy tinh cao cấp'),
(2, 'P002', 1, 750000, 1 * 750000, N'Lẵng hoa lan hồ điệp cho sự kiện'),
(2, 'P004', 2, 120000, 2 * 120000, N'Kéo cắt cành chuyên dụng cho tiệm hoa'),
(3, 'P001', 1, 250000, 1 * 250000, N'Bó hoa hồng đỏ tặng sinh nhật'),
(3, 'P003', 2, 180000, 2 * 180000, N'Bình cắm hoa cho trang trí tiệc'),
(4, 'P002', 1, 750000, 1 * 750000, N'Lẵng hoa lan hồ điệp sang trọng'),
(4, 'P004', 3, 120000, 3 * 120000, N'Kéo cắt cành cho cửa hàng hoa');


CREATE TABLE Carts (
    id INT IDENTITY(1,1) PRIMARY KEY,
    customer_id INT NULL,   -- NULL nếu khách chưa đăng nhập
    session_id VARCHAR(50) NULL,  -- ID session để theo dõi giỏ hàng ẩn danh
    created_at DATETIME DEFAULT GETDATE(),  
    updated_at DATETIME NULL,   
    status NVARCHAR(20) CHECK (status IN (N'Chờ xử lý', N'Đã đặt hàng', N'Bỏ giỏ hàng')) DEFAULT N'Chờ xử lý',
    note NVARCHAR(255) NULL,
    FOREIGN KEY (customer_id) REFERENCES Customers(id) ON DELETE CASCADE
);

CREATE TABLE CartItems (
    id INT IDENTITY(1,1) PRIMARY KEY,
    cart_id INT NOT NULL,
    product_id VARCHAR(10) NOT NULL,  -- Đổi thành VARCHAR(10) để khớp với Products.id
    quantity INT NOT NULL CHECK (quantity > 0),
    unit_price DECIMAL(10,2) NOT NULL,
    discount DECIMAL(10,2) DEFAULT 0,
    total_price AS ((quantity * unit_price) - discount) PERSISTED,
    FOREIGN KEY (cart_id) REFERENCES Carts(id) ON DELETE CASCADE,
    FOREIGN KEY (product_id) REFERENCES Products(id) ON DELETE CASCADE
);


-- Trigger cập nhật thời gian khi có thay đổi
CREATE TRIGGER trg_UpdateCartTimestamp
ON Carts
AFTER UPDATE
AS
BEGIN
    UPDATE Carts
    SET updated_at = GETDATE()
    WHERE id IN (SELECT id FROM inserted);
END;


CREATE TABLE Reports (
    id INT IDENTITY(1,1) PRIMARY KEY,                               -- Mã báo cáo tự tăng
	report_code NVARCHAR(50),
    report_month INT NOT NULL CHECK (report_month BETWEEN 1 AND 12),  -- Tháng báo cáo
    report_year INT NOT NULL CHECK (report_year >= 2000),            -- Năm báo cáo
    total_orders INT DEFAULT 0,                                      -- Tổng số đơn hàng
    total_products_sold INT DEFAULT 0,                               -- Tổng số lượng sản phẩm bán ra
    total_revenue DECIMAL(12,2) DEFAULT 0,                           -- Tổng doanh thu
    total_cost DECIMAL(12,2) DEFAULT 0,                              -- Tổng chi phí
    total_profit AS (total_revenue - total_cost) PERSISTED,          -- Lợi nhuận = doanh thu - chi phí
    note NVARCHAR(255) NULL,                                         -- Ghi chú báo cáo
    created_at DATETIME DEFAULT GETDATE()                            -- Ngày tạo báo cáo
);

CREATE TRIGGER trg_Generate_ReportCode
ON Reports
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH ReportCTE AS (
        SELECT 
            i.id,
            i.report_month,
            i.report_year,
            ROW_NUMBER() OVER (
                PARTITION BY i.report_year, i.report_month
                ORDER BY i.id
            ) AS rn
        FROM inserted i
    )
    UPDATE r
    SET r.report_code = 'RP' + 
                        RIGHT('0000' + CAST(c.report_year AS VARCHAR), 4) +
                        RIGHT('00' + CAST(c.report_month AS VARCHAR), 2) +
                        '-' +
                        RIGHT('000' + CAST(c.rn AS VARCHAR), 3)
    FROM Reports r
    INNER JOIN ReportCTE c ON r.id = c.id;
END;

CREATE PROCEDURE GenerateMonthlyReport
    @month INT,
    @year INT
AS
BEGIN
    DECLARE 
        @totalOrders INT,
        @totalProducts INT,
        @revenue DECIMAL(12,2),
        @cost DECIMAL(12,2);

    -- Đếm tổng số đơn hàng theo tháng/năm
    SELECT @totalOrders = COUNT(*)
    FROM Orders 
    WHERE MONTH(order_date) = @month AND YEAR(order_date) = @year;

    -- Tính tổng số sản phẩm bán ra và tổng doanh thu
    SELECT 
        @totalProducts = SUM(od.quantity),
        @revenue = SUM(od.total_price)
    FROM OrdersDetails od
    JOIN Orders o ON o.id = od.order_id
    WHERE MONTH(o.order_date) = @month AND YEAR(o.order_date) = @year;

    -- Tính tổng chi phí dựa trên giá vốn của sản phẩm
    SELECT 
        @cost = SUM(od.quantity * p.cost_price)
    FROM OrdersDetails od
    JOIN Orders o ON o.id = od.order_id
    JOIN Products p ON p.id = od.product_id
    WHERE MONTH(o.order_date) = @month AND YEAR(o.order_date) = @year;

    -- Thêm bản ghi báo cáo vào bảng Reports
    INSERT INTO Reports (
        report_month, report_year, 
        total_orders, total_products_sold, 
        total_revenue, total_cost
    )
    VALUES (
        @month, @year,
        ISNULL(@totalOrders, 0), ISNULL(@totalProducts, 0),
        ISNULL(@revenue, 0), ISNULL(@cost, 0)
    );
END;
-- báo cáo danh thu sản phẩm
CREATE PROCEDURE GenerateProductRevenueReport
    @month INT,
    @year INT
AS
BEGIN
    SELECT 
        p.product_name, 
        SUM(od.quantity) AS total_sold, 
        SUM(od.total_price) AS total_revenue
    FROM OrdersDetails od
    JOIN Orders o ON o.id = od.order_id
    JOIN Products p ON p.id = od.product_id
    WHERE MONTH(o.order_date) = @month AND YEAR(o.order_date) = @year
    GROUP BY p.product_name
    ORDER BY total_revenue DESC;
END;

-- báo cáo nhân viên bán hàng
CREATE PROCEDURE GenerateEmployeeSalesReport
    @month INT,
    @year INT
AS
BEGIN
    SELECT 
        e.full_name AS employee_name,
        SUM(od.total_price) AS total_sales
    FROM OrdersDetails od
    JOIN Orders o ON o.id = od.order_id
    JOIN Employees e ON e.id = o.employee_id
    WHERE MONTH(o.order_date) = @month AND YEAR(o.order_date) = @year
    GROUP BY e.full_name
    ORDER BY total_sales DESC;
END;

-- báo cáo khách hàng mua
CREATE PROCEDURE GenerateCustomerRevenueReport
    @month INT,
    @year INT
AS
BEGIN
    SELECT 
        c.full_name, 
        SUM(od.total_price) AS total_spent
    FROM OrdersDetails od
    JOIN Orders o ON o.id = od.order_id
    JOIN Customers c ON c.id = o.customer_id
    WHERE MONTH(o.order_date) = @month AND YEAR(o.order_date) = @year
    GROUP BY c.full_name
    ORDER BY total_spent DESC;
END;

-- báo cáo đơn hàng

CREATE PROCEDURE GenerateOrdersReport
    @month INT,
    @year INT
AS
BEGIN
    SELECT 
        o.order_code,
        o.order_date,
        c.full_name AS customer_name,
        e.full_name AS employee_name,
        SUM(od.quantity) AS total_quantity,
        SUM(od.total_price) AS total_amount
    FROM Orders o
    JOIN OrdersDetails od ON o.id = od.order_id
    JOIN Customers c ON c.id = o.customer_id
    JOIN Employees e ON e.id = o.employee_id
    WHERE MONTH(o.order_date) = @month AND YEAR(o.order_date) = @year
    GROUP BY 
        o.order_code, o.order_date, 
        c.full_name, e.full_name
    ORDER BY o.order_date ASC;
END;


EXEC GenerateMonthlyReport @month = 4, @year = 2025;
EXEC GenerateProductRevenueReport @month = 4, @year = 2025;

--DROP PROCEDURE GenerateMonthlyReport;
--DROP PROCEDURE GenerateProductRevenueReport;
--DROP PROCEDURE GenerateEmployeeSalesReport;
--DROP PROCEDURE GenerateCustomerRevenueReport;



