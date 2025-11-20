using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UCP_HUANCAYO.Data;
using UCP_HUANCAYO.Dtos.PredioImagen;
using UCP_HUANCAYO.Helpers;
using UCP_HUANCAYO.Models;

namespace UCP_HUANCAYO.Services
{
    public class PredioImagenService
    {
        private readonly ApplicationDbContext _context;
        private readonly AuditoriaHelper _auditoriaHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PredioImagenService(ApplicationDbContext context, AuditoriaHelper auditoriaHelper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _auditoriaHelper = auditoriaHelper;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<PredioImagenViewDto?> CreateAsync(PredioImagenCreateDto dto, Guid usuarioActual)
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

            var context = _httpContextAccessor.HttpContext!;
            var detalle = AuditoriaDetalleHelper.GenerarDetalle(imagen, "PredioImagen creada");

            await _auditoriaHelper.RegistrarDesdeContextoAsync(
                tabla: "predio_imagen",
                idRegistro: imagen.IdImagen,
                accion: "INSERT",
                detalle: detalle,
                idUsuario: usuarioActual,
                context: context
            );

            return new PredioImagenViewDto
            {
                IdImagen = imagen.IdImagen,
                IdPredio = imagen.IdPredio,
                Imagen = imagen.Imagen,
                Activo = imagen.Activo
            };
        }

        public async Task<PredioImagenViewDto?> UpdateAsync(Guid id, PredioImagenUpdateDto dto, Guid usuarioActual)
        {
            var imagen = await _context.PredioImagenes.FindAsync(id);
            if (imagen == null) return null;

            var imagenAntes = new PredioImagen
            {
                IdImagen = imagen.IdImagen,
                IdPredio = imagen.IdPredio,
                Imagen = imagen.Imagen,
                Activo = imagen.Activo
            };

            imagen.Imagen = dto.Imagen;
            await _context.SaveChangesAsync();

            var context = _httpContextAccessor.HttpContext!;
            var detalle = AuditoriaDetalleHelper.GenerarCambios(imagenAntes, imagen, "PredioImagen actualizada");

            await _auditoriaHelper.RegistrarDesdeContextoAsync(
                tabla: "predio_imagen",
                idRegistro: imagen.IdImagen,
                accion: "UPDATE",
                detalle: detalle,
                idUsuario: usuarioActual,
                context: context
            );

            return new PredioImagenViewDto
            {
                IdImagen = imagen.IdImagen,
                IdPredio = imagen.IdPredio,
                Imagen = imagen.Imagen,
                Activo = imagen.Activo
            };
        }

        public async Task<bool> DesactivarAsync(Guid id, Guid usuarioActual)
        {
            var imagen = await _context.PredioImagenes.FindAsync(id);
            if (imagen == null) return false;

            imagen.Activo = false;
            await _context.SaveChangesAsync();

            var context = _httpContextAccessor.HttpContext!;
            await _auditoriaHelper.RegistrarDesdeContextoAsync(
                tabla: "predio_imagen",
                idRegistro: imagen.IdImagen,
                accion: "DELETE",
                detalle: "PredioImagen desactivada (Activo=false)",
                idUsuario: usuarioActual,
                context: context
            );

            return true;
        }
    }
}
