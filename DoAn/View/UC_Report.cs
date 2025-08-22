using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoAn.BLL;

using System.Diagnostics;
using ClosedXML.Excel;
using iText.Layout.Properties;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;

using iText.IO.Font;









namespace DoAn.View
{
    public partial class UC_Report : UserControl
    {
        private ReportBLL reportBLL;   // Lớp xử lý nghiệp vụ báo cáo
        private bool isLoading = true; // Cờ đánh dấu đang khởi tạo, để tránh gọi sự kiện SelectedIndexChanged quá sớm




        public UC_Report()
        {

            InitializeComponent();          // Khởi tạo các control giao diện
            reportBLL = new ReportBLL();   // Tạo instance của lớp BLL để xử lý báo cáo

            // ====== Khởi tạo ComboBox Tháng (1 đến 12) ======
            for (int i = 1; i <= 12; i++)
            {
                cbo_thang.Items.Add(i.ToString("00")); // Thêm tháng dưới dạng 01, 02, ..., 12
            }
            cbo_thang.SelectedIndex = DateTime.Now.Month - 1; // Mặc định chọn tháng hiện tại

            // ====== Khởi tạo ComboBox Năm (hiện tại -5 đến +5) ======
            int currentYear = DateTime.Now.Year;
            for (int i = currentYear - 5; i <= currentYear + 5; i++)
            {
                cbo_nam.Items.Add(i);
            }
            cbo_nam.SelectedItem = currentYear; // Mặc định chọn năm hiện tại

            // ====== Khởi tạo ComboBox Loại báo cáo ======
            cbo_LoaiBC.Items.Add("Báo cáo tổng hợp");
            cbo_LoaiBC.Items.Add("Báo cáo kho");
            cbo_LoaiBC.Items.Add("Báo cáo đơn hàng");
            cbo_LoaiBC.Items.Add("Báo cáo doanh thu theo sản phẩm");
            cbo_LoaiBC.Items.Add("Báo cáo doanh thu theo nhân viên");
            cbo_LoaiBC.Items.Add("Báo cáo doanh thu theo khách hàng");

            isLoading = false; // Đã load xong, cho phép xử lý sự kiện

            cbo_LoaiBC.SelectedIndex = 0; // Mặc định chọn báo cáo tổng hợp

            // Gọi hàm để load báo cáo ngay khi form được khởi tạo
            LoadReportData();
        }

        private void cbo_LoaiBC_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (isLoading) return; // Tránh xử lý nếu đang load giao diện ban đầu
            LoadReportData(); // Tải lại báo cáo khi năm thay đổi
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            // Đặt lại ComboBox tháng về giá trị mặc định (tháng hiện tại)
            cbo_thang.SelectedIndex = DateTime.Now.Month - 1;

            // Đặt lại ComboBox năm về giá trị mặc định (năm hiện tại)
            cbo_nam.SelectedItem = DateTime.Now.Year;

            // Đặt lại ComboBox loại báo cáo về "Báo cáo tổng hợp"
            cbo_LoaiBC.SelectedIndex = 0;

            dvg_baocao.DataSource = reportBLL.GetAllMonthlyReport();

        }

