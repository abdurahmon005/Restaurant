using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Aplication.Models.Categories;
using WebApp.Aplication.Models.Products;
using WebApp.Aplication.Services.Interface;
using WebApp.DataAccess.Persistence;
using WebApp.Domain.Entities;

namespace WebApp.Aplication.Services.Impl
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _appDbContext;
        public CategoryService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<CategoryResponceModel> CreateCategory(CategoryCreateModel model)
        {
            var category = new Category()
            {
                 Name = model.Name,
            };

            await _appDbContext.Categories.AddAsync(category);
            await _appDbContext.SaveChangesAsync();
            return new CategoryResponceModel
            {
                Id = category.Id,
                Name = model.Name,
            };
        }

        public async Task<List<CategoryResponceModel>> GetAllAsync()
        {
            var categories = await _appDbContext.Categories.ToListAsync();

            return categories.Select(p => new CategoryResponceModel
            {
                Id = p.Id,
                Name = p.Name
            }).ToList();
        }

        public async Task<CategoryResponceModel> UpdateAsync(int id, CategoryUpdateModel model)
        {
            var category = await _appDbContext.Categories.FindAsync(id);

            if (category == null)
            {
                throw new Exception("Category not found");
            }

            category.Name = model.Name;


            _appDbContext.Categories.Update(category);
            await _appDbContext.SaveChangesAsync();
            return new CategoryResponceModel
            {
                Name = category.Name
            };
        }

        public async Task<bool> DeleteAsync(CategoryDeleteModel model)
        {
            var category = await _appDbContext.Categories.FindAsync(model.Id);

            if (category == null)
            {
                return false;
            }

            _appDbContext.Categories.Remove(category);
            await _appDbContext.SaveChangesAsync();

            return true;
        }
    }
}