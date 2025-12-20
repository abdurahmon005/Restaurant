using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebApp.Aplication.Models.Categories;
using WebApp.Aplication.Models.Roles;
using WebApp.Aplication.Models.UserRole;
using WebApp.Aplication.Services.Interface;
using WebApp.DataAccess.Persistence;
using WebApp.Domain.Entities;

namespace WebApp.Aplication.Services.Impl
{
    public class UserRoleService : IUserRoleService
    {
        private readonly AppDbContext _context;
        public UserRoleService(AppDbContext context)
        {
            context = _context;
        }

        public async Task<string> CreateRole(CreateUserRoleModel model)
        {
            var userRole = new UserRole()
            {
                RoleId = model.RoleId,
                UserId = model.UserId
            };
            if (userRole == null)
            {
                return "Not Created";
            }

            _context.UserRoles.AddAsync(userRole);
            await _context.SaveChangesAsync();

            return "Created";
        }

        public async Task<bool> DeleteUserRole(DeleteUserRoleModel model)
        {
            var userRole = await _context.UserRoles.FirstOrDefaultAsync(u=>u.id == model.UserId);

            if (userRole == null)
            {
                return false;
            }


            _context.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<string> GetAllAsync(int userId, int roleId)
        {
            var userRole = await _context.UserRoles
                .Where(ur => ur.UserId == userId && ur.RoleId == roleId)
                .FirstOrDefaultAsync();

            if (userRole != null)
            {
                return "Topildi"; 
            }

            return "Topilmadi"; 
        }


        public async Task<string> UpdateUserRole(int id, UpdateUserRoleModel model)
        {
            
            var userRole = await _context.UserRoles
                .FirstOrDefaultAsync(ur => ur.id == id);

            if (userRole == null)
            {
                return "Not Found"; 
            }

            userRole.UserId = model.UserId;
            userRole.RoleId = model.RoleId;
            

            
            await _context.SaveChangesAsync();

            return "Muvaffaqiyatli yangilandi"; 
        }
    }
}