using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApi.Models
{
    public class OrderProduct
    {
        public int Quantity { get; set; }

        public  int OrderId { get; set; }

        public  int ProductId { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }


    }
}
