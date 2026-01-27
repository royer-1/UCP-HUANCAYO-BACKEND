using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

        [Authorize(Policy = "PuedeVerAdministrados")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PredioViewDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [Authorize(Policy = "PuedeVerAdministrados")]
        [HttpGet("{id}")]
        public async Task<ActionResult<PredioViewDto>> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [Authorize(Policy = "SoloGestores")]
        [HttpPost]
        public async Task<ActionResult> Create(PredioCreateDto dto)
        {
            try
            {
                if (dto.ImagenesPredio != null)
                {
                    foreach (var base64 in dto.ImagenesPredio)
                    {
                        if (string.IsNullOrWhiteSpace(base64))
                        {
                            return BadRequest(new { error = "Una de las imágenes está vacía o mal formada." });
                        }
                    }
                }

                var result = await _service.CreateAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [Authorize(Policy = "SoloGestores")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(Guid id, PredioPatchDto dto)
        {
            var result = await _service.PatchAsync(id, dto);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [Authorize(Policy = "SoloGestores")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, PredioUpdateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [Authorize(Policy = "SoloGestores")]
        [HttpPatch("{id}/desactivar")]
        public async Task<IActionResult> Desactivar(Guid id)
        {
            var success = await _service.DesactivarAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "El predio fue desactivado correctamente." });
        }
    }
}