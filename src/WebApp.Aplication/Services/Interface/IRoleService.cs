using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Aplication.Models.Categories;
using WebApp.Aplication.Models.Roles;

namespace WebApp.Aplication.Services.Interface
{
    public interface IRoleService
    {
        public Task<ResponseRoleModel> CreateRole(RoleCreateModel model);
        public Task<ResponseRoleModel> UpdateRole(string name, RoleUpdateModel model);
        Task<ResponseRoleModel> GetAllAsync();
        Task<bool> DeleteAsync(RoleDeleteModel model);
    }
}
