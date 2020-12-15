using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Web.Services.Clients
{
    public interface IMicroserviceSecond : IMicroservice
    {
    }


    public class MicroserviceSecond : IMicroserviceSecond
    {
        private readonly HttpClient _client;

        public MicroserviceSecond(HttpClient client)
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
