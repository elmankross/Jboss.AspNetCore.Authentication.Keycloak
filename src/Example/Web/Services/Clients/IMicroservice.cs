using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web.Services.Clients
{
    public interface IMicroservice
    {
        ValueTask<List<string>> GetValuesAsync();
    }
}
