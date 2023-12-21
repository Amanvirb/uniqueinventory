using Microsoft.AspNetCore.Identity;

namespace Domain;
public class AppUser : IdentityUser
{
    public string Fullname { get; set; }
    public string Bio { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
