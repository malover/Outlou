using Microsoft.AspNetCore.Identity;

namespace OutlouTest.Models
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
    }
}
