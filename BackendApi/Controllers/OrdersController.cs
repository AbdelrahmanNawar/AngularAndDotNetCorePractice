using BackendApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class OrdersController : ControllerBase
    {
        private readonly StoreDBContext context;
        private readonly IServiceMessage messageSender;

        //private QueueClient queueClient;


        public OrdersController(StoreDBContext storeDBContext, IServiceMessage messageSender)
        {
            this.context = storeDBContext;
            this.messageSender = messageSender;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = context.Orders.ToList();
            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetById(int orderId)
        {
            var order = context.Orders.Where(o => o.OrderId == orderId)
                                      .Include(o => o.OrderProducts)
                                      .ThenInclude(op => op.Product);

            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] Order order)
        {
            var newOrder = new Order()
            {
                OrderStatus = OrderStatus.pending,
                OrderDate = DateTime.Now,
                DeliveryDate = order.DeliveryDate.AddMonths(1),
                ShippingId = (order.ShippingId + 1),
                CreditCardNumber = order.CreditCardNumber,
                CreditCardExpirationDate = order.CreditCardExpirationDate,
                UserId = order.UserId,
                OrderProducts = order.OrderProducts
            };

            try
            {
                var shippingCost = context.ShippingInfos.FirstOrDefault(s => s.ShippingInfoId == newOrder.ShippingId).ShippingCost;
                newOrder.TotalCost += shippingCost;
            }
            catch (Exception e)
            {
                return Problem();
            }

            context.Products.Load();
            foreach (var item in order.OrderProducts)
            {
                var product = context.Products.First(p => p.ProductId == item.ProductId);
                newOrder.TotalCost += product.Price * item.Quantity;
            }

            try
            {
                var result = await context.Orders.AddAsync(newOrder);
                await context.SaveChangesAsync();
                var message = new MessageOrderStatus { Id = newOrder.OrderId, Status = newOrder.OrderStatus };
                var messageString = JsonConvert.SerializeObject(message);
                messageSender.SendMessageAsync(messageString);
                return Ok(result);
            }
            catch (Exception e)
            {
                return Problem();
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] MessageOrderStatus orderStatus)
        {
            var order = context.Orders.FirstOrDefault(o => o.OrderId == orderStatus.Id);
            if (order == null)
                return NotFound();
            order.OrderStatus = orderStatus.Status;
            context.Entry(order).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok($"Updated to: {orderStatus.Status}");
        }


    }
}
