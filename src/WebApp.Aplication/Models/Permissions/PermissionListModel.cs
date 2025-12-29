using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Aplication.Models.Permissions
{
    public class PermissionListModel
    {
        public int Id { get; set; }
        public string ShortName { get; set; } = null!; // Permission.ShortName
        public string FullName { get; set; } = null!;   // Permission.FullName
        public string GroupName { get; set; } = null!;
    }
}
