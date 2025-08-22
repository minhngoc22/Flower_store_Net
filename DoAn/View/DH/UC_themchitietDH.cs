using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoAn.Models;
using DoAn.BLL;
using DoAn.Controler;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Diagnostics;
using ClosedXML.Excel;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom; // Thêm dòng này để nhận diện PageSize

using iText.IO.Font;
using DocumentFormat.OpenXml.Bibliography;


namespace DoAn.View.DH
{
    public partial class UC_themchitietDH : UserControl
    {
        private string orderCode;
        private string customerCode;
        private OrderDetailsBLL orderDetailsBLL; // Khai báo biến toàn c
        private OrderBLL orderBLL = new OrderBLL(); // Khai báo biến toàn cục cho OrderBLL  
        public UC_themchitietDH(string orderCode, string customerCode)
        {
            InitializeComponent();
            this.orderCode = orderCode;
            this.customerCode = customerCode;

            // Khởi tạo đối tượng BLL trước khi sử dụng
            orderDetailsBLL = new OrderDetailsBLL();
            LoadData(); // Gọi hàm để hiển thị dữ liệu lên giao diện
            decimal totalPrice = orderDetailsBLL.GetTotalPriceByOrderCode(orderCode);
            txt_tongtien.Text = totalPrice.ToString("N0"); // Hiển thị số có dấu phân cách

            orderDetailsBLL = new OrderDetailsBLL();

            LoadEmployeeList();
            LoadComboBoxData(); // Load dữ liệu vào ComboBox


        }

