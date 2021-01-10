using BackendApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly StoreDBContext context;

        public ProductsController(StoreDBContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = context.Products.ToList();
            return Ok(result);
        }

        [HttpGet("{ProductId}")]
        public async Task<IActionResult> GetById([FromHeader] int ProductId)
        {
            var product = context.Products.Where(p => p.ProductId == ProductId);
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            var newProduct = new Product()
            {
                ProductName = product.ProductName,
                Price = product.Price,
                Quantity = product.Quantity
            };

            await context.Products.AddAsync(newProduct);
            await context.SaveChangesAsync();
            var date = DateTime.Now;
            return Ok(date);
        }
    }
}
