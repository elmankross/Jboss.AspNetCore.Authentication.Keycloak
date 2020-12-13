using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCore.Authentication.WApp.Services.Clients
{
    public interface IMicroservice
    {
        ValueTask<List<string>> GetValuesAsync();
    }
}
