using BibliotecaDevlights.Business.DTOs.Auth;
using BibliotecaDevlights.Business.DTOs.User;
using BibliotecaDevlights.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaDevlights.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(LoginDto request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "Las credenciales son requeridas" });
            }

            var token = await _authService.LoginAsync(request);
            if (token == null)
            {
                return Unauthorized(new { message = "Email o contraseña inválidos" });
            }

            return Ok(token);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "Los datos del usuario son requeridos" });
            }

            var user = await _authService.RegisterAsync(request);
            if (user == null)
            {
                return BadRequest(new { message = "El email o nombre de usuario ya existe" });
            }

            return Created(nameof(Register), user);
        }
    }
}
