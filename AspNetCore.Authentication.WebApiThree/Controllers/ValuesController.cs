using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AspNetCore.Authentication.WebApiThree.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet, Authorize("test")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "passed 'test' policy." };
        }
    }
}
