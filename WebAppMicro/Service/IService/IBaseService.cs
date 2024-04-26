using WebAppMicro.Models;

namespace WebAppMicro.Service.IService
{
    public interface IBaseService
    {
        Task<ResponseDto? >  SendAsync(RequestDto requestDto,bool withBearer=true);
    }
}
