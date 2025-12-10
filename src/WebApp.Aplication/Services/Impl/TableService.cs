using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var table = new Table
            {
                TableNumber = model.TableNumber
            };
            await _db.Tables.AddAsync(table);
            await _db.SaveChangesAsync();

            return new TableResponceModel
            {
                Id = table.Id,
                TableNumber = table.TableNumber
            };
        }

        public async Task<bool> Delete(int id)
        {
            var table = await _db.Tables.FindAsync(id);
            if(table == null) return false;

            _db.Tables.Remove(table);
            await _db.SaveChangesAsync();

            return true;
        }
        public async Task<TableResponceModel> GetByIdAsync(int id)
        {
            var table = await _db.Tables.FindAsync(id);
            if(table ==null) return null;

            return new TableResponceModel
            {
                Id = table.Id,
                TableNumber = table.TableNumber
            };
        }

        public async Task<IEnumerable<TableResponceModel>> GetTableAsync()
        {
            return await _db.Tables.Select(t => new TableResponceModel
            {
                Id = t.Id,
                TableNumber = t.TableNumber
            }).ToListAsync();

        }
    }
}
