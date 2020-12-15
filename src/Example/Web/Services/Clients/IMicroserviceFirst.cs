using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Web.Services.Clients
{
    public interface IMicroserviceFirst : IMicroservice
    {
    }


    public class MicroserviceFirst : IMicroserviceFirst
    {
        private readonly HttpClient _client;

        public MicroserviceFirst(HttpClient client)
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
