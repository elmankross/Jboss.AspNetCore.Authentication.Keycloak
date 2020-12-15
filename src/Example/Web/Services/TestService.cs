using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Services
{
    using Clients;

    public interface ITestService
    {
        Task<Dictionary<string, List<string>>> GetValuesAsync();
    }


    public class TestService : ITestService
    {
        private readonly (string, IMicroservice)[] _microservices;

        public TestService(IMicroserviceFirst mFirst, IMicroserviceSecond mSecond)
        {
            _microservices = new (string name, IMicroservice client)[]
            {
                ("api-first", mFirst),
                ("api-second", mSecond)
            };
        }

        public async Task<Dictionary<string, List<string>>> GetValuesAsync()
        {
            var result = new Dictionary<string, Task<List<string>>>();

            foreach (var service in _microservices)
            {
                var t = ExecuteServiceApiAsync(service.Item2);
                result.TryAdd(service.Item1, t);
            }

            await Task.WhenAll(result.Values);
            return result.ToDictionary(x => x.Key, x => x.Value.Result);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        private async Task<List<string>> ExecuteServiceApiAsync(IMicroservice service)
        {
            try
            {
                return await service.GetValuesAsync();
            }
            catch (Exception ex)
            {
                return new List<string> { ex.Message };
            }
        }
    }
}
