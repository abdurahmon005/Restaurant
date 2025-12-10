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
        Task<TableResponceModel> CreateTableAsync(TableCreateModel model);
        Task<IEnumerable<TableResponceModel>> GetTableAsync();
        Task<TableResponceModel> GetByIdAsync(int id);
        Task<bool> Delete(int id);
    }
}
