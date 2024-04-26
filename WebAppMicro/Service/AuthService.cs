
using WebAppMicro.Models;
using WebAppMicro.Service.IService;
using WebAppMicro.Utility;

namespace WebAppMicro.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService baseService;
        public AuthService(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        public async Task<ResponseDto?> AssignRoleAsyn(RegistrationRequestDto registrationRequestDto)
        {
            return await baseService.SendAsync(new RequestDto { ApiType = SD.ApiType.POST, Url = SD.AuthBaseAPI + "/api/auth/AssignRole", Data = registrationRequestDto }, withBearer: false);
        }

        public async Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            return await baseService.SendAsync(new RequestDto { ApiType = SD.ApiType.POST, Url = SD.AuthBaseAPI + "/api/auth/login", Data = loginRequestDto },withBearer:false);
        }

        public async Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await baseService.SendAsync(new RequestDto { ApiType = SD.ApiType.POST, Url = SD.AuthBaseAPI + "/api/auth/register", Data = registrationRequestDto }, withBearer: false);
        }
    }
}
