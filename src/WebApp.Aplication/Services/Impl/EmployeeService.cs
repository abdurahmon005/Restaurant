using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Aplication.Models.Categories;
using WebApp.Aplication.Models.Employees;
using WebApp.Aplication.Models.Order;
using WebApp.Aplication.Models.Products;
using WebApp.Aplication.Models.Tables;
using WebApp.Aplication.Services.Interface;
using WebApp.DataAccess.Persistence;
using WebApp.Domain.Entities;
using WebApp.Domain.Enums;

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
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
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

        
        public async Task<bool> DeleteAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return false;
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<EmployeeResponseModel>> GetAllAsync()
        {
            var products = await _context.Employees
                
                .ToListAsync();

            return products.Select(p => new EmployeeResponseModel
            {
                Id = p.Id,
                 FirstName = p.FirstName,
                  LastName= p.LastName,
                   PhoneNumber = p.PhoneNumber,
                    Role= p.Role
            }).ToList();

        }

        public async Task<EmployeeResponseModel> Update(int Id, CreateEmployeesDTO dto)
        {
            var employee = await _context.Employees.FindAsync(Id);
            if (employee == null) return null;

            employee.FirstName = dto.FirstName;
            employee.LastName = dto.LastName;
            employee.PhoneNumber = dto.PhoneNumber;
            employee.Role = dto.Role;


            

            await _context.SaveChangesAsync();

            return new EmployeeResponseModel
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                PhoneNumber = employee.PhoneNumber,
                Role = dto.Role   
            };
        }

        
    }
}
