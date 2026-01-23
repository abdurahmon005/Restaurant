using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Aplication.Models;
using WebApp.Aplication.Models.Employees;
using WebApp.Domain.Entities;

namespace WebApp.Aplication.Services.Interface
{
        public interface IEmployeeService
        {
            Task<EmployeeResponseModel> CreateAsync(CreateEmployeesDTO dto);
            Task<EmployeeResponseModel> Update(int Id, CreateEmployeesDTO model);
            Task<List<EmployeeResponseModel>> GetAllAsync();
            Task<bool> DeleteAsync(int id);
    }
}