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
        private readonly UsuarioContextHelper _usuarioContextHelper;

        public PredioImagenService(ApplicationDbContext context, AuditoriaHelper auditoriaHelper, UsuarioContextHelper usuarioContextHelper)
        {
            _context = context;
            _auditoriaHelper = auditoriaHelper;
            _usuarioContextHelper = usuarioContextHelper;
        }

        private static PredioImagenViewDto MapToViewDto(PredioImagen i)
        {
            return new PredioImagenViewDto
            {
                IdImagen = i.IdImagen,
                IdPredio = i.IdPredio,
                Imagen = i.Imagen,
                Activo = i.Activo
            };
        }

        public async Task<List<PredioImagenViewDto>> GetAllAsync()
        {
            return await _context.PredioImagenes
                .AsNoTracking()
                .Select(i => MapToViewDto(i))
                .ToListAsync();
        }

        public async Task<PredioImagenViewDto?> GetByIdAsync(Guid id)
        {
            return await _context.PredioImagenes
                .Where(i => i.IdImagen == id)
                .AsNoTracking()
                .Select(i => MapToViewDto(i))
                .FirstOrDefaultAsync();
        }

        public async Task<PredioImagenViewDto?> CreateAsync(PredioImagenCreateDto dto)
        {
            var usuarioActual = _usuarioContextHelper.GetUsuarioActual();

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

            var detalle = AuditoriaDetalleHelper.GenerarDetalle(imagen, "PredioImagen creada");
            await _auditoriaHelper.RegistrarAsync("predio_imagen", imagen.IdImagen, "INSERT", detalle);

            return MapToViewDto(imagen);
        }

        public async Task<PredioImagenViewDto?> UpdateAsync(Guid id, PredioImagenUpdateDto dto)
        {
            var usuarioActual = _usuarioContextHelper.GetUsuarioActual();

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

            var detalle = AuditoriaDetalleHelper.GenerarCambios(imagenAntes, imagen, "PredioImagen actualizada");
            await _auditoriaHelper.RegistrarAsync("predio_imagen", imagen.IdImagen, "UPDATE", detalle);

            return MapToViewDto(imagen);
        }

        public async Task<bool> DesactivarAsync(Guid id)
        {
            var usuarioActual = _usuarioContextHelper.GetUsuarioActual();

            var imagen = await _context.PredioImagenes.FindAsync(id);
            if (imagen == null) return false;

            imagen.Activo = false;
            await _context.SaveChangesAsync();

            await _auditoriaHelper.RegistrarAsync("predio_imagen", imagen.IdImagen, "DELETE", "PredioImagen desactivada (Activo=false)");
            
            return true;
        }
    }
}
