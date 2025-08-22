using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoAn.Controler;
using DoAn.Models;
using System.IO;
using DoAn.Properties; // Thêm thư viện này nếu chưa có

namespace DoAn.View.SP
{
    public partial class UC_update : UserControl
    {
        private string productID;
        private string imagePath = string.Empty;


        public UC_update(string id)
        {
            productID = id;  // Lưu ID sản phẩm để load dữ liệu
            InitializeComponent();
            LoadProductData();
        }

        private void LoadCategories(string currentCategoryID)
        {
            ProductDAL productDAL = new ProductDAL();
            DataTable dt = productDAL.GetAllCategories(); // Lấy tất cả danh mục sản phẩm

            if (dt.Rows.Count > 0)
            {
                cbo_danhmuc.DataSource = dt;
                cbo_danhmuc.DisplayMember = "category_name"; // Hiển thị tên danh mục
                cbo_danhmuc.ValueMember = "id"; // Giá trị thực là ID danh mục

                DataRow? foundRow = dt.AsEnumerable()
      .FirstOrDefault(row => row["category_name"].ToString() == currentCategoryID);

                if (foundRow != null)
                {
                    cbo_danhmuc.SelectedValue = foundRow["id"].ToString();
                }
                else
                {
                    // Xử lý khi không tìm thấy danh mục (ví dụ: đặt giá trị mặc định)
                    cbo_danhmuc.SelectedIndex = -1; // Không chọn gì cả
                }



                if (foundRow != null)
                {
                    cbo_danhmuc.SelectedValue = foundRow["id"].ToString();
                }
            }
        }


        private void LoadSuppliers(string currentSupplierID)
        {
            ProductBLL productBLL = new ProductBLL();
            DataTable dt = productBLL.GetAllSuppliers(); // Lấy tất cả nhà cung cấp

            if (dt.Rows.Count > 0)
            {
                cbo_ncc.DataSource = dt;
                cbo_ncc.DisplayMember = "supplier_name"; // Hiển thị tên nhà cung cấp
                cbo_ncc.ValueMember = "id"; // Giá trị thực của ComboBox là ID nhà cung cấp

                // Chọn nhà cung cấp hiện tại của sản phẩm
                // Tìm ID của nhà cung cấp từ tên
                DataRow? foundRow = dt.AsEnumerable()
     .FirstOrDefault(row => row["supplier_name"].ToString() == currentSupplierID);

                if (foundRow != null)
                {
                    cbo_ncc.SelectedValue = foundRow["id"].ToString();
                }
                else
                {
                    // Có thể hiển thị thông báo hoặc đặt giá trị mặc định nếu không tìm thấy nhà cung cấp
                }



                if (foundRow != null)
                {
                    cbo_ncc.SelectedValue = foundRow["id"].ToString();
                }

            }
        }






