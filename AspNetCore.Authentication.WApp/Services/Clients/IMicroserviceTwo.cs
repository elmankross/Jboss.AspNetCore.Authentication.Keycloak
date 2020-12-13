using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace AspNetCore.Authentication.WApp.Services.Clients
{
    public interface IMicroserviceTwo : IMicroservice
    {
    }


    public class MicroserviceTwo : IMicroserviceTwo
    {
        private readonly HttpClient _client;

        public MicroserviceTwo(HttpClient client)
        {
            _client = client;
        }

        public async ValueTask<List<string>> GetValuesAsync()
        {
            var stream = await _client.GetStreamAsync("api/values");
            var payload = await JsonSerializer.DeserializeAsync<List<string>>(stream);
            return payload;
        }
    }
}
