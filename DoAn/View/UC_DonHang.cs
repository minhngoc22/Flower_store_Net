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
using DoAn.View.DH;
using DoAn.View.KH;

namespace DoAn.View
{
    public partial class UC_DonHang : UserControl
    {
        private OrderBLL orderBLL;
        private string? selectedOrderCode; // Mã đơn hàng đang chọn
        private string? selectedCustomerCode = ""; // Thêm dòng này vào
        public UC_DonHang()
        {
            InitializeComponent();
            orderBLL = new OrderBLL();
        }

        private void UC_DonHang_Load(object sender, EventArgs e)
        {
            LoadData(); // Tải danh sách đơn hàng
            LoadOrderStatuses(); // Tải danh sách trạng thái đơn hàng
        }

        // Load danh sách đơn hàng lên DataGridView
        private void LoadData()
        {
            DataTable orders = orderBLL.GetAllOrders();
            dvg_dh.DataSource = orders;
        }

        // Load trạng thái đơn hàng lên ComboBox
        private void LoadOrderStatuses()
        {
            DataTable dt = orderBLL.GetAllOrderStatuses();
            if (dt.Rows.Count > 0)
            {
                DataRow allRow = dt.NewRow();
                allRow["status"] = "Tất cả";
                dt.Rows.InsertAt(allRow, 0);

                cbo_trangthai.DataSource = dt;
                cbo_trangthai.DisplayMember = "status";
                cbo_trangthai.ValueMember = "status";
                cbo_trangthai.SelectedIndex = 0;
            }
            LoadData();
        }

        private void cbo_trangthai_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedStatus = cbo_trangthai.SelectedValue?.ToString() ?? string.Empty;

            if (!string.IsNullOrEmpty(selectedStatus))
            {
                if (selectedStatus == "Tất cả")
                {
                    LoadData();
                }
                else
                {
                    LoadOrdersByStatus(selectedStatus);
                }
            }
        }

        // Lấy danh sách đơn hàng theo trạng thái
        private void LoadOrdersByStatus(string status)
        {
            DataTable dt = orderBLL.GetOrderByStatus(status);
            dvg_dh.DataSource = dt;
        }

        //private void btn_themDH_Click(object sender, EventArgs e)
        //{
        //    Form form = new Form();
        //    UC_themDH uc = new UC_themDH();

        //    // Đăng ký sự kiện khi thêm đơn hàng thành công
        //    uc.OnOrderAdded += (sender, e) =>
        //    {
        //        LoadData();  // Cập nhật danh sách đơn hàng
        //    };
        //    form.Controls.Add(uc);
        //    uc.Dock = DockStyle.Fill;

        //    form.StartPosition = FormStartPosition.CenterScreen;
        //    form.Size = new Size(530, 460);
        //    form.ShowDialog();
        //}