        private void LoadProductData()
        {
            pic_1.SizeMode = PictureBoxSizeMode.StretchImage;
            // Lấy thông tin sản phẩm từ CSDL
            ProductBLL productBLL = new ProductBLL();
            DataTable dt = productBLL.GetProductByID(productID);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                txt_maSP.Text = row["Mã Sản Phẩm"].ToString();
                txt_tenSP.Text = row["Tên Sản Phẩm"].ToString();
                txt_color.Text = row["Màu Sắc"].ToString(); // Màu sắc (có thể thêm vào sau)
                txt_cost.Text = row["Giá Nhập"].ToString(); // Giá nhập
                txt_gia.Text = row["Giá Bán"].ToString();
                txt_discount.Text = row["Giảm Giá"].ToString();
                txt_soluong.Text = row["Số Lượng"].ToString();
                txt_unit.Text = row["Đơn Vị"].ToString(); // Đơn vị tính (có thể thêm vào sau)
                cbo_danhmuc.Text = row["Danh Mục"].ToString();
                cbo_ncc.Text = row["Nhà Cung Cấp"].ToString();
                txt_note.Text = row["Ghi Chú"].ToString();
                txt_importDate.Text = row["Ngày Nhập"].ToString();

                // Lấy danh mục hiện tại từ dữ liệu sản phẩm
                string currentCategoryID = row["Danh Mục"].ToString();

                // Load danh sách danh mục và chọn danh mục hiện tại
                LoadCategories(currentCategoryID);


                // Lấy nhà cung cấp hiện tại từ dữ liệu sản phẩm
                string currentSupplierID = row["Nhà Cung Cấp"].ToString();

                // Load danh sách nhà cung cấp và chọn nhà cung cấp hiện tại
                LoadSuppliers(currentSupplierID);

                // Load ảnh sản phẩm

                // Lấy đường dẫn thư mục gốc của project (2 cấp cha của Application.StartupPath)
                // Lấy thư mục gốc của project (2 cấp cha của thư mục bin/Debug)
                string projectRoot = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;

                // Lấy đường dẫn ảnh từ database
                string imagePath = row["Hình Ảnh"]?.ToString()?.Trim();
                if (!string.IsNullOrEmpty(imagePath))
                {
                    // Xóa ký tự '/' hoặc '\' ở đầu nếu có và thay thế dấu '/' thành '\'
                    string cleanImagePath = imagePath.TrimStart('/', '\\').Replace("/", "\\");

                    // Kết hợp thành đường dẫn đầy đủ (Thư mục images nằm ở gốc project)
                    string fullPath = Path.Combine(projectRoot, cleanImagePath);

                    Console.WriteLine("Full path to image: " + fullPath);
                    MessageBox.Show("Full path to image: " + fullPath);

                    // Kiểm tra file có tồn tại không
                    if (File.Exists(fullPath))
                    {
                        pic_1.Image = Image.FromFile(fullPath);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy file ảnh: " + fullPath);
                        Console.WriteLine("Không tìm thấy file ảnh: " + fullPath);

                        // Load ảnh mặc định nếu ảnh không tồn tại
                        string backupImagePath = Path.Combine(projectRoot, "Resources", "logo.png");
                        pic_1.Image = File.Exists(backupImagePath) ? Image.FromFile(backupImagePath) : null;
                    }
                }
                else
                {
                    MessageBox.Show("Không có đường dẫn ảnh trong database.");
                }





            }
        }

        private void btn_chon_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                openFileDialog.Title = "Chọn ảnh sản phẩm";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    imagePath = openFileDialog.FileName; // Lưu đường dẫn ảnh mới
                    pic_1.Image = Image.FromFile(imagePath); // Hiển thị ảnh trong PictureBox
                }
            }
        }

        private void btn_luu_Click(object sender, EventArgs e)
        {
            try
            {
                string maSP = txt_maSP.Text;
                string tenSP = txt_tenSP.Text;
                string color = txt_color.Text; // Màu sắc (có thể thêm vào sau)
                decimal cost = decimal.Parse(txt_cost.Text); // Giá nhập
                decimal gia = decimal.Parse(txt_gia.Text);
                int soLuong = int.Parse(txt_soluong.Text);
                string unit = txt_unit.Text; // Đơn vị tính (có thể thêm vào sau)
                int danhMucID = (int)cbo_danhmuc.SelectedValue;
                int nhaCCID = (int)cbo_ncc.SelectedValue;
                string note = txt_note.Text;
                decimal discount = decimal.Parse(txt_discount.Text);  // Lấy giá trị giảm giá
                DateTime importDate;
                if (string.IsNullOrWhiteSpace(txt_importDate.Text))
                {
                    importDate = DateTime.Now; // Mặc định là thời gian hiện tại nếu không nhập
                }
                else if (!DateTime.TryParse(txt_importDate.Text, out importDate))
                {
                    MessageBox.Show("Vui lòng nhập ngày nhập hợp lệ (định dạng dd/MM/yyyy)!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_importDate.Focus();
                    return;
                }


                // Nếu chưa chọn ảnh, giữ nguyên ảnh cũ từ database
                if (string.IsNullOrEmpty(imagePath))
                {
                    ProductDAL productDAL = new ProductDAL();
                    DataTable dt = productDAL.GetProductByID(maSP);
                    if (dt.Rows.Count > 0)
                    {
                        imagePath = dt.Rows[0]["Hình Ảnh"].ToString();
                    }
                }

                // Gọi qua BLL để cập nhật sản phẩm
                ProductBLL productBLL = new ProductBLL();
                bool success = productBLL.UpdateProduct(maSP, tenSP,color, cost, gia, discount, soLuong, unit,danhMucID, nhaCCID, imagePath, note,importDate);

                if (success)
                {
                    MessageBox.Show("Cập nhật sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.ParentForm?.Close();
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
