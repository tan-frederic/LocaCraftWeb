using Microsoft.AspNetCore.Identity;

namespace LocaCraftAPI.LocaCraftAPI.Data
{
    public class AppUser : IdentityUser
    {
        public string? ProfilePictureBase64 { get; set; }
    }
}
