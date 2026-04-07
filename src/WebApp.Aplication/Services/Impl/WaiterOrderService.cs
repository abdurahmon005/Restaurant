using Microsoft.EntityFrameworkCore;
using WebApp.Aplication.Models;
using WebApp.Aplication.Models.Waiter;
using WebApp.Aplication.Services.Interface;
using WebApp.DataAccess.Persistence;
using WebApp.Domain.Entities;
using WebApp.Domain.Enums;

namespace WebApp.Aplication.Services.Impl
{
    public class WaiterOrderService : IWaiterOrderService
    {
        private readonly AppDbContext _dbContext;

        public WaiterOrderService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApiResult<WaiterOrderResponseDTO>> CreateOrderAsync(int waiterId, WaiterCreateOrderDTO dto)
        {
            var waiter = await _dbContext.Users.FindAsync(waiterId);
            if (waiter == null)
            {
                return ApiResult<WaiterOrderResponseDTO>.Failure(new[] { "Waiter not found" });
            }

            var table = await _dbContext.Tables.FindAsync(dto.TableId);
            if (table == null)
            {
                return ApiResult<WaiterOrderResponseDTO>.Failure(new[] { "Table not found" });
            }

            var productIds = dto.Items.Select(i => i.ProductId).ToList();
            var products = await _dbContext.Products
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id, p => p);

            if (products.Count != productIds.Count)
            {
                return ApiResult<WaiterOrderResponseDTO>.Failure(new[] { "One or more products not found" });
            }

            var orderDetails = new List<OrderDetails>();
            decimal totalAmount = 0;

            foreach (var item in dto.Items)
            {
                if (products.TryGetValue(item.ProductId, out var product))
                {
                    var totalPrice = product.BasePrice * item.Quantity;
                    totalAmount += totalPrice;

                    orderDetails.Add(new OrderDetails
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = product.BasePrice,
                        TotalPrice = totalPrice
                    });
                }
            }

            var order = new Order
            {
                TableId = dto.TableId,
                UserId = waiterId,
                WaiterName = waiter.UserName,
                TotalAmount = totalAmount,
                OrderStatus = OrderStatus.Pending,
                Status = StatusType.Active,
                Notes = dto.Notes,
                CreatedAt = DateTime.UtcNow,
                OrderDetails = orderDetails
            };

            table.Status = TableStatus.Occupied;

            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();

