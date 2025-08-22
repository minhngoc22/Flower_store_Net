using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoAn.Models;
using DoAn.Controler;
using Microsoft.VisualBasic; // dùng để gọi InputBox
using System.Globalization;



namespace DoAn.View.SP
{
    public partial class UC_themSP : UserControl
    {
        private string imagePath = string.Empty; // Khai báo biến toàn cục để lưu đường dẫn ảnh

        public UC_themSP()
        {
            InitializeComponent();
            LoadCategories();
            LoadSuppliers();
        }

        private void LoadCategories()
        {
            ProductDAL productDAL = new ProductDAL();
            DataTable dt = productDAL.GetAllCategories();



            if (dt.Rows.Count > 0)
            {
                cbo_danhmuc.DataSource = dt;
                cbo_danhmuc.DisplayMember = "category_name";
                cbo_danhmuc.ValueMember = "id";
                cbo_danhmuc.SelectedIndex = 0; // Mặc định chọn "Tất cả"
            }
        }

        private void LoadSuppliers()
        {
            ProductBLL productBLL = new ProductBLL();
            DataTable dt = productBLL.GetAllSuppliers();

            cbo_ncc.DataSource = dt;
            cbo_ncc.DisplayMember = "supplier_name"; // Hiển thị tên nhà cung cấp
            cbo_ncc.ValueMember = "id"; // Giá trị thực của ComboBox là ID nhà cung cấp
        }

        private void btn_them_Click(object sender, EventArgs e)
        {
            pic_2.SizeMode = PictureBoxSizeMode.StretchImage;
            try
            {
                // Kiểm tra ô nhập liệu
                if (string.IsNullOrWhiteSpace(txt_maSP.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã sản phẩm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_maSP.Focus();
                    return;
                }

              if (string.IsNullOrWhiteSpace(txt_unit.Text))
                {
                    MessageBox.Show("Vui lòng nhập đơn vị tính!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_unit.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txt_tenSP.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên sản phẩm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_tenSP.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txt_cost.Text))  // Kiểm tra giá nhập (cost price)
                {
                    MessageBox.Show("Vui lòng nhập giá nhập của sản phẩm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_cost.Focus();
                    return;
                }
                if (!decimal.TryParse(txt_cost.Text, out decimal costPrice) || costPrice <= 0)
                {
                    MessageBox.Show("Vui lòng nhập giá nhập hợp lệ (>= 0)!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_cost.Focus();
                    return;
                }

                if (!decimal.TryParse(txt_gia.Text, out decimal gia) || gia <= 0)
                {
                    MessageBox.Show("Vui lòng nhập giá bán của sản phẩm hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_gia.Focus();
                    return;
                }

                if (!int.TryParse(txt_soluong.Text, out int soLuong) || soLuong < 0)
                {
                    MessageBox.Show("Vui lòng nhập số lượng sản phẩm hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_soluong.Focus();
                    return;
                }

                if (cbo_danhmuc.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn danh mục sản phẩm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbo_danhmuc.Focus();
                    return;
                }

                if (cbo_ncc.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn nhà cung cấp!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbo_ncc.Focus();
                    return;
                }


                DateTime importDate;
                if (string.IsNullOrWhiteSpace(txt_importDate.Text))
                {
                    importDate = DateTime.Now; // Mặc định là thời gian hiện tại nếu không nhập
                }
                else if (!DateTime.TryParseExact(txt_importDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out importDate))
                {
                    MessageBox.Show("Vui lòng nhập ngày nhập hợp lệ theo định dạng dd/MM/yyyy!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_importDate.Focus();
                    return;
                }


                // Lấy giá trị hợp lệ từ các trường
                string maSP = txt_maSP.Text.Trim();
                string unit = txt_unit.Text.Trim(); // Đơn vị tính
                string tenSP = txt_tenSP.Text.Trim();
                string color = txt_color.Text.Trim(); // Màu sắc (có thể thêm vào sau)
                int danhMucID = Convert.ToInt32(cbo_danhmuc.SelectedValue);
                int nhaCCID = Convert.ToInt32(cbo_ncc.SelectedValue);
                string note = txt_note.Text.Trim();

                // Lấy đường dẫn thư mục dự án (chỉ dùng nếu `projectRoot` chưa có)
                string projectRoot = AppDomain.CurrentDomain.BaseDirectory;
                string backupImagePath = Path.Combine(projectRoot, "Resources", "logo2.png");

                // Xử lý đường dẫn ảnh (dùng ảnh mặc định nếu chưa chọn)
                string finalImagePath = string.IsNullOrEmpty(imagePath) ? backupImagePath : imagePath;

                // Gọi BLL để thêm sản phẩm
                ProductBLL productBLL = new ProductBLL();
                bool success = productBLL.InsertProduct(maSP, tenSP, color, costPrice, gia, soLuong, unit,danhMucID, nhaCCID, finalImagePath, note, importDate);

                if (success)
                {
                    MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetFields(); // Xóa nội dung sau khi thêm thành công
                }
                else
                {
                    MessageBox.Show("Thêm sản phẩm thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    imagePath = openFileDialog.FileName; // Đường dẫn gốc ảnh

                    try
                    {
                        // Cách an toàn để hiển thị ảnh
                        Image imgTemp = Image.FromFile(imagePath);
                        pic_2.Image = new Bitmap(imgTemp);
                        imgTemp.Dispose();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không thể mở ảnh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        private void ResetFields()
        {
            txt_maSP.Clear();
            txt_tenSP.Clear();
            txt_gia.Clear();
            txt_soluong.Clear();
            txt_cost.Clear();
            txt_note.Clear();
            cbo_danhmuc.SelectedIndex = 0;
            cbo_ncc.SelectedIndex = 0;
            pic_2.Image = null;
            imagePath = string.Empty;
            txt_color.Clear();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string newCategoryName = Microsoft.VisualBasic.Interaction.InputBox("Nhập tên danh mục mới:", "Thêm danh mục", "");

            if (!string.IsNullOrWhiteSpace(newCategoryName))
            {
                string note = "";

                CategoryBLL categoryBLL = new CategoryBLL();
                bool success = categoryBLL.AddCategory(newCategoryName, note);

                if (success)
                {
                    MessageBox.Show("Thêm danh mục mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCategories();
                    cbo_danhmuc.SelectedIndex = cbo_danhmuc.Items.Count - 1;
                }
                else
                {
                    MessageBox.Show("Thêm danh mục thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
