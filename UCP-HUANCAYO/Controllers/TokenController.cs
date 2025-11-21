using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UCP_HUANCAYO.Dtos.Token;
using UCP_HUANCAYO.Services.Auth;

namespace UCP_HUANCAYO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly TokenService _service;

        public TokenController(TokenService service)
        {
            _service = service;
        }

        [Authorize(Policy = "SuperAdministradores")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TokenViewDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [Authorize(Policy = "SuperAdministradores")]
        [HttpGet("{id}")]
        public async Task<ActionResult<TokenViewDto>> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [Authorize(Policy = "SuperAdministradores")]
        [HttpPost]
        public async Task<ActionResult> Create(TokenCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(new { message = "Token creado correctamente", token = result });
        }

        [Authorize(Policy = "SuperAdministradores")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Revocar(Guid id)
        {
            var success = await _service.RevocarAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "Token revocado correctamente." });
        }
    }
}
