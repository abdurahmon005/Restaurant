using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WebApp.Aplication.Models.Products;
using WebApp.Aplication.Services.Interface;
using WebApp.DataAccess.Persistence;
using WebApp.Domain.Entities;

namespace WebApp.Aplication.Services.Impl
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IFileStorageService _fileStorageService;
        private const string BucketName = "restaurant-products";
        public ProductService(AppDbContext context, IFileStorageService fileStorageService)
        {
            _context = context;
            _fileStorageService = fileStorageService;
        }

        public async Task<ResponseProductModel> CreateAsync(CreateProductModel model)
        {
            string imgUrl = null;
            if (model.ImageUrl != null && model.ImageUrl.Length > 0)
            {
                // Unique fayl nomi yaratish
                string fileExtension = Path.GetExtension(model.ImageUrl.FileName);
                string uniqueFileName = $"products/{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid()}{fileExtension}";

                // Faylni Minio'ga yuklash
                using var stream = model.ImageUrl.OpenReadStream();
                imgUrl = await _fileStorageService.UploadFileAsync(
                    BucketName,
                    uniqueFileName,
                    stream,
                    model.ImageUrl.ContentType
                );
            }
            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                BasePrice = model.BasePrice,
                CategoryId = model.CategoryId,
                ImageUrl = imgUrl // URL saqlanadi
            };

            _context.Add(product);
            await _context.SaveChangesAsync();

            return new ResponseProductModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                BasePrice = product.BasePrice,
                CategoryId = product.CategoryId,
                ImageUrl = product.ImageUrl
            };
        }

        public async Task<ResponseProductModel> UpdateAsync(int id, UpdateProductModel model)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                throw new Exception("Product not found");
            }
            if (model.ImgUrl != null && model.ImgUrl.Length > 0)
            {
                // Eski rasmni o'chirish
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    try
                    {
                        var uri = new Uri(product.ImageUrl);
                        var pathParts = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);

                        if (pathParts.Length >= 2)
                        {
                            string bucketName = pathParts[0];
                            string objectName = string.Join("/", pathParts.Skip(1));

                            await _fileStorageService.RemoveFileAsync(bucketName, objectName);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error deleting old image: {ex.Message}");
                    }
                }
                string fileExtension = Path.GetExtension(model.ImgUrl.FileName);
                string uniqueFileName = $"products/{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid()}{fileExtension}";

                using var stream = model.ImgUrl.OpenReadStream();
                product.ImageUrl = await _fileStorageService.UploadFileAsync(
                    BucketName,
                    uniqueFileName,
                    stream,
                    model.ImgUrl.ContentType
                );
            }
            product.Name = model.Name;
            product.Description = model.Description;
            product.BasePrice = model.Price;
            product.CategoryId = model.CategoryId;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return new ResponseProductModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                BasePrice = product.BasePrice,
                CategoryId = product.CategoryId,
                ImageUrl = product.ImageUrl
            };
        }


        public async Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ResponseProductModel>> GetAllAsync()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .ToListAsync();

            return products.Select(p => new ResponseProductModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                BasePrice = p.BasePrice,
                CategoryId = p.CategoryId,
                ImageUrl = p.ImageUrl
            }).ToList();
        }

        public async Task<ResponseProductModel> GetByIdAsync(int id)
        {
            var product = await _context.Products
            .Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);

         if (product == null) return null;

            return new ResponseProductModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                BasePrice = product.BasePrice,
                CategoryId = product.CategoryId,
                ImageUrl = product.ImageUrl
            };
        }
    }
}
