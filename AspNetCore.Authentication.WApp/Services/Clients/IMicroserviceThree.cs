using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace AspNetCore.Authentication.WApp.Services.Clients
{
    public interface IMicroserviceThree
    {
        Task<IEnumerable<string>> GetValues();
    }


    public class MicroserviceThree : IMicroserviceThree
    {
        private readonly HttpClient _client;

        public MicroserviceThree(HttpClient client)
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
