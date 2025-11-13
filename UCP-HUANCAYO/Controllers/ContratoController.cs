using Microsoft.AspNetCore.Mvc;
using UCP_HUANCAYO.Dtos.Contrato;
using UCP_HUANCAYO.Services;

namespace UCP_HUANCAYO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContratoController : ControllerBase
    {
        private readonly ContratoService _service;

        public ContratoController(ContratoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContratoViewDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ContratoDetalleDto>> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Create(ContratoCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            if (result == null)
                return BadRequest("El predio no existe o es un auditorio. Usa Alquiler en su lugar.");

            return Ok(new { message = "El contrato fue creado correctamente", contrato = result });
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(Guid id, ContratoPatchDto dto)
        {
            var result = await _service.PatchAsync(id, dto);
            if (result == null)
                return BadRequest("Este contrato ya fue pagado completamente y no puede ser editado.");

            return Ok(new { message = "Contrato actualizado parcialmente", contrato = result });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, ContratoUpdateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (result == null)
                return BadRequest("Este contrato ya fue pagado completamente y no puede ser editado.");

            return Ok(new { message = "Contrato actualizado correctamente", contrato = result });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Desactivar(Guid id)
        {
            var success = await _service.DesactivarAsync(id);
            if (!success) return NotFound();

            return Ok(new { message = "Contrato y cronogramas desactivados correctamente." });
        }

        [HttpPost("paged")]
        public async Task<IActionResult> GetPaged(
            [FromBody] ContratoFilterDto filters,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var (items, totalCount) = await _service.GetPagedAsync(page, pageSize, filters);
            return Ok(new { items, totalCount });
        }
    }
}
