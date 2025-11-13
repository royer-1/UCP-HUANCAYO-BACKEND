using Microsoft.EntityFrameworkCore;
using UCP_HUANCAYO.Data;
using UCP_HUANCAYO.Dtos.PredioImagen;
using UCP_HUANCAYO.Models;

namespace UCP_HUANCAYO.Services
{
    public class PredioImagenService
    {
        private readonly ApplicationDbContext _context;

        public PredioImagenService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PredioImagenViewDto>> GetAllAsync()
        {
            return await _context.PredioImagenes
                .AsNoTracking()
                .Select(i => new PredioImagenViewDto
                {
                    IdImagen = i.IdImagen,
                    IdPredio = i.IdPredio,
                    Imagen = i.Imagen,
                    Activo = i.Activo
                })
                .ToListAsync();
        }

        public async Task<PredioImagenViewDto?> GetByIdAsync(Guid id)
        {
            return await _context.PredioImagenes
                .Where(i => i.IdImagen == id)
                .AsNoTracking()
                .Select(i => new PredioImagenViewDto
                {
                    IdImagen = i.IdImagen,
                    IdPredio = i.IdPredio,
                    Imagen = i.Imagen,
                    Activo = i.Activo
                })
                .FirstOrDefaultAsync();
        }

        public async Task<PredioImagenViewDto?> CreateAsync(PredioImagenCreateDto dto)
        {
            var predio = await _context.Predios.FindAsync(dto.IdPredio);
            if (predio == null) return null;

            var imagen = new PredioImagen
            {
                IdImagen = Guid.NewGuid(),
                IdPredio = dto.IdPredio,
                Imagen = dto.Imagen,
                Activo = true
            };

            _context.PredioImagenes.Add(imagen);
            await _context.SaveChangesAsync();

            return new PredioImagenViewDto
            {
                IdImagen = imagen.IdImagen,
                IdPredio = imagen.IdPredio,
                Imagen = imagen.Imagen,
                Activo = imagen.Activo
            };
        }

        public async Task<PredioImagenViewDto?> UpdateAsync(Guid id, PredioImagenUpdateDto dto)
        {
            var imagen = await _context.PredioImagenes.FindAsync(id);
            if (imagen == null) return null;

            imagen.Imagen = dto.Imagen;
            await _context.SaveChangesAsync();

            return new PredioImagenViewDto
            {
                IdImagen = imagen.IdImagen,
                IdPredio = imagen.IdPredio,
                Imagen = imagen.Imagen,
                Activo = imagen.Activo
            };
        }

        public async Task<bool> DesactivarAsync(Guid id)
        {
            var imagen = await _context.PredioImagenes.FindAsync(id);
            if (imagen == null) return false;

            imagen.Activo = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
