using DoAn.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn.Controler
{
    class CategoryBLL
    {
        private CategoryDAL categoryDAL;

        public CategoryBLL()
        {
            categoryDAL = new CategoryDAL();
        }
        //lấy danh sách danh mục
        public DataTable GetAllCategories()
        {
            return categoryDAL.GetAllCategories();
        }


        // 🟢 Thêm danh mục mới
        public bool AddCategory(string categoryName, string note)
        {
            return categoryDAL.AddCategory(categoryName, note);
        }

        // 🟢 Cập nhật danh mục
        public bool UpdateCategory(string categoryCode, string categoryName, string note)
        {
            return categoryDAL.UpdateCategory(categoryCode, categoryName, note);
        }

        // 🟢 Xóa danh mục
        public bool DeleteCategory(string categoryCode)
        {
            return categoryDAL.DeleteCategory(categoryCode);
        }
        // lây thông tin theo mã danh mục
        public DataTable GetCategoryByCode(string categoryCode)
        {
            return categoryDAL.GetCategoryByCode(categoryCode);
        }

        // 🟢 Lấy danh sách danh mục theo tên
        public DataTable SearchCategoriesByName(string categoryName)
        {
            return categoryDAL.SearchCategoriesByName(categoryName);
        }
    }
}
