using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Aplication.Services
{
    public class PermissionCodeDescription
    {
        internal PermissionCodeDescription() { }    
        public string Code {  get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
    }
}
