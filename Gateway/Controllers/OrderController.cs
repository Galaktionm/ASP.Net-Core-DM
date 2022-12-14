using Consul;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScrantonBranch.DTO;
using Steeltoe.Common.Discovery;
using Steeltoe.Discovery;

namespace Gateway.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        
        private static string currentBranch="Scranton";

        private static HttpClient client=new HttpClient();


        public OrderController()
        {
            RecurringJob.AddOrUpdate("loadPointJob", () => getCurrentBranch(), Cron.Minutely);
        }

        [HttpGet]
        public string getCurrentBranchName()
        {
            return currentBranch;
        }

        [HttpPost]
        public async Task<IActionResult> sendOrder(OrderRequest request)
        {
            HttpResponseMessage response=new HttpResponseMessage();

            if (currentBranch.Equals("Scranton"))
            {
                response=await client.PostAsJsonAsync("https://localhost:7152/api/order", request);
                if (response.IsSuccessStatusCode)
                {
                    return Ok("Order placed at Scranton branch");
                }
            }  else
            {
                response = await client.PostAsJsonAsync("https://localhost:7274/api/order", request);
                if (response.IsSuccessStatusCode)
                {
                    return Ok("Order placed at Utica branch");
                }
            }

            return BadRequest(response.Content);
        }



        public static async Task getCurrentBranch()
        {
            Task<LoadPointResults> scrantonResult = client.GetFromJsonAsync<LoadPointResults>("https://localhost:7152/api/loadPoint");
            Task<LoadPointResults> uticaResult = client.GetFromJsonAsync<LoadPointResults>("https://localhost:7274/api/loadPoint");

            if((await scrantonResult).loadPoint<(await uticaResult).loadPoint)
            {
                currentBranch = (await scrantonResult).branch;
            } else
            {
                currentBranch = (await uticaResult).branch;
            }        
        }

    }

    public class LoadPointResults
    {
        public string branch { get; set; }
        public int loadPoint { get; set; }

        public LoadPointResults() { }

        public LoadPointResults(string branch, int loadPoint)
        {
            this.branch = branch;
            this.loadPoint = loadPoint;
        }
    }
}
