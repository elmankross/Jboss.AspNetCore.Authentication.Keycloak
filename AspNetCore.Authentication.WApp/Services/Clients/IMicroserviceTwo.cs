using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace AspNetCore.Authentication.WApp.Services.Clients
{
    public interface IMicroserviceTwo
    {
        Task<IEnumerable<string>> GetValues();
    }


    public class MicroserviceTwo : IMicroserviceTwo
    {
        private readonly HttpClient _client;

        public MicroserviceTwo(HttpClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<string>> GetValues()
        {
            var stream = await _client.GetStreamAsync("api/values");
            var payload = await JsonSerializer.DeserializeAsync<List<string>>(stream);
            return payload;
        }
    }
}
