using HMACClient;
using System.Net.Http;
using System.Threading.Tasks;

namespace PublicAPI.Services
{
    public class RequestSourceService : IRequestSourceService
    {
        public async Task<HttpResponseMessage> MakeRequest(string url)
        {
            HMACDelegatingHandler customDelegatingHandler = new HMACDelegatingHandler();
            HttpClient client = HttpClientFactory.Create(customDelegatingHandler);
            client.DefaultRequestHeaders.Add("Keep-Alive", "666");

            HttpResponseMessage response = await client.GetAsync(url);

            return response;

        }
    }
}

