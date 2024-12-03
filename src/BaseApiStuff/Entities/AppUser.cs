using Microsoft.AspNetCore.Identity;

namespace BaseApiStuff.Entities;

public class AppUser : IdentityUser
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
}