using BibliotecaDevlights.Business.DTOs.Auth;
using BibliotecaDevlights.Business.DTOs.User;

namespace BibliotecaDevlights.Business.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto?> RegisterAsync(RegisterDto request);
        Task<TokenResponseDto?> LoginAsync(LoginDto request);
    }
}
