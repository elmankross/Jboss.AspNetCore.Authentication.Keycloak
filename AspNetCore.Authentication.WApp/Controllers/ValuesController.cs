using AspNetCore.Authentication.WApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCore.Authentication.WApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ITestService _testService;

        public ValuesController(ITestService testService)
        {
            _testService = testService;
        }


        // GET api/values
        [HttpGet]
        public Task<IReadOnlyDictionary<string, List<string>>> Get()
        {
            return _testService.GetValuesAsync();
        }
    }
}
