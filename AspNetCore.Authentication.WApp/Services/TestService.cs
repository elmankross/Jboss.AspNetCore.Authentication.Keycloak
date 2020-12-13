using AspNetCore.Authentication.WApp.Services.Clients;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Authentication.WApp.Services
{
    public class TestService : ITestService
    {
        private readonly (string, IMicroservice)[] _microservices;

        public TestService(IMicroserviceTwo msTwo, IMicroserviceThree msThree)
        {
            _microservices = new (string name, IMicroservice client)[]
            {
                ("ms:twoo", msTwo),
                ("ms:three", msThree)
            };
        }

        public async Task<IReadOnlyDictionary<string, List<string>>> GetValuesAsync()
        {
            var result = new ConcurrentDictionary<string, Task<List<string>>>();

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
