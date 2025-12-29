using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Aplication.Models.Tables;

namespace WebApp.Aplication.Services.Interface
{
    public interface ITableService
    {
        public Task<TableResponceModel> CreateTableAsync(TableCreateModel model);
        public Task<List<TableResponceModel>> GetAllTablesAsync();
        public Task<TableResponceModel?> GetByIdAsync(int id);
        public Task<bool> Delete(int id);
    }
}
