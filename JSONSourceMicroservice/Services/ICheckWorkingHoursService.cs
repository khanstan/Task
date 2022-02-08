using System;

namespace XmlSourceMicroservice.Services
{
    public interface ICheckWorkingHoursService
    {
        bool WorkingHours(DateTime datetime);
    }
}