        private void btn_them_Click(object sender, EventArgs e)
        {
            // Tạo form con để nhập thời gian
            Form logForm = new Form();
            logForm.Text = "Nhập Thời Gian";
            logForm.Size = new Size(400, 300);  // Đặt kích thước form nhỏ hơn để dễ nhìn
            logForm.StartPosition = FormStartPosition.CenterScreen;  // Đặt form con hiển thị ở giữa màn hình
            logForm.BackColor = System.Drawing.Color.WhiteSmoke;  // Màu nền nhẹ nhàng cho form

            // Thêm Label cho người dùng chọn tháng
            Label lbl_month = new Label();
            lbl_month.Text = "Chọn tháng:";
            lbl_month.Font = new Font("Arial", 12, FontStyle.Bold);  // Thêm kiểu chữ đẹp và dễ nhìn
            lbl_month.Location = new Point(50, 30);  // Đặt vị trí Label
            lbl_month.ForeColor = System.Drawing.Color.Black;  // Màu chữ đen
            logForm.Controls.Add(lbl_month);

            // Thêm ComboBox để người dùng chọn tháng
            ComboBox cbo_month = new ComboBox();
            cbo_month.Location = new Point(150, 30);
            cbo_month.Width = 150;
            cbo_month.Font = new Font("Arial", 12);  // Đặt font chữ cho ComboBox
            cbo_month.DropDownStyle = ComboBoxStyle.DropDownList;  // Không cho phép người dùng gõ vào ComboBox
            for (int i = 1; i <= 12; i++) // Thêm tháng từ 1 đến 12
            {
                cbo_month.Items.Add(i.ToString("00"));
            }
            cbo_month.SelectedIndex = DateTime.Now.Month - 1;  // Mặc định chọn tháng hiện tại
            logForm.Controls.Add(cbo_month);

            // Thêm Label cho người dùng chọn năm
            Label lbl_year = new Label();
            lbl_year.Text = "Chọn năm:";
            lbl_year.Font = new Font("Arial", 12, FontStyle.Bold);  // Kiểu chữ cho label năm
            lbl_year.Location = new Point(50, 80);
            lbl_year.ForeColor = System.Drawing.Color.Black;
            logForm.Controls.Add(lbl_year);

            // Thêm ComboBox để người dùng chọn năm
            ComboBox cbo_year = new ComboBox();
            cbo_year.Location = new Point(150, 80);
            cbo_year.Width = 150;
            cbo_year.Font = new Font("Arial", 12);  // Đặt font chữ cho ComboBox
            cbo_year.DropDownStyle = ComboBoxStyle.DropDownList;  // Không cho phép người dùng gõ vào ComboBox
            for (int i = 2020; i <= DateTime.Now.Year; i++) // Thêm năm từ 2020 đến năm hiện tại
            {
                cbo_year.Items.Add(i);
            }
            cbo_year.SelectedItem = DateTime.Now.Year;  // Mặc định chọn năm hiện tại
            logForm.Controls.Add(cbo_year);

            // Thêm nút OK để xác nhận
            Button btn_OK = new Button();
            btn_OK.Text = "OK";
            btn_OK.Font = new Font("Arial", 12, FontStyle.Bold);  // Kiểu chữ cho nút
            btn_OK.Size = new Size(100, 40);  // Kích thước nút
            btn_OK.Location = new Point(150, 130);  // Đặt vị trí nút OK
            btn_OK.BackColor = System.Drawing.Color.DeepSkyBlue;  // Màu nền cho nút OK
            btn_OK.ForeColor = System.Drawing.Color.White;  // Màu chữ của nút OK
            btn_OK.FlatStyle = FlatStyle.Flat;  // Đặt kiểu flat cho nút để đẹp hơn
            btn_OK.FlatAppearance.BorderSize = 0;  // Không có viền cho nút
            btn_OK.Click += (s, args) =>
            {
                // Kiểm tra xem người dùng đã chọn tháng và năm chưa
                if (cbo_month.SelectedItem != null && cbo_year.SelectedItem != null)
                {
                    string selectedMonth = cbo_month.SelectedItem.ToString();
                    string selectedYear = cbo_year.SelectedItem.ToString();

                    // Tạo đối tượng DateTime từ tháng và năm đã chọn
                    DateTime selectedDate = new DateTime(int.Parse(selectedYear), int.Parse(selectedMonth), 1);
                    MessageBox.Show($"Ngày đã chọn: {selectedDate.ToString("dd/MM/yyyy")}");
                    // Gọi phương thức tạo báo cáo từ BLL
                    DataTable reportData = reportBLL.CreateMonthlyReport(int.Parse(selectedMonth), int.Parse(selectedYear));
                    MessageBox.Show($"Báo cáo đã được tạo cho tháng {selectedMonth} năm {selectedYear}.");
                    // Kiểm tra xem có dữ liệu trả về không
                    LoadReportData(); // Tải lại báo cáo sau khi tạo
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn tháng và năm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                logForm.Close();
            };
            logForm.Controls.Add(btn_OK);

            // Hiển thị form con
            logForm.ShowDialog();

        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu chưa chọn dòng nào
            if (dvg_baocao.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một bản ghi để xóa.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Xác nhận trước khi xóa
            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa bản ghi này?", "Xác Nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                int selectedRowIndex = dvg_baocao.SelectedRows[0].Index; // Lấy chỉ số dòng
                var selectedRow = dvg_baocao.Rows[selectedRowIndex];     // Lấy dòng được chọn

                // Giả định có cột "Mã Báo Cáo" chứa ID báo cáo
                string reportID = selectedRow.Cells["Mã Báo Cáo"].Value.ToString();

                try
                {
                    // Gọi BLL để xóa báo cáo
                    bool success = reportBLL.DeleteReport(reportID);

                    if (success)
                    {
                        dvg_baocao.Rows.RemoveAt(selectedRowIndex); // Xóa dòng khỏi DataGridView
                        MessageBox.Show("Bản ghi đã được xóa thành công.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        dvg_baocao.DataSource = reportBLL.GetAllMonthlyReport();
                    }
                    else
                    {
                        MessageBox.Show("Xóa bản ghi không thành công.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa bản ghi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btn_xuat_Click(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu trước khi xuất
            if (dvg_baocao.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Mở hộp thoại lưu file với tùy chọn cho cả HTML và Excel
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "HTML Files|*.html|Excel Files|*.xlsx|PDF Files|*.pdf";
            saveFileDialog.Title = "Lưu báo cáo"; saveFileDialog.Title = "Lưu báo cáo";
            saveFileDialog.FileName = $"{GetEnglishReportTitle(cbo_LoaiBC.SelectedItem?.ToString() ?? "BÁO CÁO")}_{DateTime.Now:yyyyMMdd}";


            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                string extension = Path.GetExtension(filePath).ToLower();

                try
                {
                    switch (extension)
                    {
                        case ".html":
                            System.IO.File.WriteAllText(filePath, GenerateHtmlContent());
                            MessageBox.Show("Xuất báo cáo thành công dưới dạng HTML.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;

                        case ".xlsx":
                            ExportToExcel(filePath);
                            MessageBox.Show("Xuất báo cáo thành công dưới dạng Excel.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;

                        case ".pdf":
                            ExportToPdf(filePath);
                            MessageBox.Show("Xuất báo cáo thành công dưới dạng PDF.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;

                        default:
                            MessageBox.Show("Định dạng file không được hỗ trợ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                    }

                    // Mở file sau khi xuất
                    OpenFile(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xuất file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string GetEnglishReportTitle(string vietnameseTitle)
        {
            switch (vietnameseTitle)
            {
                case "Báo cáo tổng hợp": return "MonthlySummaryReport";
                case "Báo cáo kho": return "StockReport";
                case "Báo cáo đơn hàng": return "OrdersReport";
                case "Báo cáo doanh thu theo sản phẩm": return "RevenueByProduct";
                case "Báo cáo doanh thu theo nhân viên": return "RevenueByEmployee";
                case "Báo cáo doanh theo khách hàng": return "RevenueByCustomer";
                default: return "Report";
            }
        }

        // Hàm tạo chuỗi HTML từ dữ liệu
        private string GenerateHtmlContent()
        {
            StringBuilder html = new StringBuilder();

            string reportTitle = cbo_LoaiBC.SelectedItem?.ToString() ?? "BÁO CÁO";
            string currentTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            string thang = cbo_thang.SelectedItem?.ToString() ?? DateTime.Now.Month.ToString("00");
            string nam = cbo_nam.SelectedItem?.ToString() ?? DateTime.Now.Year.ToString();


            // Bắt đầu tạo HTML
            html.AppendLine("<html><head><meta charset='UTF-8'>");
            html.AppendLine("<style>");
            html.AppendLine("@media print { @page { size: A4 portrait; margin: 20mm; } }");
            html.AppendLine("body { background: #ccc; padding: 20px; display: flex; justify-content: center; font-family: \"Times New Roman\", Times, serif; }"); // Đặt font cho body
            html.AppendLine(".page { background: white; width: 210mm; min-height: 297mm; padding: 20mm; box-shadow: 0 0 15px rgba(0, 0, 0, 0.3); box-sizing: border-box; font-family: \"Times New Roman\", Times, serif; }"); // Đặt font cho page
            html.AppendLine("h2 { text-align: center; margin-top: 0; font-family: \"Times New Roman\", Times, serif; }"); // Đặt font cho h2
            html.AppendLine("p { margin: 10px 0; font-size: 14px; font-family: \"Times New Roman\", Times, serif; }"); // Đặt font cho p
            html.AppendLine("table { width: 100%; border-collapse: collapse; margin-top: 20px; font-family: \"Times New Roman\", Times, serif; }"); // Đặt font cho table
            html.AppendLine("th, td { border: 1px solid #000; padding: 8px; text-align: center; font-size: 14px; font-family: \"Times New Roman\", Times, serif; }"); // Đặt font cho th, td
            html.AppendLine("th { background-color: #f2f2f2; }");
            html.AppendLine("</style></head><body>");
            html.AppendLine("<div class='page'>");

            // Thêm thông tin cửa hàng
            html.AppendLine("<div style='text-align: left; font-weight: bold; font-size: 12px; margin-bottom: 5px; font-family: \"Times New Roman\";'>Cửa hàng hoa và phụ kiện Fleur de Vie</div>");
            html.AppendLine("<div style='text-align: left; font-size: 10px; margin-bottom: 32px; font-family: \"Times New Roman\";'>Địa chỉ: 20b Đ. Phó Cơ Điều, Phường 3, Vĩnh Long</div>");
            html.AppendLine($"<h2 style='text-align: center;'>{reportTitle.ToUpper()}</h2>");
            html.AppendLine($"<p><strong>Thời gian tạo báo cáo:</strong> {currentTime}</p>");
            html.AppendLine($"<p><strong>Tháng:</strong> {thang} &nbsp;&nbsp;&nbsp; <strong>Năm:</strong> {nam}</p>");

            // Tạo bảng dữ liệu
            html.AppendLine("<table>");
            html.AppendLine("<tr>");

            // Tiêu đề cột
            foreach (DataGridViewColumn col in dvg_baocao.Columns)
            {
                html.AppendLine($"<th>{col.HeaderText}</th>");
            }
            html.AppendLine("</tr>");

            // Dữ liệu các hàng
            foreach (DataGridViewRow row in dvg_baocao.Rows)
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

            // Thêm phần ký tên người lập đơn
            // Thêm phần địa điểm và ngày tháng để trống + ký tên
            html.AppendLine("<div style='margin-top: 50px; text-align: right; font-family: \"Times New Roman\";'>");
            html.AppendLine("<p>Vĩnh Long, ngày ..... tháng ..... năm ........</p>");
            html.AppendLine("<p><strong>Người lập hóa đơn</strong></p>");
            html.AppendLine("<br><br><br>"); // Chừa khoảng trống cho ký tên
            html.AppendLine("<p>(Ký và ghi rõ họ tên)</p>");
            html.AppendLine("</div>");



            html.AppendLine("</body></html>");

            return html.ToString();


        }


        // Hàm xuất dữ liệu sang Excel
        // Hàm xuất dữ liệu sang Excel
        private void ExportToExcel(string filePath)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Báo Cáo");

                // Thiết kế tiêu đề giống như HTML
                // Đổi tiêu đề theo lựa chọn trong cbo_LoaiBC
                var titleCell = worksheet.Cell(1, 1);
                titleCell.Value = cbo_LoaiBC.SelectedItem?.ToString() ?? "Báo cáo dữ liệu"; // Thay đổi đây
                titleCell.Style.Font.Bold = true;
                titleCell.Style.Font.FontSize = 16;
                titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                // Hợp nhất các ô tiêu đề để chiếm hết chiều rộng của bảng
                worksheet.Range(1, 1, 1, dvg_baocao.Columns.Count).Merge();

                // Thiết kế thông tin thời gian tạo báo cáo
                var timeCell = worksheet.Cell(2, 1);
                timeCell.Value = $"Thời gian tạo báo cáo: {DateTime.Now:dd/MM/yyyy HH:mm}";
                timeCell.Style.Font.Italic = true;
                timeCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                // Tiêu đề cột
                for (int col = 0; col < dvg_baocao.Columns.Count; col++)
                {
                    var headerCell = worksheet.Cell(3, col + 1);
                    headerCell.Value = dvg_baocao.Columns[col].HeaderText;
                    headerCell.Style.Font.Bold = true;
                    headerCell.Style.Fill.BackgroundColor = XLColor.LightGray;
                    headerCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    headerCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    headerCell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    headerCell.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                }

                // Dữ liệu các hàng
                for (int row = 0; row < dvg_baocao.Rows.Count; row++)
                {
                    if (!dvg_baocao.Rows[row].IsNewRow)
                    {
                        for (int col = 0; col < dvg_baocao.Columns.Count; col++)
                        {
                            var cellValue = dvg_baocao.Rows[row].Cells[col].Value;
                            var cell = worksheet.Cell(row + 4, col + 1);  // Dữ liệu bắt đầu từ hàng thứ 4

                            if (cellValue != null)
                            {
                                cell.Value = cellValue.ToString();
                            }
                            else
                            {
                                cell.Value = string.Empty;
                            }

                            // Căn chỉnh nội dung
                            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                            // Thiết lập đường viền cho các ô
                            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            cell.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                        }
                    }
                }

                // Định dạng cột tự động điều chỉnh chiều rộng giống HTML
                worksheet.Columns().AdjustToContents();

                // Lưu file Excel
                workbook.SaveAs(filePath);
            }
        }

        // Hàm xuất dữ liệu sang PDF
        private void ExportToPdf(string filePath)
        {
            using (PdfWriter writer = new PdfWriter(filePath))
            {
                using (PdfDocument pdf = new PdfDocument(writer))
                {
                    // Dùng alias iText.Layout.Document cho PDF
                    Document document = new Document(pdf);

                    // Font
                    PdfFont fontTitle = PdfFontFactory.CreateFont("C:\\Windows\\Fonts\\timesbd.ttf", PdfEncodings.IDENTITY_H); // Times New Roman Bold
                    PdfFont fontNormal = PdfFontFactory.CreateFont("C:\\Windows\\Fonts\\times.ttf", PdfEncodings.IDENTITY_H);

                    // Tiêu đề báo cáo
                    document.Add(new Paragraph("Cửa hàng hoa và phụ kiện Fleur de Vie")
                      .SetFont(fontTitle).SetFontSize(10));
                    document.Add(new Paragraph("Địa chỉ: 20b Đ. Phó Cơ Điều, Phường 3, Vĩnh Long")
                        .SetFont(fontNormal).SetFontSize(10));

                    string reportTitle = $"{cbo_LoaiBC.SelectedItem?.ToString() ?? "BÁO CÁO"} - {cbo_nam.SelectedItem?.ToString() ?? DateTime.Now.Month.ToString("00")}";
                    string thang = cbo_thang.SelectedItem?.ToString() ?? DateTime.Now.Month.ToString("00");
                    string nam = cbo_nam.SelectedItem?.ToString() ?? DateTime.Now.Year.ToString();
                    string currentTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

                    // Tiêu đề lớn
                    Paragraph title = new Paragraph(reportTitle.ToUpper())
                        .SetFont(fontTitle)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(16)
                        .SetMarginTop(20)
                        .SetMarginBottom(10);
                    document.Add(title);

                    // Thông tin cửa hàng và thời gian
                    document.Add(new Paragraph($"Thời gian tạo báo cáo: {currentTime}")
                        .SetFont(fontNormal).SetFontSize(11));
                    document.Add(new Paragraph($"Tháng: {thang}     Năm: {nam}")
                        .SetFont(fontNormal).SetFontSize(11).SetMarginBottom(10));

                    // Tạo bảng dữ liệu
                    int columnCount = dvg_baocao.Columns.Count;
                    Table table = new Table(columnCount).UseAllAvailableWidth();

                    // Header
                    foreach (DataGridViewColumn column in dvg_baocao.Columns)
                    {
                        var headerCell = new iText.Layout.Element.Cell()
                            .Add(new Paragraph(column.HeaderText)
                                .SetFont(fontTitle)  // Sử dụng font cho header
                                .SetFontSize(12)  // Thêm kích thước font cho header
                                .SetTextAlignment(TextAlignment.CENTER))
                            .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                            .SetPadding(5); // Thêm padding để dễ nhìn hơn

                        table.AddHeaderCell(headerCell);
                    }

                    // Dữ liệu từng dòng
                    foreach (DataGridViewRow row in dvg_baocao.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                string value = cell.Value?.ToString() ?? "";
                                table.AddCell(new iText.Layout.Element.Cell()
                                    .Add(new Paragraph(value).SetFont(fontNormal) // Gán font Times New Roman cho nội dung
                                        .SetFontSize(11)
                                        .SetTextAlignment(TextAlignment.CENTER)));
                            }
                        }
                    }


                    document.Add(table);
                    // ==== Ký tên / xác nhận ==== 
                    document.Add(new Paragraph("\n\n"));

                    // Dòng địa điểm và ngày tháng để trống cho người điền
                    document.Add(new Paragraph("Vĩnh Long, ngày ..... tháng ..... năm ........")
                        .SetFont(fontNormal)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFontSize(12));

                    // Dòng "Người lập hóa đơn"
                    document.Add(new Paragraph("Người lập hóa đơn")
                        .SetFont(fontNormal)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetFontSize(12));

                    document.Close();
                }
            }
        }








        //Mở file
        private void OpenFile(string filePath)
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể mở file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbo_thang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading) return; // Tránh xử lý nếu đang load giao diện ban đầu
            LoadReportData(); // Tải lại báo cáo khi tháng thay đổi
        }

        private void cbo_nam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading) return; // Tránh xử lý nếu đang load giao diện ban đầu
            LoadReportData(); // Tải lại báo cáo khi năm thay đổi
        }
        private void LoadReportData()
        {
            // Kiểm tra người dùng đã chọn tháng và năm chưa
            if (cbo_thang.SelectedItem == null || cbo_nam.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn tháng và năm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int month = int.Parse(cbo_thang.SelectedItem.ToString());
            int year = int.Parse(cbo_nam.SelectedItem.ToString());
            // Xử lý theo loại báo cáo
            switch (cbo_LoaiBC.SelectedIndex)
            {
                case 0: // Báo cáo tổng hợp
                    dvg_baocao.DataSource = reportBLL.GetAllMonthlyReport();
                    break;
                case 1: // Báo cáo kho
                    dvg_baocao.DataSource = reportBLL.GetProductSalesStockReport(month, year);
                    break;
                case 2: // Báo cáo đơn hàng
                    dvg_baocao.DataSource = reportBLL.GetOrdersReport(month, year);
                    break;
                case 3: // Doanh thu theo sản phẩm
                    dvg_baocao.DataSource = reportBLL.GetAllProductReport(month, year);
                    break;
                case 4: // Doanh thu theo nhân viên
                    dvg_baocao.DataSource = reportBLL.GetEmployeeSalesReport(month, year);
                    break;
                case 5: // Doanh thu theo khách hàng
                    dvg_baocao.DataSource = reportBLL.GetCustomerRevenueReport(month, year);
                    break;
                default:
                    dvg_baocao.DataSource = null; // Xóa dữ liệu nếu không hợp lệ
                    break;
            }
        }
    }
}

