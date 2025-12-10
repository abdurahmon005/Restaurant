using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Aplication.Models.Categories;

namespace WebApp.Aplication.Services.Interface
{
    public interface ICategoryService
    {
        Task<CategoryModel> CreateCategory(CategoryCreateModel model);
        Task<CategoryModel> GetAllAsync();
        Task<CategoryModel> DeleteAsync(int id);    
    }
}
