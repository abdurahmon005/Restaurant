using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebApp.Aplication.Models.Roles;
using WebApp.Aplication.Models.UserRole;
using WebApp.Domain.Entities;


namespace WebApp.Aplication.Services.Interface
{
    public interface IUserRoleService
    {
        Task<string> CreateRole(CreateUserRoleModel model);
        Task<string> UpdateUserRole(int id, UpdateUserRoleModel model);
        Task<string> GetAllAsync(int UserId, int RoleId);
        Task<bool> DeleteUserRole(DeleteUserRoleModel model);
    }
}
