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

namespace DoAn.View.DH
{
    public partial class UC_updateDH : UserControl
    {
        private string OrderCode; // Mã khách hàng cần cập nhật
        public UC_updateDH(string orderCode)
        {
            InitializeComponent();
            OrderCode = orderCode;
            LoadComboBoxData(); // Load dữ liệu vào ComboBox
            LoadOrderData(); // Tải dữ liệu đơn hàng lên form
        }

        private void LoadComboBoxData()
        {
            OrderBLL orderBLL = new OrderBLL();



            // Lấy thông tin đơn hàng theo order_code
            DataTable dtOrder = orderBLL.GetOrderByCode(OrderCode);

            if (dtOrder.Rows.Count > 0)
            {
                DataRow row = dtOrder.Rows[0]; // Lấy dòng dữ liệu đầu tiên (vì order_code là duy nhất)

                // Load trạng thái đơn hàng
                DataTable dtStatus = orderBLL.GetOrderStatusList();
                cbo_trangthai.DataSource = dtStatus;
                cbo_trangthai.DisplayMember = "status";
                cbo_trangthai.ValueMember = "status";
                cbo_trangthai.SelectedValue = row["Trạng Thái"].ToString(); // Chọn trạng thái của đơn hàng

                // Load danh sách nhân viên xử lý
                DataTable dtEmployees = orderBLL.GetEmployeeList();
                cbo_nvxl.DataSource = dtEmployees;
                cbo_nvxl.DisplayMember = "full_name"; // Hiển thị tên nhân viên
                cbo_nvxl.ValueMember = "id";          // Giá trị là id nhân viên
                cbo_nvxl.Text = row["Tên Nhân Viên Xử Lý"].ToString(); // ✅ Dùng Text thay vì SelectedValue


                // Load danh sách phương thức thanh toán
                DataTable dtPayments = orderBLL.GetPaymentMethods();
                cbo_thanhtoan.DataSource = dtPayments;
                cbo_thanhtoan.DisplayMember = "payment";
                cbo_thanhtoan.ValueMember = "payment";
                cbo_thanhtoan.SelectedValue = row["Thanh Toán"].ToString(); // Chọn phương thức thanh toán của đơn hàng
            }
            else
            {
                MessageBox.Show("Không tìm thấy đơn hàng với mã: " + OrderCode, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        // Phương thức tải dữ liệu đơn hàng lên form
        private void LoadOrderData()
        {
            OrderBLL orderBLL = new OrderBLL();
            DataTable dt = orderBLL.GetOrderByCode(OrderCode);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                txt_maDH.Text = row["Mã Đơn Hàng"].ToString();
                cbo_trangthai.SelectedItem = row["Trạng Thái"].ToString();
                cbo_nvxl.SelectedItem = row["Tên Nhân Viên Xử Lý"].ToString();
                cbo_thanhtoan.SelectedItem = row["Thanh Toán"].ToString();
                txt_tongtien.Text = row["Tổng Tiền"].ToString();
                txt_note.Text = row["Ghi Chú"].ToString();
                txt_diachi.Text = row["Địa Chỉ"].ToString();

            }

        }

        private void btn_luu_Click(object sender, EventArgs e)
        {

            try
            {
                // Lấy dữ liệu từ form
                string status = cbo_trangthai.SelectedValue?.ToString();
                string employeeId = cbo_nvxl.SelectedValue?.ToString();
                string paymentMethod = cbo_thanhtoan.SelectedValue?.ToString();

                // Chuyển đổi giá trị từ TextBox sang decimal
                decimal totalAmount;
                bool isValidAmount = decimal.TryParse(txt_tongtien.Text, out totalAmount);

                if (!isValidAmount)
                {
                    // Xử lý nếu giá trị không hợp lệ (ví dụ: thông báo lỗi cho người dùng)
                    totalAmount = 0; // Hoặc thực hiện hành động khác tùy theo yêu cầu của bạn
                }
                string note = txt_note.Text;
                string diachi = txt_diachi.Text;

                if (string.IsNullOrEmpty(OrderCode))
                {
                    MessageBox.Show("Mã đơn hàng không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Gọi BLL để cập nhật đơn hàng
                OrderBLL orderBLL = new OrderBLL();
                bool isUpdated = orderBLL.UpdateOrder(OrderCode,totalAmount, status, employeeId, paymentMethod, note,diachi);

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
