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
    public class ShippingInfoController : ControllerBase
    {
        private readonly StoreDBContext context;

        public ShippingInfoController(StoreDBContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = context.ShippingInfos.ToList();
            return Ok(result);
        }

        [HttpGet("{ShippingInfoId}")]
        public async Task<IActionResult> GetById([FromHeader] int ShippingInfoId)
        {
            var product = context.ShippingInfos.Where(p => p.ShippingInfoId == ShippingInfoId);
            return Ok(product);
        }
    }
}
