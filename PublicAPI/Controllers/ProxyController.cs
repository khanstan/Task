using Microsoft.AspNetCore.Mvc;
using PublicAPI.Services;
using System.Net.Http;
using System.Threading.Tasks;

namespace PublicAPI.Controllers
{

    [Route("[action]")]
    [ApiController]
    public class ProxyController : ControllerBase
    {
        private readonly IRequestSourceService _RequestSourceService;

        internal const string XmlSourceBaseUrl = "https://localhost:44372/today";
        internal const string JsonSourceBaseUrl = "https://localhost:44382/today";
        internal string[] sourceOrder = new string[2] { JsonSourceBaseUrl, XmlSourceBaseUrl };

        public ProxyController(IHttpClientFactory httpClientFactory, IRequestSourceService requestSourceService)
        {
            _RequestSourceService = requestSourceService;
        }

        [HttpGet]
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            ContentResult result = await ProxyTo(sourceOrder[0]);

            if (result.StatusCode != 200 || result.Content.Contains("False"))
            {
                return await ProxyTo(sourceOrder[1]);
            }
            else
            {
                return result;
            }
        }


        private async Task<ContentResult> ProxyTo(string url)
        {
            Task<HttpResponseMessage> request = _RequestSourceService.MakeRequest(url);

            Task<string> content = request.Result.Content.ReadAsStringAsync();

            return new ContentResult()
            {
                Content = await content,
                ContentType = request.Result.Content.Headers.ContentType.ToString(),
                StatusCode = (int?)request.Result.StatusCode
            };
        }

    }
}
