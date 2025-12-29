using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Aplication.Models.Permissions
{
    public class PermissionGroupListModel
    {
        public string GroupName { get; set; } = null!; // PermissionGroup.Name
        public List<PermissionListModel> Permissions { get; set; } = new List<PermissionListModel>();
    }
}
