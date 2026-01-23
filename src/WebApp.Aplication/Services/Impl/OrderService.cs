using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Aplication.Models;
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

        public async Task<ApiResult<string>> CreateAsync(CreateOrderModel model)
        {            
            var productIds = model.Items.Select(i => i.ProductId).ToList();
            var products = await _appDbContext.Products
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id, p => p.BasePrice);

            var orderDetails = new List<OrderDetails>();
            decimal totalAmount = 0;

            foreach (var item in model.Items)
            {
                if (products.TryGetValue(item.ProductId, out var unitPrice))
                {
                    var totalPrice = unitPrice * item.Quantity;
                    totalAmount += totalPrice;

                    orderDetails.Add(new OrderDetails
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = unitPrice,
                        TotalPrice = totalPrice
                    });
                }
            }

            var order = new Order
            {
                TotalAmount = totalAmount,
                UserId = model.UserId,
                TableId = model.TableId,
                Status = model.Status,
                CreatedAt = DateTime.UtcNow,
                OrderDetails = orderDetails
            };

            await _appDbContext.Orders.AddAsync(order);
            await _appDbContext.SaveChangesAsync();

            return ApiResult<string>.Success("yaratildi");
        }

        public async Task<bool> DeleteAsync(DeleteOrderModel model)
        {
            var order = await _appDbContext.Orders.FindAsync(model.Id);

            if (order == null)
            {
                return false;
            }

            _appDbContext.Orders.Remove(order);
            await _appDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<ResponseOrderModel>> GetAllAsync()
        {
            var orders = await _appDbContext.Orders
                .Include(o => o.OrderDetails)
                .ToListAsync();

            return orders.Select(p => new ResponseOrderModel
            {
                Id = p.Id,
                Status = p.Status.ToString().ToLower(),
                TotalAmount = p.TotalAmount,
                TableId = p.TableId,
                UserId = p.UserId,
                WaiterName = p.WaiterName,
                CreatedAt = p.CreatedAt,
                Notes = p.Notes,
                OrderDetails = p.OrderDetails.Select(d => new OrderDetailModel
                {
                    Id = d.Id,
                    OrderId = d.OrderId,
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    Price = d.TotalPrice,
                    UnitPrice = d.UnitPrice
                }).ToList()
            }).ToList();
        }

        public async Task<ResponseOrderModel> GetByIdAsync(int id)
        {
            var order = await _appDbContext.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (order == null) return null;

            return new ResponseOrderModel
            {
                Id = order.Id,
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString().ToLower(),
                TableId = order.TableId,
                UserId = order.UserId,
                WaiterName = order.WaiterName,
                CreatedAt = order.CreatedAt,
                Notes = order.Notes,
                OrderDetails = order.OrderDetails.Select(d => new OrderDetailModel
                {
                    Id = d.Id,
                    OrderId = d.OrderId,
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    Price = d.TotalPrice,
                    UnitPrice = d.UnitPrice
                }).ToList()
            };
        }

        public async Task<ResponseOrderModel> UpdateAsync(int id, UpdateOrderModel model)
        {
            var order = await _appDbContext.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                throw new Exception("Order not found");
            }

            order.Status = model.Status;
            order.TotalAmount = model.TotalPrice;
            if (!string.IsNullOrEmpty(model.Notes))
            {
                order.Notes = model.Notes;
            }

            _appDbContext.Orders.Update(order);
            await _appDbContext.SaveChangesAsync();

            return new ResponseOrderModel
            {
                Id = order.Id,
                Status = order.Status.ToString().ToLower(),
                TotalAmount = order.TotalAmount,
                TableId = order.TableId,
                UserId = order.UserId,
                WaiterName = order.WaiterName,
                CreatedAt = order.CreatedAt,
                Notes = order.Notes,
                OrderDetails = order.OrderDetails.Select(d => new OrderDetailModel
                {
                    Id = d.Id,
                    OrderId = d.OrderId,
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    Price = d.TotalPrice,
                    UnitPrice = d.UnitPrice
                }).ToList()
            };
        }
    }
}