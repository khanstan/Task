using System;

namespace XMLSourceMicroservice.Services
{
    public interface ICheckWorkingHoursService
    {
        bool WorkingHours(DateTime datetime);
    }
}