using MicroServices.AuthApi.Models.Dto;

namespace MicroServices.AuthApi.Service.IService
{
    public interface IAuthService
    {
        Task<string>Register(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto>Login(LoginRequestDto loginRequestDto);
        Task<bool> AssignRole(string email, string role);
    }
}
