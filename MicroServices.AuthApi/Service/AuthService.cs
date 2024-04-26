using MicroServices.AuthApi.Data;
using MicroServices.AuthApi.Models;
using MicroServices.AuthApi.Models.Dto;
using MicroServices.AuthApi.Service.IService;
using Microsoft.AspNetCore.Identity;

namespace MicroServices.AuthApi.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(AppDbContext context,IJwtTokenGenerator jwtTokenGenerator, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            // var user= _context.ApplicationUsers.FirstOrDefault(u=>u.UserName.ToLower()==loginRequestDto.Username.ToLower());
            var user = await _userManager.FindByNameAsync(loginRequestDto.Username);
             //bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            //if (user == null /*|| !isValid*/)
            //{
            //    var token = _jwtTokenGenerator.GenerateToken(user);
            //    return new LoginResponseDto { Token = token, UserDto = user };
            //}
            var roles = await _userManager.GetRolesAsync(user);
            UserDto userDto = new()
            {
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };
            LoginResponseDto loginResponseDto = new LoginResponseDto
            {
                UserDto = userDto,
                Token = _jwtTokenGenerator.GenerateToken(user, roles)
        };
            return loginResponseDto;
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                Name = registrationRequestDto.Name,
                PhoneNumber = registrationRequestDto.PhoneNumber,
            };
            try
            {
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded) return "";
                return result.Errors.First().Description;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

       public async Task<bool>  AssignRole(string email, string role)
        {  
            ApplicationUser user =  _context.ApplicationUsers.FirstOrDefault(u=>u.Email.ToLower()==email.ToLower());
            if(user == null) return false;
            if(!_roleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
            {
             _roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
            }
            await _userManager.AddToRoleAsync(user, role);
            return true;
        }
    }
}
