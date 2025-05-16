using Microsoft.AspNetCore.Identity;

namespace Restoran.Models
{
    public class AppUser:IdentityUser
    {
        public string Name { get; set; }
    }
}
