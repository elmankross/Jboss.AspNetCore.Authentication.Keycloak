using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCore.Authentication.WApp.Services
{
    public interface ITestService
    {
        Task<IReadOnlyDictionary<string, List<string>>> GetValuesAsync();
    }
}
