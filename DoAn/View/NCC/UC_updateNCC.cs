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

namespace DoAn.View.NCC
{
    public partial class UC_updateNCC : UserControl
    {
        private string supplierCode;
        public UC_updateNCC(string supCode)
        {
            InitializeComponent();
            supplierCode = supCode;
            LoadSupplierData();
        }

        private void LoadSupplierData()
        {
            SupplierBLL supplierBLL = new SupplierBLL();
            DataTable dt = supplierBLL.GetSupplierByCode(supplierCode);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                txt_maNCC.Text = row["Mã Nhà Cung Cấp"].ToString();
                txt_tenNCC.Text = row["Tên Nhà Cung Cấp"].ToString();
                txt_sdt.Text = row["Số Điện Thoại"].ToString();
                txt_email.Text = row["Email"].ToString();
                txt_diachi.Text = row["Địa Chỉ"].ToString();
                txt_note.Text = row["Ghi Chú"].ToString();
            }
            else
            {
                MessageBox.Show("Không tìm thấy nhà cung cấp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_luu_Click(object sender, EventArgs e)
        {
            try
            {
                string maNCC = txt_maNCC.Text;
                string tenNCC = txt_tenNCC.Text;
                string sdt = txt_sdt.Text;
                string email = txt_email.Text;
                string diaChi = txt_diachi.Text;
                string note = txt_note.Text;

                // Kiểm tra dữ liệu nhập vào
                if (string.IsNullOrEmpty(tenNCC) || string.IsNullOrEmpty(diaChi) ||
                    string.IsNullOrEmpty(sdt) || string.IsNullOrEmpty(email))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra số điện thoại hợp lệ
                if (!long.TryParse(sdt, out _))
                {
                    MessageBox.Show("Số điện thoại không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Gọi BLL để cập nhật nhà cung cấp
                SupplierBLL supplierBLL = new SupplierBLL();
                bool success = supplierBLL.UpdateSupplier( maNCC,tenNCC, sdt, email,diaChi, note);

                if (success)
                {
                    MessageBox.Show("Cập nhật nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

