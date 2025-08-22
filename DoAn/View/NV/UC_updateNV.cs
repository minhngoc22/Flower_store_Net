using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection.Emit;
using DoAn.Controler;

namespace DoAn.View.NV
{
    public partial class UC_updateNV : UserControl
    {
        private string employeeCode;
        public UC_updateNV(string empCode)
        {
            employeeCode = empCode;  // Lưu mã nhân viên để load dữ liệu
            InitializeComponent();
            LoadEmployeeData();

        }

        private void LoadEmployeeData()
        {
            NhanVienBLL employeeBLL = new NhanVienBLL();
            DataTable dt = employeeBLL.GetEmployeeByCode(employeeCode);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                txt_maNV.Text = row["Mã Nhân Viên"].ToString();
                txt_tenNV.Text = row["Họ Và Tên"].ToString();
                txt_sdt.Text = row["Số Điện Thoại"].ToString();
                txt_email.Text = row["Email"].ToString();
                txt_diachi.Text = row["Địa Chỉ"].ToString();
                txt_luong.Text = row["Lương/giờ"].ToString();
                txt_vitri.Text = row["Chức Vụ"].ToString();
                txt_note.Text = row["Ghi Chú"].ToString();
                txt_wh.Text = row["Giờ làm trong tháng"].ToString();
                txt_endDate.Text = row["Ngày Kết Thúc"].ToString();

                // 🟢 Lấy và gán giá trị của salaryType vào các checkbox
                string salaryType = row["Hệ số lương"].ToString();  // Giả sử dữ liệu trong cột "SalaryType"

                if (salaryType == "hourly")
                {
                    chk_giờ.Checked = true;
                    chk_thang.Checked = false;  // Deselect other checkbox if hourly is selected
                }
                else if (salaryType == "monthly")
                {
                    chk_thang.Checked = true;
                    chk_giờ.Checked = false;  // Deselect other checkbox if monthly is selected
                }
                else
                {
                    chk_giờ.Checked = false;
                    chk_thang.Checked = false;  // Clear both if no valid salary type is found
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy nhân viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_luu_Click(object sender, EventArgs e)
        {
            try
            {
                string maNV = txt_maNV.Text;
                string tenNV = txt_tenNV.Text;
                string sdt = txt_sdt.Text;
                string email = txt_email.Text;
                string diaChi = txt_diachi.Text;
                decimal luong = decimal.Parse(txt_luong.Text);
                decimal wh = decimal.Parse(txt_wh.Text);
                string vitri = txt_vitri.Text;
                string note = txt_note.Text;
                DateTime? endDate = null;

                // Kiểm tra và lấy ngày nghỉ việc nếu có
                if (!string.IsNullOrWhiteSpace(txt_endDate.Text))
                {
                    if (DateTime.TryParse(txt_endDate.Text, out DateTime parsedDate))
                    {
                        endDate = parsedDate;
                    }
                    else
                    {
                        MessageBox.Show("Ngày nghỉ việc không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // 🟢 Lấy giá trị từ các CheckBox cho salaryType
                string salaryType = "";

                // Kiểm tra các checkbox để xác định loại lương
                if (chk_giờ.Checked)
                {
                    salaryType = "hourly";
                }
                else if (chk_thang.Checked)
                {
                    salaryType = "monthly";
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn kiểu lương (Giờ hoặc Tháng).", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Gọi BLL để cập nhật nhân viên, bao gồm cả salaryType
                NhanVienBLL employeeBLL = new NhanVienBLL();
                bool success = employeeBLL.UpdateEmployee(maNV, tenNV, sdt, email, diaChi, luong, wh, vitri, note, endDate, salaryType);

                if (success)
                {
                    MessageBox.Show("Cập nhật nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
