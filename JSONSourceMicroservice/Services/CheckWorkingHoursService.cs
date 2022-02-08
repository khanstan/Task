using System;

namespace XmlSourceMicroservice.Services
{
    public class CheckWorkingHoursService : ICheckWorkingHoursService
    {
        public bool WorkingHours(DateTime datetime)
        {

            TimeSpan start = new TimeSpan(22, 0, 0);

            TimeSpan end = new TimeSpan(10, 0, 0);

            TimeSpan now = datetime.TimeOfDay;

            if (start < end)
            {
                return start <= now && now <= end;
            }
            else
            {
                return !(end < now && now < start);
            }
        }
    }
}
