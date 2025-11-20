using Microsoft.AspNetCore.Mvc;
using UCP_HUANCAYO.Dtos.Predio;
using UCP_HUANCAYO.Services;

namespace UCP_HUANCAYO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PredioController : ControllerBase
    {
        private readonly PredioService _service;

        public PredioController(PredioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PredioViewDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PredioViewDto>> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Create(PredioCreateDto dto)
        {
            var usuarioActual = Guid.NewGuid();
            var result = await _service.CreateAsync(dto, usuarioActual);
            return Ok(new { message = "El predio fue creado correctamente", predio = result });
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(Guid id, PredioPatchDto dto)
        {
            var usuarioActual = Guid.NewGuid();
            var result = await _service.PatchAsync(id, dto, usuarioActual);
            if (result == null) return NotFound();
            return Ok(new { message = "El predio fue actualizado parcialmente", predio = result });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, PredioUpdateDto dto)
        {
            var usuarioActual = Guid.NewGuid();
            var result = await _service.UpdateAsync(id, dto, usuarioActual);
            if (result == null) return NotFound();
            return Ok(new { message = "El predio fue actualizado correctamente", predio = result });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Desactivar(Guid id)
        {
            var usuarioActual = Guid.NewGuid();
            var success = await _service.DesactivarAsync(id, usuarioActual);
            if (!success) return NotFound();
            return Ok(new { message = "El predio fue desactivado correctamente." });
        }
    }
}