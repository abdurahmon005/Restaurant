using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Aplication.Models.Categories;
using WebApp.Aplication.Models.Order;
using WebApp.Aplication.Models.Products;
using WebApp.Aplication.Services.Interface;
using WebApp.DataAccess.Persistence;
using WebApp.Domain.Entities;

namespace WebApp.Aplication.Services.Impl
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _appDbContext;

        public OrderService(AppDbContext context)
        {
            _appDbContext = context;
        }

        public async Task<ResponseOrderModel> CreateAsync(CreateOrderModel model)
        {
            var order = new Order
            {
                CustomerId = model.Customer.Id,   
                TotalAmount = model.TotalAmount,
                Status = model.Status,
                CreatedAt = DateTime.UtcNow
            };

            await _appDbContext.Orders.AddAsync(order);
            await _appDbContext.SaveChangesAsync();

            return new ResponseOrderModel
            {
                Id = order.Id,
                Status = order.Status,
                TotalPrice = order.TotalAmount
            };
        }

        public async Task<bool> DeleteAsync(DeleteOrderModel model)
        {
            var order = await _appDbContext.Orders.FindAsync(model.Id);

            if (order == null)
            {
                Console.WriteLine("не найдено");
                return false;
            }

            _appDbContext.Orders.Remove(order);
            await _appDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<ResponseOrderModel>> GetAllAsync()
        {
            var orders = await _appDbContext.Orders.ToListAsync();

            return orders.Select(p => new ResponseOrderModel
            {
                Id = p.Id,
                Status = p.Status,
                TotalPrice = p.TotalAmount
            }).ToList();
        }

        public async Task<ResponseOrderModel> GetByIdAsync(int id)
        {
            var order = await _appDbContext.Orders
                .FirstOrDefaultAsync(p => p.Id == id);

            if (order == null) return null;

            return new ResponseOrderModel
            {
                Id = order.Id,
                TotalPrice = order.TotalAmount,
                Status = order.Status
            };
        }

        public async Task<ResponseOrderModel> UpdateAsync(int id, UpdateOrderModel model)
        {
            var order = await _appDbContext.Orders.FindAsync(id);

            if (order == null)
            {
                throw new Exception("Order not found");
            }


            order.Status = model.Status;
            order.TotalAmount = model.TotalPrice;
            order.Status = model.Status;

            _appDbContext.Orders.Update(order);
            await _appDbContext.SaveChangesAsync();

            return new ResponseOrderModel
            {
                Id = order.Id,
                Status = order.Status,
                TotalPrice = order.TotalAmount
            };
        }
    }
}