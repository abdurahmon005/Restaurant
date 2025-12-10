using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Domain.Enums;

namespace WebApp.Aplication.Models.Users
{
    public class UserListResponceModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public RoleType Role {  get; set; }



    }
}
