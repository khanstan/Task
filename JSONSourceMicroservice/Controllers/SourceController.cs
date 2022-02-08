using JsonMicroservice.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using XmlSourceMicroservice.Services;

namespace XmlSourceMicroservice.Controllers
{
    [ApiController]
    [Route("today")]
    public class SourceController : ControllerBase
    {
        private readonly Repository _repository;
        private readonly IHMACAuthenticationService _HMACAuthenticationService;
        private readonly ICheckWorkingHoursService _CheckWorkingHoursService;

        public SourceController(Repository repository, IHMACAuthenticationService hMACAuthenticationService, ICheckWorkingHoursService checkWorkingHoursService)
        {
            _repository = repository;
            _HMACAuthenticationService = hMACAuthenticationService;
            _CheckWorkingHoursService = checkWorkingHoursService;
        }

        [HttpGet]
        public JsonResult Get()
        {
            int statusCode = this.HttpContext.Response.StatusCode;

            bool isAuth = false;
            string authHeader = Request.Headers["Authorization"].ToString();
            bool working = _CheckWorkingHoursService.WorkingHours(DateTime.Now);
            isAuth = _HMACAuthenticationService.IsValidRequest(authHeader);

            if (!isAuth)
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                return new JsonResult(new { message = "Unauthorized" });
            }
            else if (!working)
            {
                Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                return new JsonResult(new { message = "ServiceIsOffline" });
            }
            else if (statusCode == 500)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return new JsonResult(new { message = "UnexpectedError" });
            }
            else
            {
                return new JsonResult(_repository.GetData());
            }

        }
    }
}
