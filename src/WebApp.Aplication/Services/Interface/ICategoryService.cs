using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Aplication.Models.Categories;
using WebApp.Aplication.Models.Products;

namespace WebApp.Aplication.Services.Interface
{
    public interface ICategoryService
    {
        Task<CategoryResponceModel> CreateCategory(CategoryCreateModel model);
        Task<CategoryResponceModel> UpdateAsync(int id, CategoryUpdateModel model);

        Task<CategoryResponceModel> GetAllAsync();
        Task<bool> DeleteAsync(CategoryDeleteModel model);    
    }
}
