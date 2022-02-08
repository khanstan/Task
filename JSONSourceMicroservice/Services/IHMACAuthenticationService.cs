using System.Net.Http;
using System.Threading.Tasks;

namespace XmlSourceMicroservice.Services
{
    public interface IHMACAuthenticationService
    {
        Task<byte[]> ComputeHash(HttpContent httpContent);
        bool IsValidRequest(string header);
        string[] SplitHeader(string header);
    }
}