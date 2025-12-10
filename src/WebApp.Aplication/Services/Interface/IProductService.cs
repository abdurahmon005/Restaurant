using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Aplication.Models;
using WebApp.Aplication.Models.Products;

namespace WebApp.Aplication.Services.Interface
{
    public interface IProductService
    {
        Task<ResponseProductModel> CreateAsync( CreateProductModel model);
        //Task<ResponseProductModel> CreateAsync(string bucketName, string objectName, Stream data, string contentType, CreateProductModel model);
        Task<ResponseProductModel> UpdateAsync(int id, UpdateProductModel model);
        Task<bool> DeleteAsync(int id);
        Task<ResponseProductModel> GetByIdAsync(int id);
        Task<List<ResponseProductModel>> GetAllAsync();

    }
}
