using Microsoft.AspNetCore.Mvc;
using UCP_HUANCAYO.Dtos.PredioTipo;
using UCP_HUANCAYO.Services;

namespace UCP_HUANCAYO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PredioTipoController : ControllerBase
    {
        private readonly PredioTipoService _service;

        public PredioTipoController(PredioTipoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PredioTipoViewDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PredioTipoViewDto>> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Create(PredioTipoCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(new { message = "Tipo de predio creado correctamente", tipo = result });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, PredioTipoUpdateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (result == null) return NotFound();
            return Ok(new { message = "El Tipo de predio fue actualizado correctamente", tipo = result });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Desactivar(Guid id)
        {
            var success = await _service.DesactivarAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "El Tipo de Predio fue desactivado correctamente." });
        }
    }
}
