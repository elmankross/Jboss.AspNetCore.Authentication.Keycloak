using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Services;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITestService _testService;

        public HomeController(ITestService testService)
        {
            _testService = testService;
        }

        [HttpGet]
        public async Task<ActionResult<Dictionary<string, List<string>>>> Index()
        {
            var result = await _testService.GetValuesAsync();
            return Json(result);
        }
    }
}
