using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Aplication.Models.Categories;
using WebApp.Aplication.Models.Roles;
using WebApp.Aplication.Services.Interface;
using WebApp.DataAccess.Persistence;
using WebApp.Domain.Entities;

namespace WebApp.Aplication.Services.Impl
{
    public class RoleService : IRoleService
    {
        private readonly AppDbContext _context;
        public RoleService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseRoleModel> CreateRole(RoleCreateModel model)
        {
            var role = new Role()
            {
                Name = model.Name,
                 Description = model.Description,
            };

            _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
            return new ResponseRoleModel
            {
                Name = model.Name,
                 Description = model.Description
            };
        }

        public async Task<bool> DeleteAsync(RoleDeleteModel model)
        {
            var role = await _context.Roles.FindAsync(model.Id);

            if (role == null)
            {
                Console.WriteLine("не найдено");
                return false;
            }

            _context.Roles.Remove(role);
            _context.SaveChanges();

            return true;
        }

        public async Task<ResponseRoleModel> GetAllAsync()
        {
            var role = await _context.Roles
        .ToListAsync();

            return role.Select(p => new ResponseRoleModel
            {
                Name = p.Name,
                 Description= p.Description
            }).FirstOrDefault();
        }

        public async Task<ResponseRoleModel> UpdateRole(string name, RoleUpdateModel model)
        {
            var role = await _context.Roles
                .Where(r => r.Name == name)
                .FirstOrDefaultAsync();

            if (role == null)
            {
                throw new Exception("Role not found");
            }

            role.Name = model.Name;
            role.Description = model.Description;

            _context.Roles.Update(role);
            await _context.SaveChangesAsync();

            return new ResponseRoleModel
            {
                Name = role.Name,
                Description = role.Description
            };
        }
    }
}
