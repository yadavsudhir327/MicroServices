using Microsoft.AspNetCore.Identity;

namespace MicroServices.AuthApi.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
    }
}
