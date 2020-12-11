using AspNetCore.Authentication.WApp.Services.Clients;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCore.Authentication.WApp.Services
{
    public class TestService : ITestService
    {
        private readonly IMicroserviceTwo _msTwo;
        private readonly IMicroserviceThree _msThree;

        public TestService(IMicroserviceTwo msTwo, IMicroserviceThree msThree)
        {
            _msTwo = msTwo;
            _msThree = msThree;
        }

        public async Task<IEnumerable<string>> GetValues()
        {
            var result = new List<string>();

            try
            {
                var t1 = _msTwo.GetValues();
                var t2 = _msThree.GetValues();

                await Task.WhenAll(t1, t2);

                result.AddRange(t1.Result);
                result.AddRange(t2.Result);
            }
            catch (Exception ex)
            {
                result = new List<string> { ex.Message };
            }

            return result;
        }
    }
}
