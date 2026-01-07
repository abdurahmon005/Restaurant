using Microsoft.EntityFrameworkCore;
using WebApp.Aplication.Models.Tables;
using WebApp.Aplication.Services.Interface;
using WebApp.DataAccess.Persistence;
using WebApp.Domain.Entities;

namespace WebApp.Aplication.Services.Impl
{
    public class TableService : ITableService
    {
        private readonly AppDbContext _db;
        public TableService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<TableResponceModel> CreateTableAsync(TableCreateModel model)
        {
            var exists = await _db.Tables
                .AnyAsync(t => t.TableNumber == model.TableNumber);

            if (exists)
            {
                throw new ArgumentException($"Table with number {model.TableNumber} already exists");
            }

            var table = new Table
            {
                TableNumber = model.TableNumber,
                Capacity = model.Capacity,
                Section = model.Section
            };

            await _db.Tables.AddAsync(table);
            await _db.SaveChangesAsync();

            return new TableResponceModel
            {
                Id = table.Id,
                TableNumber = table.TableNumber,
                Capacity = table.Capacity,
                Status = table.Status.ToString().ToLower(),
                Section = table.Section
            };
        }

        public async Task<bool> Delete(int id)
        {
            var table = await _db.Tables.FindAsync(id);
            if (table == null) return false;

            // Delete related orders and their details first
            var relatedOrders = await _db.Orders
                .Include(o => o.OrderDetails)
                .Include(o => o.Payments)
                .Where(o => o.TableId == id)
                .ToListAsync();

            foreach (var order in relatedOrders)
            {
                if (order.OrderDetails.Any())
                {
                    _db.OrdersDetails.RemoveRange(order.OrderDetails);
                }
                if (order.Payments.Any())
                {
                    _db.Payments.RemoveRange(order.Payments);
                }
            }

            if (relatedOrders.Any())
            {
                _db.Orders.RemoveRange(relatedOrders);
            }

            _db.Tables.Remove(table);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<TableResponceModel?> GetByIdAsync(int id)
        {
            var table = await _db.Tables.FindAsync(id);
            if (table == null) return null;

            return new TableResponceModel
            {
                Id = table.Id,
                TableNumber = table.TableNumber,
                Capacity = table.Capacity,
                Status = table.Status.ToString().ToLower(),
                Section = table.Section
            };
        }

        public async Task<List<TableResponceModel>> GetAllTablesAsync()
        {
            var tables = await _db.Tables.ToListAsync();

            return tables.Select(p => new TableResponceModel
            {
                Id = p.Id,
                TableNumber = p.TableNumber,
                Capacity = p.Capacity,
                Status = p.Status.ToString().ToLower(),
                Section = p.Section
            }).ToList();
        }

        public async Task<TableResponceModel?> UpdateAsync(int id, TableCreateModel model)
        {
            var table = await _db.Tables.FindAsync(id);
            if (table == null) return null;

            table.TableNumber = model.TableNumber;
            table.Capacity = model.Capacity;
            table.Section = model.Section;

            await _db.SaveChangesAsync();

            return new TableResponceModel
            {
                Id = table.Id,
                TableNumber = table.TableNumber,
                Capacity = table.Capacity,
                Status = table.Status.ToString().ToLower(),
                Section = table.Section
            };
        }
    }
}
