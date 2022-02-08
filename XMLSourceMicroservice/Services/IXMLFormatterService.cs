using XmlSourceMicroservice.Models;

namespace XMLSourceMicroservice.Services
{
    public interface IXMLFormatterService
    {
        public bool successCheck { get; set; }
        int ErrorCodeCheck(bool authenticated, bool working);
        string XmlStringBuilder(int error, XmlModel data);
    }
}