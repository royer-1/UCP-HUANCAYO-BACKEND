using Microsoft.AspNetCore.Mvc;
using UCP_HUANCAYO.Dtos.PredioImagen;
using UCP_HUANCAYO.Services;

namespace UCP_HUANCAYO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PredioImagenController : ControllerBase
    {
        private readonly PredioImagenService _service;

        public PredioImagenController(PredioImagenService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PredioImagenViewDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PredioImagenViewDto>> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Create(PredioImagenCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            if (result == null) return NotFound("Predio no encontrado.");
            return Ok(new { message = "Imagen agregada correctamente", imagen = result });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, PredioImagenUpdateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (result == null) return NotFound();
            return Ok(new { message = "Imagen actualizada correctamente", imagen = result });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Desactivar(Guid id)
        {
            var success = await _service.DesactivarAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "Imagen desactivada correctamente." });
        }
    }
}
