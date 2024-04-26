using Newtonsoft.Json;
using System.Text;
using WebAppMicro.Models;
using WebAppMicro.Service.IService;
using static WebAppMicro.Utility.SD;

namespace WebAppMicro.Service
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenProvider _tokenProvider;
        public BaseService(IHttpClientFactory httpClientFactory,ITokenProvider tokenProvider) {
            _httpClientFactory = httpClientFactory;
            _tokenProvider = tokenProvider;
        }
       public async Task<ResponseDto?>  SendAsync(RequestDto requestDto, bool withBearer = true)
        {
            HttpClient client = _httpClientFactory.CreateClient("CouponApi");
            HttpRequestMessage message = new();
            if (requestDto.ContentType == ContentType.MultipartFormData)
            {
                message.Headers.Add("Accept", "*/*");
            }
            else
            {
                message.Headers.Add("Accept", "application/json");
            }
            if (withBearer)
            {
                var token = _tokenProvider.GetToken();
                message.Headers.Add("Authorization", $"Bearer {token}");
            }
            message.RequestUri = new Uri(requestDto.Url);
            if (requestDto.ContentType == ContentType.MultipartFormData)
            {
                var content = new MultipartFormDataContent();

                foreach (var prop in requestDto.Data.GetType().GetProperties())
                {
                    var value = prop.GetValue(requestDto.Data);
                    if (value is FormFile)
                    {
                        var file = (FormFile)value;
                        if (file != null)
                        {
                            content.Add(new StreamContent(file.OpenReadStream()), prop.Name, file.FileName);
                        }
                    }
                    else
                    {
                        content.Add(new StringContent(value == null ? "" : value.ToString()), prop.Name);
                    }
                }
                message.Content = content;
            }
            else {
                if (requestDto.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
                }
            }
            
            
            HttpResponseMessage? httpResponseMessage = null;
            switch (requestDto.ApiType)
            {
                  case ApiType.POST:
                    message.Method=HttpMethod.Post;
                    break;  
                  case ApiType.PUT:
                    message.Method=HttpMethod.Put;
                    break;
                  case ApiType.DELETE:
                    message.Method=HttpMethod.Delete;
                    break;
                  default:
                    message.Method=HttpMethod.Get;
                    break;


            }
            try {
                httpResponseMessage = await client.SendAsync(message);
                switch (httpResponseMessage.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        return new() { IsSuccess = false, Message = "Not Found" };
                    case System.Net.HttpStatusCode.Unauthorized:
                        return new() { IsSuccess = false, Message = "Unauthorized" };
                    case System.Net.HttpStatusCode.Forbidden:
                        return new() { IsSuccess = false, Message = "Forbidden" };
                    case System.Net.HttpStatusCode.BadRequest:
                        return new() { IsSuccess = false, Message = "Bad Request" };
                    case System.Net.HttpStatusCode.InternalServerError:
                        return new() { IsSuccess = false, Message = "Internal Server Error" };
                    default:
                        var apiContent = await httpResponseMessage.Content.ReadAsStringAsync();
                        var responseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                        return responseDto;
                }

                }
            catch (Exception ex)
                {
                    var dto=new ResponseDto { IsSuccess = false, Message = ex.Message };
                        return dto;
                }
           
       }

       
    }
}
