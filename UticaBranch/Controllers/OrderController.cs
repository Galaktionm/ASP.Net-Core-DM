using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UticaBranch.DTO;
using UticaBranch.Entities;
using UticaBranch;

namespace ScrantonBranch.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private DatabaseContext dbContext;
        private IConfiguration configuration;

        public OrderController(DatabaseContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
        }



        public async Task<IActionResult> placeOrder(OrderRequest request)
        {
            List<OrderProduct> totalOrderedProducts = new List<OrderProduct>();
            foreach (Dictionary<string, int> products in request.orderedProducts)
            {
                foreach(KeyValuePair<string, int> product in products)
                {
                    var productFromDb=dbContext.Products.Find(product.Key);
                    if (productFromDb != null)
                    {
                        totalOrderedProducts.Add(new OrderProduct(productFromDb, product.Value));
                    }
                    else
                    {
                        return BadRequest($"Product {product.Key} not available");
                    }
                }                       
            }

            Order order = new Order(request.client, totalOrderedProducts);
            dbContext.Add(order);
            dbContext.SaveChanges();

            return Ok("Order placed");

        }
    }
}
