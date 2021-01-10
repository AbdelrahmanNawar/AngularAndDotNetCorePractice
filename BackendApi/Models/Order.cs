using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApi.Models
{
    public class Order
    {
        public Order()
        {
            OrderProducts = new HashSet<OrderProduct>();
        }

        public int OrderId { get; set; }

        public DateTime DeliveryDate { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalCost { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public string CreditCardNumber { get; set; }

        public DateTime CreditCardExpirationDate { get; set; }

        public string UserId { get; set; }

        public int ShippingId { get; set; }

        public virtual ShippingInfo ShippingInfo { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }

    }
}