            return ApiResult<WaiterOrderResponseDTO>.Success(await MapToResponseDTO(order));
        }

        public async Task<ApiResult<WaiterOrderResponseDTO>> GetOrderByIdAsync(int orderId)
        {
            var order = await _dbContext.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.Table)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return ApiResult<WaiterOrderResponseDTO>.Failure(new[] { "Order not found" });
            }

            return ApiResult<WaiterOrderResponseDTO>.Success(MapToResponseDTOSync(order));
        }

        public async Task<ApiResult<List<WaiterOrderResponseDTO>>> GetWaiterOrdersAsync(int waiterId, WaiterOrdersFilterDTO? filter = null)
        {
            var query = _dbContext.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.Table)
                .Include(o => o.User)
                .Where(o => o.UserId == waiterId);

            if (filter != null)
            {
                if (filter.Status.HasValue)
                {
                    query = query.Where(o => o.OrderStatus == filter.Status.Value);
                }
                if (filter.TableId.HasValue)
                {
                    query = query.Where(o => o.TableId == filter.TableId.Value);
                }
                if (filter.FromDate.HasValue)
                {
                    query = query.Where(o => o.CreatedAt >= filter.FromDate.Value);
                }
                if (filter.ToDate.HasValue)
                {
                    query = query.Where(o => o.CreatedAt <= filter.ToDate.Value);
                }
            }

            var orders = await query
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            var result = orders.Select(MapToResponseDTOSync).ToList();
            return ApiResult<List<WaiterOrderResponseDTO>>.Success(result);
        }

        public async Task<ApiResult<List<WaiterOrderResponseDTO>>> GetActiveOrdersAsync(int waiterId)
        {
            var activeStatuses = new[] { OrderStatus.Pending, OrderStatus.Preparing, OrderStatus.Ready };

            var orders = await _dbContext.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.Table)
                .Include(o => o.User)
                .Where(o => o.UserId == waiterId && activeStatuses.Contains(o.OrderStatus))
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            var result = orders.Select(MapToResponseDTOSync).ToList();
            return ApiResult<List<WaiterOrderResponseDTO>>.Success(result);
        }

        public async Task<ApiResult<List<WaiterOrderResponseDTO>>> GetOrdersByTableAsync(int tableId)
        {
            var orders = await _dbContext.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.Table)
                .Include(o => o.User)
                .Where(o => o.TableId == tableId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            var result = orders.Select(MapToResponseDTOSync).ToList();
            return ApiResult<List<WaiterOrderResponseDTO>>.Success(result);
        }

        public async Task<ApiResult<WaiterOrdersSummaryDTO>> GetOrdersSummaryAsync(int waiterId)
        {
            var today = DateTime.UtcNow.Date;
            var orders = await _dbContext.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.Table)
                .Include(o => o.User)
                .Where(o => o.UserId == waiterId && o.CreatedAt >= today)
                .ToListAsync();

            var activeStatuses = new[] { OrderStatus.Pending, OrderStatus.Preparing, OrderStatus.Ready };

            var closedStatuses = new[] { OrderStatus.Completed, OrderStatus.Cancelled };

            var summary = new WaiterOrdersSummaryDTO
            {
                TotalActiveOrders = orders.Count(o => activeStatuses.Contains(o.OrderStatus)),
                ClosedOrders = orders.Count(o => closedStatuses.Contains(o.OrderStatus)),
                PendingOrders = orders.Count(o => o.OrderStatus == OrderStatus.Pending),
                PreparingOrders = orders.Count(o => o.OrderStatus == OrderStatus.Preparing),
                ReadyOrders = orders.Count(o => o.OrderStatus == OrderStatus.Ready),
                TotalSalesAmount = orders.Where(o => o.OrderStatus == OrderStatus.Completed).Sum(o => o.TotalAmount),
                Orders = orders.Select(MapToResponseDTOSync).ToList()
            };

            return ApiResult<WaiterOrdersSummaryDTO>.Success(summary);
        }

        public async Task<ApiResult<WaiterOrderResponseDTO>> UpdateOrderStatusAsync(int orderId, int waiterId, WaiterUpdateOrderStatusDTO dto)
        {
            var order = await _dbContext.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.Table)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return ApiResult<WaiterOrderResponseDTO>.Failure(new[] { "Order not found" });
            }

            if (order.UserId != waiterId)
            {
                return ApiResult<WaiterOrderResponseDTO>.Failure(new[] { "You can only update your own orders" });
            }

            order.OrderStatus = dto.NewStatus;
            if (!string.IsNullOrEmpty(dto.Notes))
            {
                order.Notes = dto.Notes;
            }

            if (dto.NewStatus == OrderStatus.Completed || dto.NewStatus == OrderStatus.Cancelled)
            {
                var hasOtherActiveOrders = await _dbContext.Orders
                    .AnyAsync(o => o.TableId == order.TableId &&
                                   o.Id != orderId &&
                                   o.OrderStatus != OrderStatus.Completed &&
                                   o.OrderStatus != OrderStatus.Cancelled);

                if (!hasOtherActiveOrders)
                {
                    order.Table.Status = TableStatus.Available;
                }
            }

            await _dbContext.SaveChangesAsync();
            return ApiResult<WaiterOrderResponseDTO>.Success(MapToResponseDTOSync(order));
        }

        public async Task<ApiResult<WaiterOrderResponseDTO>> AddItemsToOrderAsync(int orderId, int waiterId, WaiterAddItemsDTO dto)
        {
            var order = await _dbContext.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.Table)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return ApiResult<WaiterOrderResponseDTO>.Failure(new[] { "Order not found" });
            }

            if (order.UserId != waiterId)
            {
                return ApiResult<WaiterOrderResponseDTO>.Failure(new[] { "You can only modify your own orders" });
            }

            if (order.OrderStatus == OrderStatus.Completed || order.OrderStatus == OrderStatus.Cancelled)
            {
                return ApiResult<WaiterOrderResponseDTO>.Failure(new[] { "Cannot add items to completed or cancelled order" });
            }

            var productIds = dto.Items.Select(i => i.ProductId).ToList();
            var products = await _dbContext.Products
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id, p => p);

            foreach (var item in dto.Items)
            {
                if (products.TryGetValue(item.ProductId, out var product))
                {
                    var totalPrice = product.BasePrice * item.Quantity;
                    order.TotalAmount += totalPrice;

                    order.OrderDetails.Add(new OrderDetails
                    {
                        OrderId = orderId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = product.BasePrice,
                        TotalPrice = totalPrice
                    });
                }
            }

            await _dbContext.SaveChangesAsync();

            order = await _dbContext.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.Table)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            return ApiResult<WaiterOrderResponseDTO>.Success(MapToResponseDTOSync(order!));
        }

        public async Task<ApiResult<WaiterOrderResponseDTO>> RemoveItemFromOrderAsync(int orderId, int orderItemId, int waiterId)
        {
            var order = await _dbContext.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.Table)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return ApiResult<WaiterOrderResponseDTO>.Failure(new[] { "Order not found" });
            }

            if (order.UserId != waiterId)
            {
                return ApiResult<WaiterOrderResponseDTO>.Failure(new[] { "You can only modify your own orders" });
            }

            if (order.OrderStatus == OrderStatus.Completed || order.OrderStatus == OrderStatus.Cancelled)
            {
                return ApiResult<WaiterOrderResponseDTO>.Failure(new[] { "Cannot remove items from completed or cancelled order" });
            }

            var orderItem = order.OrderDetails.FirstOrDefault(od => od.Id == orderItemId);
            if (orderItem == null)
            {
                return ApiResult<WaiterOrderResponseDTO>.Failure(new[] { "Order item not found" });
            }

            order.TotalAmount -= orderItem.TotalPrice;
            order.OrderDetails.Remove(orderItem);
            _dbContext.Remove(orderItem);

            await _dbContext.SaveChangesAsync();
            return ApiResult<WaiterOrderResponseDTO>.Success(MapToResponseDTOSync(order));
        }

        public async Task<ApiResult<WaiterOrderResponseDTO>> UpdateItemQuantityAsync(int orderId, int orderItemId, int waiterId, WaiterUpdateItemQuantityDTO dto)
        {
            var order = await _dbContext.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.Table)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return ApiResult<WaiterOrderResponseDTO>.Failure(new[] { "Order not found" });
            }

            if (order.UserId != waiterId)
            {
                return ApiResult<WaiterOrderResponseDTO>.Failure(new[] { "You can only modify your own orders" });
            }

            if (order.OrderStatus == OrderStatus.Completed || order.OrderStatus == OrderStatus.Cancelled)
            {
                return ApiResult<WaiterOrderResponseDTO>.Failure(new[] { "Cannot modify completed or cancelled order" });
            }

            var orderItem = order.OrderDetails.FirstOrDefault(od => od.Id == orderItemId);
            if (orderItem == null)
            {
                return ApiResult<WaiterOrderResponseDTO>.Failure(new[] { "Order item not found" });
            }

            order.TotalAmount -= orderItem.TotalPrice;
            orderItem.Quantity = dto.Quantity;
            orderItem.TotalPrice = orderItem.UnitPrice * dto.Quantity;
            order.TotalAmount += orderItem.TotalPrice;

            await _dbContext.SaveChangesAsync();
            return ApiResult<WaiterOrderResponseDTO>.Success(MapToResponseDTOSync(order));
        }

        public async Task<ApiResult<WaiterOrderResponseDTO>> UpdateOrderNotesAsync(int orderId, int waiterId, WaiterUpdateOrderNotesDTO dto)
        {
            var order = await _dbContext.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.Table)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return ApiResult<WaiterOrderResponseDTO>.Failure(new[] { "Order not found" });
            }

            if (order.UserId != waiterId)
            {
                return ApiResult<WaiterOrderResponseDTO>.Failure(new[] { "You can only modify your own orders" });
            }

            order.Notes = dto.Notes;
            await _dbContext.SaveChangesAsync();

            return ApiResult<WaiterOrderResponseDTO>.Success(MapToResponseDTOSync(order));
        }

        public async Task<ApiResult<bool>> CancelOrderAsync(int orderId, int waiterId, string? reason = null)
        {
            var order = await _dbContext.Orders
                .Include(o => o.Table)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return ApiResult<bool>.Failure(new[] { "Order not found" });
            }

            if (order.UserId != waiterId)
            {
                return ApiResult<bool>.Failure(new[] { "You can only cancel your own orders" });
            }

            if (order.OrderStatus == OrderStatus.Completed)
            {
                return ApiResult<bool>.Failure(new[] { "Cannot cancel a completed order" });
            }

            order.OrderStatus = OrderStatus.Cancelled;
            if (!string.IsNullOrEmpty(reason))
            {
                order.Notes = $"{order.Notes}\nCancellation reason: {reason}".Trim();
            }

            var hasOtherActiveOrders = await _dbContext.Orders
                .AnyAsync(o => o.TableId == order.TableId &&
                               o.Id != orderId &&
                               o.OrderStatus != OrderStatus.Completed &&
                               o.OrderStatus != OrderStatus.Cancelled);

            if (!hasOtherActiveOrders)
            {
                order.Table.Status = TableStatus.Available;
            }

            await _dbContext.SaveChangesAsync();
            return ApiResult<bool>.Success(true);
        }

        public async Task<ApiResult<List<WaiterTableInfoDTO>>> GetAvailableTablesAsync()
        {
            var tables = await _dbContext.Tables
                .Where(t => t.Status == TableStatus.Available)
                .ToListAsync();

            var result = tables.Select(t => new WaiterTableInfoDTO
            {
                Id = t.Id,
                TableNumber = t.TableNumber.ToString(),
                Capacity = t.Capacity,
                Status = t.Status.ToString(),
                HasActiveOrder = false,
                ActiveOrderId = null
            }).ToList();

            return ApiResult<List<WaiterTableInfoDTO>>.Success(result);
        }

        public async Task<ApiResult<List<WaiterTableInfoDTO>>> GetAllTablesAsync()
        {
            var tables = await _dbContext.Tables
                .Include(t => t.Order)
                .ToListAsync();

            var activeStatuses = new[] { OrderStatus.Pending, OrderStatus.Preparing, OrderStatus.Ready };

            var result = tables.Select(t =>
            {
                var activeOrder = t.Order.FirstOrDefault(o => activeStatuses.Contains(o.OrderStatus));
                return new WaiterTableInfoDTO
                {
                    Id = t.Id,
                    TableNumber = t.TableNumber.ToString(),
                    Capacity = t.Capacity,
                    Status = t.Status.ToString(),
                    HasActiveOrder = activeOrder != null,
                    ActiveOrderId = activeOrder?.Id
                };
            }).ToList();

            return ApiResult<List<WaiterTableInfoDTO>>.Success(result);
        }

        private async Task<WaiterOrderResponseDTO> MapToResponseDTO(Order order)
        {
            await _dbContext.Entry(order)
                .Collection(o => o.OrderDetails)
                .Query()
                .Include(od => od.Product)
                .LoadAsync();

            await _dbContext.Entry(order).Reference(o => o.Table).LoadAsync();
            await _dbContext.Entry(order).Reference(o => o.User).LoadAsync();

            return MapToResponseDTOSync(order);
        }

        private WaiterOrderResponseDTO MapToResponseDTOSync(Order order)
        {
            return new WaiterOrderResponseDTO
            {
                Id = order.Id,
                TableId = order.TableId,
                TableName = order.Table?.TableNumber.ToString() ?? "",
                WaiterId = order.UserId,
                WaiterName = order.WaiterName ?? order.User?.UserName ?? "",
                TotalAmount = order.TotalAmount,
                OrderStatus = order.OrderStatus.ToString(),
                Notes = order.Notes,
                CreatedAt = order.CreatedAt,
                Items = order.OrderDetails.Select(od => new WaiterOrderItemResponseDTO
                {
                    Id = od.Id,
                    ProductId = od.ProductId,
                    ProductName = od.Product?.Name ?? "",
                    Quantity = od.Quantity,
                    UnitPrice = od.UnitPrice,
                    TotalPrice = od.TotalPrice
                }).ToList()
            };
        }
    }
}
