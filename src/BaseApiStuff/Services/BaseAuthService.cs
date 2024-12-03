using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BaseApiStuff.Models.User;
using BaseApiStuff.Statics;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace BaseApiStuff.Services;

public class BaseAuthService<TUser>(
    ILogger<BaseAuthService<TUser>> logger,
    UserManager<TUser> userManager,
    IConfiguration configuration) : IAuthService where TUser : IdentityUser
{
    public async Task<IdentityResult> RegisterUserAsync(RegisterUserDto userDto,
        CancellationToken cancellationToken = default)
    {
        var user = new IdentityUser
        {
            UserName = userDto.UserName,
            Email = userDto.Email
        };

        return await userManager.CreateAsync((TUser)user, userDto.Password);
    }

    public async Task<AuthResponse> LoginUserAsync(LoginUserDto userDto, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Login attempt for {userDto.Email}", userDto.Email);

        var user = await userManager.FindByEmailAsync(userDto.Email);
        var passwordValid = user != null && await userManager.CheckPasswordAsync(user, userDto.Password);
        if (!passwordValid) return null;

        var tokenString = await GenerateToken(user);

        var response = new AuthResponse
        {
            Email = user.Email,
            Token = tokenString,
            UserId = user.Id
        };

        return response;
    }

    private async Task<string> GenerateToken(IdentityUser user)
    {
        ArgumentNullException.ThrowIfNull(user);
        var securityKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SigningKey"] ??
                                                            throw new InvalidOperationException()));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var roles = await userManager.GetRolesAsync((TUser)user);
        var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();

        var userClaims = await userManager.GetClaimsAsync((TUser)user);

        var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.UserName!),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email!),
                new(AuthConstants.ClaimTypes.ClaimTypeUid, user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

        var token = new JwtSecurityToken(configuration["JwtSettings:Issuer"],
            configuration["JwtSettings:Audience"],
            claims,
            expires: DateTime.UtcNow.AddHours(Convert.ToInt32(configuration["JwtSettings:Duration"])),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}