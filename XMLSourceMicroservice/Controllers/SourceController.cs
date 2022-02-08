using Microsoft.AspNetCore.Mvc;
using System;
using XmlSourceMicroservice.Data;
using XMLSourceMicroservice.Services;

namespace XMLSourceMicroservice.Controllers
{
    [ApiController]
    [Route("today")]
    public class SourceController : ControllerBase
    {
        private readonly Repository _repository;
        private readonly IXMLFormatterService _XMLFormatterService;
        private readonly IHMACAuthenticationService _HMACAuthenticationService;
        private readonly ICheckWorkingHoursService _CheckWorkingHoursService;

        public SourceController(Repository repository, IXMLFormatterService xMLFormatterService, IHMACAuthenticationService hMACAuthenticationService, ICheckWorkingHoursService checkWorkingHoursService)
        {
            _repository = repository;
            _XMLFormatterService = xMLFormatterService;
            _HMACAuthenticationService = hMACAuthenticationService;
            _CheckWorkingHoursService = checkWorkingHoursService;
        }

        [HttpGet]
        public ContentResult Get()
        {
            bool isAuth = false;

            string authHeader = Request.Headers["Authorization"].ToString();
            isAuth = _HMACAuthenticationService.IsValidRequest(authHeader);
            bool working = _CheckWorkingHoursService.WorkingHours(DateTime.Now);


            return new ContentResult
            {
                Content = _XMLFormatterService.XmlStringBuilder(_XMLFormatterService.ErrorCodeCheck(isAuth, working), _repository.GetData()),
                ContentType = "application/xml",
            };

        }
    }

}


