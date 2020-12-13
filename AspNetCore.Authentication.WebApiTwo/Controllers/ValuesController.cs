using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AspNetCore.Authentication.WebApiTwo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet, Authorize(Policy = "adm")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "passed 'adm' policy." };
        }
    }
}
