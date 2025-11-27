using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UCP_HUANCAYO.Dtos.Administrado;
using UCP_HUANCAYO.Services;

namespace UCP_HUANCAYO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdministradoController : ControllerBase
    {
        private readonly AdministradoService _service;

        public AdministradoController(AdministradoService service)
        {
            _service = service;
        }

        [Authorize(Policy = "PuedeVerAdministrados")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdministradoViewDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [Authorize(Policy = "PuedeVerAdministrados")]
        [HttpGet("{id}")]
        public async Task<ActionResult<AdministradoViewDto>> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [Authorize(Policy = "SoloGestores")]
        [HttpPost]
        public async Task<ActionResult> Create(AdministradoCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        [Authorize(Policy = "SoloGestores")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(Guid id, AdministradoPatchDto dto)
        {
            var result = await _service.PatchAsync(id, dto);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [Authorize(Policy = "SoloGestores")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, AdministradoUpdateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [Authorize(Policy = "SoloGestores")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Desactivar(Guid id)
        {
            var success = await _service.DesactivarAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "El administrado fue desactivado correctamente." });
        }

        [Authorize(Policy = "SoloGestores")]
        [HttpPost("paged")]
        public async Task<IActionResult> GetPaged(
            [FromBody] AdministradoFilterDto filters,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var (items, totalCount) = await _service.GetPagedAsync(page, pageSize, filters);
            return Ok(new { items, totalCount });
        }
    }
}
