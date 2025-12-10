using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Aplication.Models.Users
{
    public class LoginUserModel
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }

    }
    public class LoginResponceModel
    {
        public string UserName { get; set; }
        public string Phonenumber { get; set; }
        public string  Token { get; set; }  

    }
}
