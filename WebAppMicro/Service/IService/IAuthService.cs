using WebAppMicro.Models;

namespace WebAppMicro.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto);
        Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto);
        Task<ResponseDto?> AssignRoleAsyn(RegistrationRequestDto registrationRequestDto);
    }
}
