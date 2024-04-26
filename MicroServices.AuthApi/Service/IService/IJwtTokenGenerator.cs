using MicroServices.AuthApi.Models;

namespace MicroServices.AuthApi.Service.IService
{
    public interface IJwtTokenGenerator
    {
        public string GenerateToken(ApplicationUser applicationUser,IEnumerable<string>roles);
    }
}
