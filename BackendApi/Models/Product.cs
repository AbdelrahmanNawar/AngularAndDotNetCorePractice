using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApi.Models
{
    public class Product
    {
        public Product()
        {
            OrderProducts = new HashSet<OrderProduct>();
        }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int? Quantity { get; set; }

        public decimal Price { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
