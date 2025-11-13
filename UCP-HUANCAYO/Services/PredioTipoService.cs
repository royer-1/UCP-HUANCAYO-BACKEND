using Microsoft.EntityFrameworkCore;
using UCP_HUANCAYO.Data;
using UCP_HUANCAYO.Dtos.PredioTipo;
using UCP_HUANCAYO.Models;

namespace UCP_HUANCAYO.Services
{
    public class PredioTipoService
    {
        private readonly ApplicationDbContext _context;

        public PredioTipoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PredioTipoViewDto>> GetAllAsync()
        {
            return await _context.PredioTipos
                .Where(t => t.Activo)
                .AsNoTracking()
                .Select(t => new PredioTipoViewDto
                {
                    IdPredioTipo = t.IdPredioTipo,
                    NombreTipo = t.NombreTipo,
                    Contrato = t.Contrato
                })
                .ToListAsync();
        }

        public async Task<PredioTipoViewDto?> GetByIdAsync(Guid id)
        {
            return await _context.PredioTipos
                .Where(t => t.IdPredioTipo == id)
                .AsNoTracking()
                .Select(t => new PredioTipoViewDto
                {
                    IdPredioTipo = t.IdPredioTipo,
                    NombreTipo = t.NombreTipo,
                    Contrato = t.Contrato
                })
                .FirstOrDefaultAsync();
        }

        public async Task<PredioTipoViewDto> CreateAsync(PredioTipoCreateDto dto)
        {
            var tipo = new PredioTipo
            {
                IdPredioTipo = Guid.NewGuid(),
                NombreTipo = dto.NombreTipo,
                Contrato = dto.Contrato,
                IdResponsable = dto.IdResponsable,
                Activo = true
            };

            _context.PredioTipos.Add(tipo);
            await _context.SaveChangesAsync();

            return new PredioTipoViewDto
            {
                IdPredioTipo = tipo.IdPredioTipo,
                NombreTipo = tipo.NombreTipo,
                Contrato = tipo.Contrato
            };
        }

        public async Task<PredioTipoViewDto?> UpdateAsync(Guid id, PredioTipoUpdateDto dto)
        {
            var tipo = await _context.PredioTipos.FindAsync(id);
            if (tipo == null) return null;

            tipo.NombreTipo = dto.NombreTipo;
            tipo.Contrato = dto.Contrato;

            await _context.SaveChangesAsync();

            return new PredioTipoViewDto
            {
                IdPredioTipo = tipo.IdPredioTipo,
                NombreTipo = tipo.NombreTipo,
                Contrato = tipo.Contrato
            };
        }

        public async Task<bool> DesactivarAsync(Guid id)
        {
            var tipo = await _context.PredioTipos.FindAsync(id);
            if (tipo == null) return false;

            tipo.Activo = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