        private void LoadComboBoxData()
        {
            OrderBLL orderBLL = new OrderBLL();



            // Lấy thông tin đơn hàng theo order_code
            DataTable dtOrder = orderBLL.GetOrderByCode(orderCode);

            if (dtOrder.Rows.Count > 0)
            {
                DataRow row = dtOrder.Rows[0]; // Lấy dòng dữ liệu đầu tiên (vì order_code là duy nhất)

                // Load trạng thái đơn hàng
                DataTable dtStatus = orderBLL.GetOrderStatusList();
                cbo_trangthai.DataSource = dtStatus;
                cbo_trangthai.DisplayMember = "status";
                cbo_trangthai.ValueMember = "status";
                cbo_trangthai.SelectedValue = row["Trạng Thái"].ToString(); // Chọn trạng thái của đơn hàng


                // Load danh sách phương thức thanh toán
                DataTable dtPayments = orderBLL.GetPaymentMethods();
                cbo_thanhtoan.DataSource = dtPayments;
                cbo_thanhtoan.DisplayMember = "payment";
                cbo_thanhtoan.ValueMember = "payment";
                cbo_thanhtoan.SelectedValue = row["Thanh Toán"].ToString(); // Chọn phương thức thanh toán của đơn hàng

                txt_diachi.Text = row["Địa Chỉ"].ToString(); // Địa chỉ giao hàng
            }
            else
            {
                MessageBox.Show("Không tìm thấy đơn hàng với mã: " + orderCode, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void LoadEmployeeList()
        {
            OrderBLL orderBLL = new OrderBLL();
            DataTable dtEmployees = orderBLL.GetEmployeeList();

            if (dtEmployees.Rows.Count > 0)
            {
                cbo_nv.DataSource = dtEmployees; // Gán dữ liệu cho ComboBox
                cbo_nv.DisplayMember = "full_name"; // Hiển thị tên nhân viên
                cbo_nv.ValueMember = "id"; // Giá trị khi chọn sẽ là ID nhân viên
                cbo_nv.SelectedIndex = 0; // Không chọn nhân viên nào mặc định
            }
        }
        private void LoadData()
        {
            // Hiển thị mã đơn hàng và mã khách hàng lên các label hoặc textbox
            txt_maDH.Text = orderCode;
            txt_maKH.Text = customerCode;

            txt_maDH.Enabled = false;
            txt_maKH.Enabled = false;

            DataTable dt = orderDetailsBLL.GetAll(orderCode);
            dvg_ctdh.DataSource = dt;

            // Lấy dữ liệu từ BLL
            DataTable orderStatusTable = orderBLL.GetOrderStatus(orderCode);

            // Kiểm tra xem có dữ liệu không
            if (orderStatusTable.Rows.Count > 0)
            {
                string orderStatus = orderStatusTable.Rows[0]["status"].ToString(); // Lấy giá trị cột "status"

                if (orderStatus == "Đang giao" || orderStatus == "Đã hủy" || orderStatus == "Hoàn thành")
                    DisableEditing();
            }
        }



        private void UC_chitietDH_Load(object sender, EventArgs e)
        {
            LoadData();
            // Lấy tổng tiền từ BLL và hiển thị

        }
        private void DisableEditing()
        {
            txt_maSP.Enabled = false;
            txt_discount.Enabled = false;
            txt_maKH.Enabled = false;
            txt_maDH.Enabled = false;
            txt_soluong.Enabled = false;
            txt_dongia.Enabled = false;
            txt_note.Enabled = false;

            btn_them.Enabled = false;
            btn_xuat.Enabled = false;
        }



        private void btn_them_Click(object sender, EventArgs e)
        {
            // Kiểm tra đầu vào có hợp lệ không
            if (string.IsNullOrEmpty(txt_maSP.Text) || string.IsNullOrEmpty(txt_soluong.Text) || string.IsNullOrEmpty(txt_dongia.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy dữ liệu từ giao diện
            string productCode = txt_maSP.Text.Trim();
            int quantity;
            decimal unitPrice;
            string note = txt_note.Text.Trim();
            string diachi = txt_diachi.Text.Trim();

            // Kiểm tra số lượng và đơn giá có hợp lệ không
            if (!int.TryParse(txt_soluong.Text.Trim(), out quantity) || quantity <= 0)
            {
                MessageBox.Show("Số lượng phải là số nguyên dương!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(txt_dongia.Text.Trim(), out unitPrice) || unitPrice <= 0)
            {
                MessageBox.Show("Đơn giá phải là số dương!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Gọi BLL để thêm chi tiết đơn hàng
            bool result = orderDetailsBLL.AddOrderDetail(orderCode, productCode, quantity, unitPrice, note);

            if (result)
            {
                MessageBox.Show("Thêm chi tiết đơn hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData(); // Cập nhật lại danh sách chi tiết đơn hàng
                ClearFields(); // Xóa dữ liệu nhập sau khi thêm thành công

                // Cập nhật tổng tiền
                decimal totalPrice = orderDetailsBLL.GetTotalPriceByOrderCode(orderCode);
                txt_tongtien.Text = totalPrice.ToString("N0"); // Hiển thị tổng tiền có dấu phân cách
            }
            else
            {
                MessageBox.Show("Không thể thêm chi tiết đơn hàng! Đơn hàng có thể đã bị hủy hoặc đang giao.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            try
            {
                // Lấy dữ liệu từ form
                string status = cbo_trangthai.SelectedValue?.ToString();
                string employeeId = cbo_nv.SelectedValue?.ToString();
                string paymentMethod = cbo_thanhtoan.SelectedValue?.ToString();

                // Chuyển đổi giá trị từ TextBox sang decimal
                decimal totalAmount;
                bool isValidAmount = decimal.TryParse(txt_tongtien.Text, out totalAmount);

                if (!isValidAmount)
                {
                    // Xử lý nếu giá trị không hợp lệ (ví dụ: thông báo lỗi cho người dùng)
                    totalAmount = 0; // Hoặc thực hiện hành động khác tùy theo yêu cầu của bạn
                }
        

                if (string.IsNullOrEmpty(orderCode))
                {
                    MessageBox.Show("Mã đơn hàng không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Gọi BLL để cập nhật đơn hàng
                OrderBLL orderBLL = new OrderBLL();
                bool isUpdated = orderBLL.UpdateOrder(orderCode, totalAmount, status, employeeId, paymentMethod, note, diachi);

                if (isUpdated)
                {
                    MessageBox.Show("Cập nhật đơn hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        public void ClearFields()
        {

            txt_maSP.Text = "";
            txt_soluong.Text = "";
            txt_dongia.Text = "";
            txt_note.Text = "";
        }


        private void txt_maSP_TextChanged(object sender, EventArgs e)
        {
            txt_maSP.Text = txt_maSP.Text.Trim().ToUpper();
            txt_maSP.SelectionStart = txt_maSP.Text.Length;  // Đặt con trỏ vào cuối TextBox
            // Kiểm tra nếu có giá trị nhập vào
            if (!string.IsNullOrEmpty(txt_maSP.Text))
            {
                ProductBLL productBLL = new ProductBLL();

                decimal price = orderDetailsBLL.GetProductPrice(txt_maSP.Text);
                txt_dongia.Text = price.ToString("N0"); // Hiển thị đơn giá với định dạng số

                decimal discount = orderDetailsBLL.GetProductDiscount(txt_maSP.Text);
                txt_discount.Text = discount.ToString("N0"); // Hiển thị giảm giá với định dạng số
            }
            else
            {
                txt_dongia.Text = "";
                txt_discount.Text = "";
            }
        }

        private void btn_xuat_Click(object sender, EventArgs e)
        {

            // Lấy thông tin đơn hàng từ các TextBox
            string orderCode = txt_maDH.Text;
            string customerCode = txt_maKH.Text;
            string totalPrice = txt_tongtien.Text;
            string orderDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");


            // Hộp thoại chọn nơi lưu file và định dạng
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "HTML Files (*.html)|*.html|Excel Files (*.xlsx)|*.xlsx|PDF Files (*.pdf)|*.pdf";
            saveFileDialog.Title = "Lưu hóa đơn";
            saveFileDialog.FileName = $"HoaDon_{orderCode}";


            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                string extension = System.IO.Path.GetExtension(filePath).ToLower();  // Sử dụng System.IO.Path
                try
                {
                    if (extension == ".html")
                    {
                        ExportToHtml(filePath, orderCode, customerCode, totalPrice, orderDate);
                        MessageBox.Show("Xuất hóa đơn HTML thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (extension == ".xlsx")
                    {
                        ExportToExcel(filePath);
                        MessageBox.Show("Xuất hóa đơn Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (extension == ".pdf")
                    {
                        ExportToPdf(filePath, orderCode, customerCode, totalPrice, orderDate);
                        MessageBox.Show("Xuất hóa đơn PDF thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Định dạng tệp không được hỗ trợ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Mở file sau khi lưu
                    Process.Start("explorer.exe", filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xuất hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ExportToHtml(string filePath, string orderCode, string customerCode, string totalPrice, string orderDate)
        {
            StringBuilder html = new StringBuilder();
            string nguoiLapDon = orderDetailsBLL.GetEmployeeNameByOrderCodestring(orderCode);


            // Phần đầu HTML và CSS cho bảng và viền trang
            html.AppendLine("<html><head><meta charset='UTF-8'><style>");
            html.AppendLine("@page { size: A4; margin: 20mm; }");
            html.AppendLine("body { font-family: 'Times New Roman', Times, serif; background-color: #eee; }");
            html.AppendLine(".wrapper { border: 1px solid black; padding: 20mm; width: 210mm; min-height: 297mm; margin: 0 auto; background-color: white; box-sizing: border-box; }");
            html.AppendLine("h2 { text-align: center;font-size: 20px; }");
            html.AppendLine("table { width: 100%; border-collapse: collapse; margin-top: 20px; }");
            html.AppendLine("th, td { border: 1px solid black; padding: 8px; text-align: center; }");
            html.AppendLine("th { background-color: #f2f2f2; }");
            html.AppendLine("</style></head><body>");

            // Bắt đầu wrapper có viền
            // Bắt đầu wrapper có viền
            html.AppendLine("<div class='wrapper'>");

            // Thêm tên cửa hàng
            // Tên cửa hàng ở góc phải
            html.AppendLine("<div style='text-align:left; font-size: 10px;'>");
            html.AppendLine("<strong>CỬA HÀNG HOA & PHỤ KIỆN Fleur de Vie</strong><br>");
            html.AppendLine("Địa chỉ: 20b Đ. Phó Cơ Điều, Phường 3, Vĩnh Long<br>");
            html.AppendLine("Điện thoại: 0123 456 789");
            html.AppendLine("</div>");



            // Tiêu đề hóa đơn
            html.AppendLine("<h2>HÓA ĐƠN BÁN HÀNG</h2>");
            html.AppendLine($"<p><strong>Mã đơn hàng:</strong> {orderCode}</p>");
            html.AppendLine($"<p><strong>Mã khách hàng:</strong> {customerCode}</p>");
            html.AppendLine($"<p><strong>Ngày đặt hàng:</strong> {orderDate}</p>");
            html.AppendLine("<hr>");


            // Bảng chi tiết
            html.AppendLine("<table>");
            html.AppendLine("<tr>");
            foreach (DataGridViewColumn col in dvg_ctdh.Columns)
            {
                html.AppendLine($"<th>{col.HeaderText}</th>");
            }
            html.AppendLine("</tr>");

            foreach (DataGridViewRow row in dvg_ctdh.Rows)
            {
                if (!row.IsNewRow)
                {
                    html.AppendLine("<tr>");
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        html.AppendLine($"<td>{cell.Value}</td>");
                    }
                    html.AppendLine("</tr>");
                }
            }
            html.AppendLine("</table>");

            // Tổng tiền
            html.AppendLine($"<h3 style='text-align:right;'>Tổng tiền: {totalPrice} VND</h3>");


            // Thêm phần ký tên người lập đơn
            html.AppendLine("<div style='margin-top: 50px; text-align: right;'>");
            html.AppendLine($"<p><strong>Người lập đơn:</p>");
            html.AppendLine($"<p>{nguoiLapDon}</p>");

            // Kết thúc wrapper
            html.AppendLine("</div>");
            html.AppendLine("</body></html>");

            // Ghi ra file HTML
            File.WriteAllText(filePath, html.ToString(), Encoding.UTF8);
        }

        //xuất excel
        private void ExportToExcel(string filePath)
        {


            using (var workbook = new XLWorkbook()) // Tạo workbook mới
            {
                var worksheet = workbook.Worksheets.Add("HoaDon"); // Thêm worksheet mới tên "HoaDon"
                int currentRow = 1; // Biến đếm dòng hiện tại

                // === Tiêu đề hóa đơn ===
                worksheet.Cell(currentRow, 1).Value = "HÓA ĐƠN BÁN HÀNG"; // Ghi tiêu đề
                worksheet.Cell(currentRow, 1).Style.Font.FontName = "Times New Roman"; // Font chữ Times New Roman
                worksheet.Cell(currentRow, 1).Style.Font.Bold = true; // In đậm
                worksheet.Cell(currentRow, 1).Style.Font.FontSize = 16; // Cỡ chữ lớn
                worksheet.Cell(currentRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center; // Căn giữa ngang
                worksheet.Range(currentRow, 1, currentRow, dvg_ctdh.Columns.Count).Merge(); // Gộp các ô theo chiều ngang
                currentRow += 2; // Xuống dòng

                // === Thông tin đơn hàng ===
                worksheet.Cell(currentRow++, 1).Value = $"Mã đơn hàng: {txt_maDH.Text}";
                worksheet.Cell(currentRow++, 1).Value = $"Mã khách hàng: {txt_maKH.Text}";
                worksheet.Cell(currentRow++, 1).Value = $"Ngày đặt hàng: {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}";
                currentRow++; // Thêm dòng trống

                // === Tiêu đề cột bảng chi tiết đơn hàng ===
                for (int i = 0; i < dvg_ctdh.Columns.Count; i++)
                {
                    worksheet.Cell(currentRow, i + 1).Value = dvg_ctdh.Columns[i].HeaderText; // Ghi tiêu đề cột
                    worksheet.Cell(currentRow, i + 1).Style.Font.Bold = true; // In đậm
                    worksheet.Cell(currentRow, i + 1).Style.Font.FontName = "Times New Roman"; // Font chữ Times New Roman
                    worksheet.Cell(currentRow, i + 1).Style.Font.FontSize = 14; // Cỡ chữ
                    worksheet.Cell(currentRow, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray; // Màu nền xám nhạt
                    worksheet.Cell(currentRow, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center; // Căn giữa
                }

                // === Ghi dữ liệu các dòng trong bảng ===
                for (int i = 0; i < dvg_ctdh.Rows.Count; i++)
                {
                    var row = dvg_ctdh.Rows[i];
                    if (!row.IsNewRow) // Bỏ qua dòng trống cuối cùng
                    {
                        for (int j = 0; j < dvg_ctdh.Columns.Count; j++)
                        {
                            var value = row.Cells[j].Value?.ToString();
                            worksheet.Cell(currentRow + 1 + i, j + 1).Value = value; // Ghi giá trị vào từng ô
                            worksheet.Cell(currentRow + 1 + i, j + 1).Style.Font.FontName = "Times New Roman"; // Áp dụng font Times New Roman
                            worksheet.Cell(currentRow + 1 + i, j + 1).Style.Font.FontSize = 12; // Cỡ chữ
                        }
                    }
                }

                currentRow += dvg_ctdh.Rows.Count + 1; // Cập nhật dòng hiện tại để xuống dòng ghi tổng tiền

                // === Ghi tổng tiền ===
                worksheet.Cell(currentRow, dvg_ctdh.Columns.Count - 1).Value = "Tổng tiền:";
                worksheet.Cell(currentRow, dvg_ctdh.Columns.Count).Value = txt_tongtien.Text;
                worksheet.Cell(currentRow, dvg_ctdh.Columns.Count - 1).Style.Font.FontName = "Times New Roman"; // Font chữ Times New Roman
                worksheet.Cell(currentRow, dvg_ctdh.Columns.Count).Style.Font.FontSize = 14; // Cỡ chữ
                worksheet.Cell(currentRow, dvg_ctdh.Columns.Count - 1).Style.Font.Bold = true;
                worksheet.Cell(currentRow, dvg_ctdh.Columns.Count).Style.Font.Bold = true;

                // === Căn chỉnh và tự động điều chỉnh độ rộng cột ===
                worksheet.Columns().AdjustToContents();

                // === Lưu file Excel ===
                workbook.SaveAs(filePath);
            }
        }


        private void ExportToPdf(string filePath, string orderCode, string customerCode, string totalPrice, string orderDate)
        {
            string createdBy = orderDetailsBLL.GetEmployeeNameByOrderCodestring(orderCode);

            using (PdfWriter writer = new PdfWriter(filePath))
            {
                using (PdfDocument pdf = new PdfDocument(writer))
                {
                    Document document = new Document(pdf, PageSize.A4);
                    document.SetMargins(20, 20, 20, 20);

                    // Font tiếng Việt
                    PdfFont fontTitle = PdfFontFactory.CreateFont("C:\\Windows\\Fonts\\timesbd.ttf", PdfEncodings.IDENTITY_H);
                    PdfFont fontNormal = PdfFontFactory.CreateFont("C:\\Windows\\Fonts\\times.ttf", PdfEncodings.IDENTITY_H);
                    PdfFont fontItalic = PdfFontFactory.CreateFont("C:\\Windows\\Fonts\\timesi.ttf", PdfEncodings.IDENTITY_H);

                    // ==== Thông tin cửa hàng ====
                    document.Add(new Paragraph("CỬA HÀNG HOA & PHỤ KIỆN Fleur de Vie")
                        .SetFont(fontTitle).SetFontSize(10));
                    document.Add(new Paragraph("Địa chỉ: 20b Đ. Phó Cơ Điều, Phường 3, Vĩnh Long")
                        .SetFont(fontNormal).SetFontSize(10));
                    document.Add(new Paragraph("Điện thoại: 0123 456 789")
                        .SetFont(fontNormal).SetFontSize(10));

                    document.Add(new Paragraph("\n"));

                    // ==== Tiêu đề hóa đơn ====
                    Paragraph title = new Paragraph("HÓA ĐƠN BÁN HÀNG")
                        .SetFont(fontTitle)
                        .SetFontSize(20)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetMarginBottom(20);
                    document.Add(title);

                    // ==== Thông tin đơn hàng ====
                    document.Add(new Paragraph($"Mã đơn hàng: {orderCode}")
                        .SetFont(fontNormal).SetFontSize(11));
                    document.Add(new Paragraph($"Mã khách hàng: {customerCode}")
                        .SetFont(fontNormal).SetFontSize(11));
                    document.Add(new Paragraph($"Ngày đặt hàng: {orderDate}")
                        .SetFont(fontNormal).SetFontSize(11));
                    document.Add(new Paragraph("\n"));

                    // ==== Bảng chi tiết đơn hàng ====
                    int columnCount = dvg_ctdh.Columns.Count;
                    Table table = new Table(columnCount).UseAllAvailableWidth();

                    // Header bảng
                    foreach (DataGridViewColumn column in dvg_ctdh.Columns)
                    {
                        var headerCell = new iText.Layout.Element.Cell()
                            .Add(new Paragraph(column.HeaderText)
                                .SetFont(fontTitle)
                                .SetFontSize(12)
                                .SetTextAlignment(TextAlignment.CENTER))
                            .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                            .SetPadding(5);

                        table.AddHeaderCell(headerCell);
                    }

                    // Dữ liệu từng dòng
                    foreach (DataGridViewRow row in dvg_ctdh.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                string value = cell.Value?.ToString() ?? "";
                                table.AddCell(new iText.Layout.Element.Cell()
                                    .Add(new Paragraph(value)
                                        .SetFont(fontNormal)
                                        .SetFontSize(11)
                                        .SetTextAlignment(TextAlignment.CENTER)));
                            }
                        }
                    }

                    document.Add(table);
                    document.Add(new Paragraph("\n"));

                    // ==== Tổng tiền ====
                    Paragraph total = new Paragraph($"Tổng tiền: {totalPrice} VND")
                        .SetFont(fontTitle)
                        .SetFontSize(12)
                        .SetTextAlignment(TextAlignment.RIGHT);
                    document.Add(total);

                    // ==== Ký tên / xác nhận ====
                    document.Add(new Paragraph("\n\n"));
                    document.Add(new Paragraph("Người lập hóa đơn")
                        .SetFont(fontNormal)
                        .SetTextAlignment(TextAlignment.RIGHT));



                    // 
                    document.Add(new Paragraph(createdBy)
                .SetFont(fontItalic)
                .SetFontSize(11)

                .SetTextAlignment(TextAlignment.RIGHT));
                    // Kết thúc
                    document.Close();
                }
            }
        }

        private void cbo_trangthai_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra xem trạng thái có phải là "Hoàn thành" không
                if (cbo_trangthai.SelectedValue != null && cbo_trangthai.SelectedValue.ToString() == "Hoàn thành")
                {
                    // Khi trạng thái là "Hoàn thành", tự động chọn "Đã thanh toán" cho cbo_thanhtoan
                    cbo_thanhtoan.SelectedValue = "Đã thanh toán";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
