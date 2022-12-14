using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UticaBranch;

namespace ScrantonBranch.Controllers
{
    [Route("api/loadPoint")]
    [ApiController]
    public class LoadPointController : ControllerBase
    {

        private DatabaseContext dbContext;
        public LoadPointController(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> returnLoadScore()
        {
            return Ok(new { branch = "Utica", loadPoint = dbContext.Orders.Count() });
        }

    }
}
