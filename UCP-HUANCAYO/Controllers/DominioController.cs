using Microsoft.AspNetCore.Mvc;
using UCP_HUANCAYO.Dtos.Dominio;
using UCP_HUANCAYO.Dtos.Usuario;
using UCP_HUANCAYO.Services;

namespace UCP_HUANCAYO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DominioController : ControllerBase
    {
        private readonly DominioService _service;

        public DominioController(DominioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DominioViewDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DominioViewDto>> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Create(DominioCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(new { message = "El dominio fue creado correctamente", dominio = result });
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(Guid id, DominioPatchDto dto)
        {
            var result = await _service.PatchAsync(id, dto);
            if (result == null) return NotFound();
            return Ok(new { message = "El dominio fue actualizado parcialmente", dominio = result });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, DominioUpdateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (result == null) return NotFound();
            return Ok(new { message = "El dominio se actualizado correctamente", dominio = result });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Desactivar(Guid id)
        {
            var success = await _service.DesactivarAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "El dominio fue desactivado correctamente." });
        }
    }
}
