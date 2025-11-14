using Microsoft.AspNetCore.Mvc;
using UCP_HUANCAYO.Dtos.Auditoria;
using UCP_HUANCAYO.Services;

namespace UCP_HUANCAYO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditoriaController : ControllerBase
    {
        private readonly AuditoriaService _service;

        public AuditoriaController(AuditoriaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuditoriaViewDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuditoriaViewDto>> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Create(AuditoriaCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(new { message = "Auditoría registrada correctamente", auditoria = result });
        }
    }
}
