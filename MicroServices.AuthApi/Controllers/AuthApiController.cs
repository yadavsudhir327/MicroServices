using MicroServices.AuthApi.Models.Dto;
using MicroServices.AuthApi.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroServices.AuthApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthApiController : ControllerBase
    {

        private readonly IAuthService _authService;
        private readonly ResponseDto _responseDto;

        public AuthApiController(IAuthService authService)
        {
            _authService = authService;
            _responseDto = new ResponseDto();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            var errorMessage = await _authService.Register(registrationRequestDto);
            if(!string.IsNullOrEmpty(errorMessage))
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = errorMessage;
                return BadRequest(_responseDto);
            }
            return Ok(_responseDto);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginRequestDto loginRequestDto)
        {
            var loginResponse = await _authService.Login(loginRequestDto);
            if (loginResponse == null || loginResponse.UserDto == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Something is wrong either username or password";
                return BadRequest(_responseDto);
            }
            _responseDto.Result= loginResponse;
            return Ok(_responseDto);
        }
        [HttpPost("AssignRole")]
        public async Task<IActionResult> Login([FromBody]RegistrationRequestDto registrationRequestDto)
        {
            var assignRoleSuccessFul = await _authService.AssignRole(registrationRequestDto.Email, registrationRequestDto.Role.ToUpper());
           if(!assignRoleSuccessFul)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Role is assigned successfully";
                return BadRequest(_responseDto);
            }
            return Ok(_responseDto);
        }


    }
}
