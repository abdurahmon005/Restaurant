using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Aplication.Models;
using WebApp.Aplication.Models.Categories;
using WebApp.Aplication.Models.Order;
using WebApp.Aplication.Models.Products;

namespace WebApp.Aplication.Services.Interface
{
    public interface IOrderService
    {
        Task<ApiResult<string>> CreateAsync(CreateOrderModel model);
        Task<ResponseOrderModel> UpdateAsync(int id, UpdateOrderModel model);
        Task<bool> DeleteAsync(DeleteOrderModel model);
        Task<ResponseOrderModel> GetByIdAsync(int id);
        Task<List<ResponseOrderModel>> GetAllAsync();
    }
}
