using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UCP_HUANCAYO.Dtos.Auth;
using UCP_HUANCAYO.Dtos.Usuario;
using UCP_HUANCAYO.Services;
using UCP_HUANCAYO.Services.Auth;

namespace UCP_HUANCAYO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _service;
        private readonly AuthService _authService;
        private readonly TokenService _tokenService;

        public UsuarioController(UsuarioService service, AuthService authService, TokenService tokenService)
        {
            _service = service;
            _authService = authService;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginDto dto)
        {
            var usuario = await _service.ValidarCredencialesAsync(dto.Alias, dto.Clave);
            if (usuario == null) return Unauthorized("Credenciales inválidas");

            var (token, expiracion) = _authService.GenerarToken(usuario);
            await _tokenService.RegistrarEmisionAsync(usuario.IdUsuario, expiracion);

            return Ok(new LoginResponseDto
            {
                Token = token,
                Expiracion = expiracion,
                IdUsuario = usuario.IdUsuario,
                Rol = usuario.Rol ?? "supervisor"
            });
        }

        [Authorize(Policy = "SoloAdministradores")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioViewDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [Authorize(Policy = "SoloAdministradores")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioViewDto>> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [Authorize(Policy = "SoloAdministradores")]
        [HttpPost]
        public async Task<ActionResult> Create(UsuarioCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(new { message = "El usuario fue creado correctamente", usuario = result });
        }

        [Authorize(Policy = "SoloAdministradores")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(Guid id, UsuarioPatchDto dto)
        {
            var result = await _service.PatchAsync(id, dto);
            if (result == null) return NotFound();
            return Ok(new { message = "El usuario fue actualizado parcialmente", usuario = result });
        }

        [Authorize(Policy = "SoloAdministradores")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, UsuarioUpdateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (result == null) return NotFound();
            return Ok(new { message = "El usuario se actualizado correctamente", usuario = result });
        }

        [Authorize(Policy = "SoloAdministradores")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Desactivar(Guid id)
        {
            var success = await _service.DesactivarAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "El usuario fue desactivado correctamente." });
        }
    }
}