        private void btn_update_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedOrderCode))
            {
                MessageBox.Show("Vui lòng chọn một đơn hàng để cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy dữ liệu từ BLL
            DataTable orderStatusTable = orderBLL.GetOrderStatus(selectedOrderCode);

            // Kiểm tra xem có dữ liệu không
            if (orderStatusTable.Rows.Count > 0)
            {
                string orderStatus = orderStatusTable.Rows[0]["status"].ToString(); // Lấy giá trị cột "status"

                if (orderStatus == "Đã hủy")
                {
                    MessageBox.Show("Không thể cập nhật đơn hàng đã hủy!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (orderStatus == "Hoàn thành")
                {
                    MessageBox.Show("Không thể cập nhật đơn hàng đã hoàn thành!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy đơn hàng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Nếu đơn hàng chưa hủy, mở form cập nhật
            Form form = new Form();
            UC_updateDH uc = new UC_updateDH(selectedOrderCode);
            form.Controls.Add(uc);
            uc.Dock = DockStyle.Fill;

            form.StartPosition = FormStartPosition.CenterScreen;
            form.Size = new Size(670, 650);
            form.ShowDialog();

            LoadOrderStatuses();// ✅ Nạp lại danh sách địa chỉ từ đầu
            cbo_trangthai.SelectedIndex = 0; // Chọn lại "Tất cả"
            LoadData(); // ✅ Hiển thị lại toàn bộ danh sách nhân viên
            dvg_dh.ClearSelection(); // Bỏ chọn hàng trong DataGridView
        }

        private void dvg_dh_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dvg_dh.Rows[e.RowIndex];
                selectedOrderCode = row.Cells["Mã Đơn Hàng"].Value?.ToString();
                Console.WriteLine($"Selected Order Code: {selectedOrderCode}");
            }
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {

            LoadOrderStatuses();// ✅ Nạp lại danh sách địa chỉ từ đầu
            cbo_trangthai.SelectedIndex = 0; // Chọn lại "Tất cả"
            LoadData(); // ✅ Hiển thị lại toàn bộ danh sách nhân viên
            dvg_dh.ClearSelection(); // Bỏ chọn hàng trong DataGridView
        }

        private void btn_chitiet_Click(object sender, EventArgs e)
        {
            if (dvg_dh.SelectedRows.Count > 0)
            {
                selectedOrderCode = dvg_dh.SelectedRows[0].Cells["Mã Đơn Hàng"].Value?.ToString();
                selectedCustomerCode = dvg_dh.SelectedRows[0].Cells["Mã Khách Hàng"].Value?.ToString();
            }

            if (string.IsNullOrEmpty(selectedOrderCode))
            {
                MessageBox.Show("Vui lòng chọn một đơn hàng để cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy dữ liệu từ BLL
            DataTable orderStatusTable = orderBLL.GetOrderStatus(selectedOrderCode);

            //Kiểm tra xem có dữ liệu không
            //if (orderStatusTable.Rows.Count > 0)
            //{
            //    string orderStatus = orderStatusTable.Rows[0]["status"].ToString(); // Lấy giá trị cột "status"

            //    if (orderStatus == "Đã hủy")
            //    {
            //        MessageBox.Show("Không thể thêm chi tiết đơn hàng đã hủy!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        return;
            //    }
            //    if (orderStatus == "Đang giao")
            //    {
            //        MessageBox.Show("Không thể thêm chi tiết đơn hàng đang giao!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        return;
            //    }
            //    if (orderStatus == "Hoàn thành")
            //    {
            //        MessageBox.Show("Đơn hàng đã hoàn thành không thể thêm chi tiết", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        return;
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Không tìm thấy đơn hàng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            Form form = new Form();
            UC_themchitietDH uc = new UC_themchitietDH(selectedOrderCode, selectedCustomerCode);

            form.Controls.Add(uc);
            uc.Dock = DockStyle.Fill;

            form.StartPosition = FormStartPosition.CenterScreen;
            form.Size = new Size(1250, 730);
            form.ShowDialog();

            LoadOrderStatuses();// ✅ Nạp lại danh sách địa chỉ từ đầu
            cbo_trangthai.SelectedIndex = 0; // Chọn lại "Tất cả"
            LoadData(); // ✅ Hiển thị lại toàn bộ danh sách nhân viên
            dvg_dh.ClearSelection(); // Bỏ chọn hàng trong DataGridView
        }

        private void dvg_dh_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)  // Kiểm tra nếu click chuột phải
            {
                int rowIndex = dvg_dh.HitTest(e.X, e.Y).RowIndex;  // Lấy chỉ số dòng
                if (rowIndex >= 0) // Kiểm tra có phải hàng hợp lệ
                {
                    dvg_dh.ClearSelection();
                    dvg_dh.Rows[rowIndex].Selected = true; // Chọn hàng được click
                    contextMenu_ChiTietDH.Show(dvg_dh, new Point(e.X, e.Y)); // Hiển thị menu
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dvg_dh.SelectedRows.Count > 0)
            {
                selectedOrderCode = dvg_dh.SelectedRows[0].Cells["Mã Đơn Hàng"].Value?.ToString();
                selectedCustomerCode = dvg_dh.SelectedRows[0].Cells["Mã Khách Hàng"].Value?.ToString();
            }

            if (string.IsNullOrEmpty(selectedOrderCode) || string.IsNullOrEmpty(selectedCustomerCode))
            {
                MessageBox.Show("Vui lòng chọn một đơn hàng để xem chi tiết!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Form form = new Form();
            UC_CTDH uc = new UC_CTDH(selectedOrderCode, selectedCustomerCode);

            form.Controls.Add(uc);
            uc.Dock = DockStyle.Fill;

            form.StartPosition = FormStartPosition.CenterScreen;
            form.Size = new Size(1024, 500);
            form.ShowDialog();
        }
    }
}
