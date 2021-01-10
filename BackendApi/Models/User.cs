using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApi.Models
{
    public class User : IdentityUser
    {
        public User()
            :base()
        {
            Orders = new HashSet<Order>();
        }

        public virtual ICollection<Order> Orders { get; set;}
    }
}
