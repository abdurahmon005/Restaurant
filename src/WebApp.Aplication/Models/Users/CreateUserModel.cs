using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Domain.Enums;

namespace WebApp.Aplication.Models.Users
{
    public class CreateUserModel
    {
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public RoleType Role { get; set; }
        public string Password { get; set; }
    }
}
