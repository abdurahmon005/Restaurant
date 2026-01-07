using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Aplication.Models.Categories;
using WebApp.Aplication.Models.Employees;
using WebApp.Aplication.Models.Order;
using WebApp.Aplication.Services.Interface;
using WebApp.DataAccess.Persistence;
using WebApp.Domain.Entities;

namespace WebApp.Aplication.Services.Impl
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _context;
        public EmployeeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EmployeeResponseModel> CreateAsync(CreateEmployeesDTO dto)
        {
            var employees = new Employees()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                CreatedAt = DateTime.Now,
                Role = dto.Role
            };
            await _context.Employees.AddAsync(employees);
            await _context.SaveChangesAsync();

            return new EmployeeResponseModel
            {
                FirstName = employees.FirstName,
                LastName = employees.LastName,
                PhoneNumber = employees.PhoneNumber,
                Role = employees.Role
            };

        }

        public async Task<bool> Delete(int id, DeleteEmployeeDTO dto)
        {
            var result = await _context.Employees.FindAsync(id);
            if (result == null)
            {
                return false;
            }
            _context.Employees.Remove(result);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<EmployeeResponseModel>> GetAll()
        {
            var result = await _context.Employees.ToListAsync();

            return result.Select(p => new EmployeeResponseModel
            {
                FirstName = p.FirstName,
                LastName = p.LastName,
                PhoneNumber = p.PhoneNumber,
                Role = p.Role
            }).ToList();
        }

        public async Task<EmployeeResponseModel> Update(int Id, UpdateEmployeeDTO dto)
        {
            var result = await _context.Employees.FindAsync(Id);

            if (result == null)
            {
                throw new Exception("Category not found");
            }

            result.FirstName = dto.FirstName;
            result.LastName = dto.LastName;
            result.PhoneNumber = dto.PhoneNumber;
            result.Role = dto.Role;


            _context.Employees.Update(result);
            await _context.SaveChangesAsync();
            return new EmployeeResponseModel
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                Role = dto.Role
            };
        }

       
    }
}
