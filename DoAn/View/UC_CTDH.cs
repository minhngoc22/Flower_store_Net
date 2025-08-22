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

namespace DoAn.View
{
    public partial class UC_CTDH : UserControl
    {
        private string orderCode;
        private string customerCode;
        private OrderDetailsBLL orderDetailsBLL; // Khai báo biến toàn c
        public UC_CTDH(string orderCode, string customerCode)
        {
            InitializeComponent();
            this.orderCode = orderCode;
            this.customerCode = customerCode;

            // Khởi tạo đối tượng BLL trước khi sử dụng
            orderDetailsBLL = new OrderDetailsBLL();
            LoadData(); // Gọi hàm để hiển thị dữ liệu lên giao diện
        }

        private void UC_CTDH_Load(object sender, EventArgs e)
        {
            LoadData();
            // Lấy tổng tiền từ BLL và hiển thị
            decimal totalPrice = orderDetailsBLL.GetTotalPriceByOrderCode(orderCode);
            txt_tongtien.Text = totalPrice.ToString("N0"); // Hiển thị số có dấu phân cách
        }

        private void LoadData()
        {
            // Hiển thị mã đơn hàng và mã khách hàng lên các label hoặc textbox
           

             DataTable dt = orderDetailsBLL.GetAll(orderCode);
             dvg_ctdh.DataSource = dt;


        }
    }
}
