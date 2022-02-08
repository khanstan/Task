using System.Net.Http;
using System.Threading.Tasks;

namespace PublicAPI.Services
{
    public interface IRequestSourceService
    {
        Task<HttpResponseMessage> MakeRequest(string url);
    }
}