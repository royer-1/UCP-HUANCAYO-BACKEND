using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UCP_HUANCAYO.Data;
using UCP_HUANCAYO.Dtos.CronogramaPago;
using UCP_HUANCAYO.Models;
using UCP_HUANCAYO.Services;

namespace UCP_HUANCAYO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CronogramaPagoController : ControllerBase
    {
        private readonly CronogramaPagoService _service;

        public CronogramaPagoController(CronogramaPagoService service)
        {
            _service = service;
        }

        [Authorize(Policy = "PuedeVerAdministrados")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CronogramaPagoViewDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [Authorize(Policy = "PuedeVerAdministrados")]
        [HttpGet("{id}")]
        public async Task<ActionResult<CronogramaPagoViewDto>> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [Authorize(Policy = "SoloGestores")]
        [HttpPost]
        public async Task<ActionResult> Create(CronogramaPagoCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            if (result == null) return BadRequest("Contrato no encontrado o inactivo.");
            return Ok(result);
        }

        [Authorize(Policy = "SoloGestores")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(Guid id, CronogramaPagoPatchDto dto)
        {
            var result = await _service.PatchAsync(id, dto);
            if (result == null) return BadRequest("Este cronograma ya fue pagado y no puede ser editado.");
            return Ok(result);
        }

        [Authorize(Policy = "SoloGestores")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, CronogramaPagoUpdateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (result == null) return BadRequest("Este cronograma ya fue pagado y no puede ser editado.");
            return Ok(result);
        }

        [Authorize(Policy = "SoloGestores")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Desactivar(Guid id)
        {
            var success = await _service.DesactivarAsync(id);
            if (!success) return NotFound();

            return Ok(new { message = "Cronograma desactivado correctamente." });
        }
    }
}
