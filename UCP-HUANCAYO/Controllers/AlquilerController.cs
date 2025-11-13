using Microsoft.AspNetCore.Mvc;
using UCP_HUANCAYO.Dtos.Alquiler;
using UCP_HUANCAYO.Services;

namespace UCP_HUANCAYO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlquilerController : ControllerBase
    {
        private readonly AlquilerService _service;

        public AlquilerController(AlquilerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlquilerViewDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AlquilerViewDto>> GetById(Guid id)
        {
            var result  = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Create(AlquilerCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(new { message = "El alquiler fue creado correctamente", alquiler = result });
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(Guid id, AlquilerPatchDto dto)
        {
            var result = await _service.PatchAsync(id, dto);
            if (result == null) return NotFound();
            return Ok(new { message = "El alquiler fue actualizado parcialmente", alquiler = result });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, AlquilerUpdateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (result == null) return NotFound();
            return Ok(new { message = "El alquiler fue actualizado correctamente", alquiler = result });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Desactivar(Guid id)
        {
            var success = await _service.DesactivarAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "El alquiler fue desactivado correctamente." });
        }

        [HttpPost("paged")]
        public async Task<IActionResult> GetPaged(
            [FromBody] AlquilerFilterDto filters,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var (items, totalCount) = await _service.GetPagedAsync(page, pageSize, filters);
            return Ok(new { items, totalCount });
        }
    }
}
