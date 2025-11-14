using Microsoft.AspNetCore.Mvc;
using UCP_HUANCAYO.Dtos.Usuario;
using UCP_HUANCAYO.Services;

namespace UCP_HUANCAYO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _service;

        public UsuarioController(UsuarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioViewDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioViewDto>> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Create(UsuarioCreateDto dto)
        {
            var usuarioActual = Guid.NewGuid();
            var result = await _service.CreateAsync(dto, usuarioActual);
            return Ok(new { message = "El usuario fue creado correctamente", usuario = result });
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(Guid id, UsuarioPatchDto dto)
        {
            var usuarioActual = Guid.NewGuid();
            var result = await _service.PatchAsync(id, dto, usuarioActual);
            if (result == null) return NotFound();
            return Ok(new { message = "El usuario fue actualizado parcialmente", usuario = result });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, UsuarioUpdateDto dto)
        {
            var usuarioActual = Guid.NewGuid();
            var result = await _service.UpdateAsync(id, dto, usuarioActual);
            if (result == null) return NotFound();
            return Ok(new { message = "El usuario se actualizado correctamente", usuario = result });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Desactivar(Guid id)
        {
            var usuarioActual = Guid.NewGuid();
            var success = await _service.DesactivarAsync(id, usuarioActual);
            if (!success) return NotFound();
            return Ok(new { message = "El usuario fue desactivado correctamente." });
        }
    }
}
