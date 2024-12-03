using BaseApiStuff.Models.User;
using Microsoft.AspNetCore.Identity;

namespace BaseApiStuff.Services;

public interface IAuthService
{
    Task<IdentityResult> RegisterUserAsync(RegisterUserDto userDto, CancellationToken cancellationToken = default);
    Task<AuthResponse> LoginUserAsync(LoginUserDto userDto, CancellationToken cancellationToken = default);
}