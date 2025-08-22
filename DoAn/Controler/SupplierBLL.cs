using System.Data;
using DoAn.Models;  // Import lớp DAL

namespace DoAn.BLL
{
    public class SupplierBLL
    {
        private SupplierDAL supplierDAL;

        public SupplierBLL()
        {
            supplierDAL = new SupplierDAL();
        }

        // 🟢 Lấy danh sách nhà cung cấp
        public DataTable GetAllSuppliers()
        {
            return supplierDAL.GetAllSuppliers();
        }

        // 🟢 Thêm nhà cung cấp
        public bool AddSupplier(string supplierName, string phone, string email, string address, string note)
        {
            return supplierDAL.AddSupplier(supplierName, phone, email, address, note);
        }

        // 🟢 Cập nhật nhà cung cấp
        public bool UpdateSupplier(string supplier_code, string supplierName, string phone, string email, string address, string note)
        {
            return supplierDAL.UpdateSupplier(supplier_code, supplierName, phone, email, address, note);
        }

        // 🟢 Xóa nhà cung cấp
        public bool DeleteSupplier(string supplier_code)
        {
            return supplierDAL.DeleteSupplier(supplier_code);
        }

        // 🟢 Tìm kiếm nhà cung cấp theo tên
        public DataTable SearchSuppliersByName(string keyword)
        {
            return supplierDAL.SearchSuppliersByName(keyword);
        }

        // 🟢 Lấy danh sách nhà cung cấp theo địa chỉ
        public DataTable GetSuppliersByAddress(string address)
        {
            return supplierDAL.GetSuppliersByAddress(address);
        }

        public DataTable GetAllSupplierAddresses()
        {
            return supplierDAL.GetAllSupplierAddresses();
        }

        public DataTable GetSupplierByCode(string supplierCode)
        {
           
            return supplierDAL.GetSupplierByCode(supplierCode);
        }

    }
}
