using System.Net.Http;
using System.Threading.Tasks;

namespace XMLSourceMicroservice.Services
{
    public interface IHMACAuthenticationService
    {
        string[] SplitHeader(string authHeader);

        bool IsValidRequest(string header);
        Task<byte[]> ComputeHash(HttpContent httpContent);
    }
}