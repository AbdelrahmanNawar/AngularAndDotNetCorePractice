using BackendApi.Models.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApi.Models
{
    public class ShippingInfo
    {
        public ShippingInfo()
        {
            Orders = new HashSet<Order>();
        }

        public int ShippingInfoId { get; set; }

        public ShippingMethod ShippingMethod { get; set; }

        public decimal ShippingCost { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
