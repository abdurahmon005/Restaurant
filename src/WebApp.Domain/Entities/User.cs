using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Domain.Enums;

namespace WebApp.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public RoleType Role { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsVerified { get; set; }

    }
}

      //  public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        //public ICollection<Order> OrdersTaken { get; set; } = new List<Order>();
        //public ICollection<Order> OrdersPlaced { get; set; } = new List<Order>();