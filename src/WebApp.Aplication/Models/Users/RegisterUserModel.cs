using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Aplication.Models.Users
{
    public  class RegisterUserModel
    {
        public string Fullname { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public bool isAdminSite { get; set; }
    }
}
