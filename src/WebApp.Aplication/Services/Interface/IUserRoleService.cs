using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebApp.Domain.Entities;


namespace WebApp.Aplication.Services.Interface
{
    public interface IUserRoleService
    {
        Task<Domain.Entities.User> CreateUserAsync(UserRole userRole);
    }
}
