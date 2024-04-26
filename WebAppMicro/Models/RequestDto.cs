using static WebAppMicro.Utility.SD;

namespace WebAppMicro.Models
{
    public class RequestDto
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public string AaccessToken { get; set; }
        public object Data { get; set; }
        public ContentType ContentType { get; set; } = ContentType.Json;
    }
}
