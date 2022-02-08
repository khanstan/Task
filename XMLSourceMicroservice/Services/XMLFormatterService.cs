using System.Text;
using XmlSourceMicroservice.Models;

namespace XMLSourceMicroservice.Services
{
    public class XMLFormatterService : IXMLFormatterService
    {

        public bool successCheck { get; set; }

        public int ErrorCodeCheck(bool authenticated, bool working)
        {
            if (authenticated && working)
            {
                // Success
                successCheck = true;
                return 0;
            }
            else if (!authenticated)
            {
                // Unauthorized
                return 1;
            }
            else if (!working)
            {
                // Service unavailable (Source is offline) 
                return 3;
            }
            else
            {
                // Unexpected error.
                return 2;
            }
        }

        public string XmlStringBuilder(int error, XmlModel data)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"<?xml version=""1.0"" encoding=""utf-8""?>");
            sb.AppendLine("<response>");
            sb.AppendLine($"<success>{successCheck}</success>");
            sb.AppendLine($"<error>{error}</error>");
            if (successCheck)
            {
                sb.AppendLine("<data>");
                sb.AppendLine($"<temperature>{data.Temperature}</temperature>");
                sb.AppendLine($"<pressure>{data.Pressure}</pressure>");
                sb.AppendLine("</data>");
            }
            sb.AppendLine("</response>");
            return sb.ToString();
        }
    }
}
