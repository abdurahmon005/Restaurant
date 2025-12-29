using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.DataAccess.Persistence;
using WebApp.Domain.Enums;

namespace RestaurantApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            var dailySales = await _context.Orders
                .Where(o => o.CreatedAt >= today && o.CreatedAt < tomorrow)
                .SumAsync(o => o.TotalAmount);

            var totalOrders = await _context.Orders
                .Where(o => o.CreatedAt >= today && o.CreatedAt < tomorrow)
                .CountAsync();

            var occupiedTables = await _context.Tables
                .Where(t => t.Status == TableStatus.Occupied || t.Status == TableStatus.Billing)
                .CountAsync();

            var totalTables = await _context.Tables.CountAsync();

            var activeOrders = await _context.Orders
                .Where(o => o.OrderStatus == OrderStatus.Pending ||
                           o.OrderStatus == OrderStatus.Preparing ||
                           o.OrderStatus == OrderStatus.Ready)
                .CountAsync();

            // Estimate current guests (4 per occupied table average)
            var currentGuests = occupiedTables * 4;

            // Yesterday's sales for trend calculation
            var yesterday = today.AddDays(-1);
            var yesterdaySales = await _context.Orders
                .Where(o => o.CreatedAt >= yesterday && o.CreatedAt < today)
                .SumAsync(o => o.TotalAmount);

            var salesTrend = yesterdaySales > 0
                ? Math.Round(((dailySales - yesterdaySales) / yesterdaySales) * 100, 1)
                : 0;

            return Ok(new
            {
                dailySales,
                totalOrders,
                activeOrders,
                currentGuests,
                occupiedTables,
                totalTables,
                salesTrend
            });
        }

        [HttpGet("sales-chart")]
        public async Task<IActionResult> GetSalesChart([FromQuery] string period = "today")
        {
            var today = DateTime.UtcNow.Date;
            var data = new List<object>();

            if (period == "today")
            {
                // Hourly sales for today
                for (int hour = 0; hour < 24; hour++)
                {
                    var startHour = today.AddHours(hour);
                    var endHour = startHour.AddHours(1);

                    var sales = await _context.Orders
                        .Where(o => o.CreatedAt >= startHour && o.CreatedAt < endHour)
                        .SumAsync(o => o.TotalAmount);

                    data.Add(new
                    {
                        time = $"{hour:D2}:00",
                        sales = sales
                    });
                }
            }
            else if (period == "week")
            {
                // Daily sales for last 7 days
                for (int i = 6; i >= 0; i--)
                {
                    var day = today.AddDays(-i);
                    var nextDay = day.AddDays(1);

                    var sales = await _context.Orders
                        .Where(o => o.CreatedAt >= day && o.CreatedAt < nextDay)
                        .SumAsync(o => o.TotalAmount);

                    data.Add(new
                    {
                        time = day.ToString("dd.MM"),
                        sales = sales
                    });
                }
            }

            return Ok(data);
        }

        [HttpGet("popular-items")]
        public async Task<IActionResult> GetPopularItems([FromQuery] int limit = 10)
        {
            var today = DateTime.UtcNow.Date;
            var weekAgo = today.AddDays(-7);

            var popularItems = await _context.OrdersDetails
                .Include(od => od.Product)
                .ThenInclude(p => p.Category)
                .Where(od => od.Order.CreatedAt >= weekAgo)
                .GroupBy(od => new { od.ProductId, od.Product.Name, CategoryName = od.Product.Category.Name })
                .Select(g => new
                {
                    id = g.Key.ProductId,
                    name = g.Key.Name,
                    category = g.Key.CategoryName,
                    orders = g.Sum(od => od.Quantity),
                    revenue = g.Sum(od => od.TotalPrice)
                })
                .OrderByDescending(x => x.orders)
                .Take(limit)
                .ToListAsync();

            // Calculate trend (compare to previous week)
            var twoWeeksAgo = weekAgo.AddDays(-7);
            var result = new List<object>();

            foreach (var item in popularItems)
            {
                var previousWeekOrders = await _context.OrdersDetails
                    .Where(od => od.ProductId == item.id &&
                                od.Order.CreatedAt >= twoWeeksAgo &&
                                od.Order.CreatedAt < weekAgo)
                    .SumAsync(od => od.Quantity);

                var trend = previousWeekOrders > 0
                    ? Math.Round(((double)(item.orders - previousWeekOrders) / previousWeekOrders) * 100, 1)
                    : 0;

                result.Add(new
                {
                    item.id,
                    item.name,
                    item.category,
                    item.orders,
                    revenue = $"{item.revenue:N0} so'm",
                    trend
                });
            }

            return Ok(result);
        }

        [HttpGet("recent-orders")]
        public async Task<IActionResult> GetRecentOrders([FromQuery] int limit = 10)
        {
            var recentOrders = await _context.Orders
                .Include(o => o.Table)
                .Include(o => o.OrderDetails)
                .Include(o => o.User)
                .OrderByDescending(o => o.CreatedAt)
                .Take(limit)
                .Select(o => new
                {
                    id = o.Id.ToString(),
                    table = o.Table.TableNumber,
                    items = o.OrderDetails.Count,
                    total = $"{o.TotalAmount:N0} so'm",
                    status = o.OrderStatus.ToString().ToLower(),
                    time = o.CreatedAt.ToString("HH:mm"),
                    waiter = o.WaiterName ?? o.User.UserName
                })
                .ToListAsync();

            return Ok(recentOrders);
        }

        [HttpGet("tables-overview")]
        public async Task<IActionResult> GetTablesOverview()
        {
            var tables = await _context.Tables
                .Select(t => new
                {
                    t.Id,
                    number = t.TableNumber,
                    t.Capacity,
                    status = t.Status.ToString().ToLower(),
                    t.Section
                })
                .ToListAsync();

            return Ok(tables);
        }
    }
}
