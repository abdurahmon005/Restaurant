using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Domain.Entities;

namespace WebApp.Aplication.Services.Interface
{
    public interface IJwtTokenService
    {
        string GenerateJwtToken(User user);
    }
}
