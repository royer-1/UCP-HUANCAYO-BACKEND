using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UCP_HUANCAYO.Data;
using UCP_HUANCAYO.Dtos.Predio;
using UCP_HUANCAYO.Helpers;
using UCP_HUANCAYO.Models;

namespace UCP_HUANCAYO.Services
{
    public class PredioService
    {
        private readonly ApplicationDbContext _context;
        private readonly AuditoriaHelper _auditoriaHelper;
        private readonly UsuarioContextHelper _usuarioContextHelper;

        public PredioService(ApplicationDbContext context, AuditoriaHelper auditoriaHelper, UsuarioContextHelper usuarioContextHelper)
        {
            _context = context;
            _auditoriaHelper = auditoriaHelper;
            _usuarioContextHelper = usuarioContextHelper;
        }

        private PredioViewDto MapToViewDto(Predio p)
        {
            return new PredioViewDto
            {
                IdPredio = p.IdPredio,
                IdPredioTipo = p.IdPredioTipo,
                NombrePredio = p.NombrePredio,
                NombreTipo = p.PredioTipo?.NombreTipo,
                Descripcion = p.Descripcion,
                AreaPredio = p.AreaPredio,
                Capacidad = p.Capacidad,
                RegistroAgua = p.RegistroAgua,
                RegistroLuz = p.RegistroLuz,
                Direccion = p.Direccion,
                Ubigeo = p.Ubigeo,
                Latitud = p.Latitud,
                Longitud = p.Longitud,
                Imagenes = _context.PredioImagenes
                    .Where(i => i.IdPredio == p.IdPredio && i.Activo)
                    .Select(i => i.Imagen!)
                    .ToList()
            };
        }

        public async Task<IEnumerable<PredioViewDto>> GetAllAsync()
        {
            return await _context.Predios
                .Include(p => p.PredioTipo)
                .Where(p => p.Activo)
                .AsNoTracking()
                .Select(p => MapToViewDto(p))
                .ToListAsync();
        }

        public async Task<PredioViewDto?> GetByIdAsync(Guid id)
        {
            return await _context.Predios
                .Include(p => p.PredioTipo)
                .Where(p => p.IdPredio == id)
                .AsNoTracking()
                .Select(p => MapToViewDto(p))
                .FirstOrDefaultAsync();
        }

        public async Task<PredioViewDto> CreateAsync(PredioCreateDto dto)
        {
            var usuarioActual = _usuarioContextHelper.GetUsuarioActual();

            var predio = new Predio
            {
                IdPredio = Guid.NewGuid(),
                IdPredioTipo = dto.IdPredioTipo,
                NombrePredio = dto.NombrePredio,
                Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? null : dto.Descripcion,
                AreaPredio = dto.AreaPredio,
                Capacidad = dto.Capacidad,
                RegistroAgua = dto.RegistroAgua,
                RegistroLuz = dto.RegistroLuz,
                Direccion = dto.Direccion,
                Ubigeo = dto.Ubigeo,
                Latitud = string.IsNullOrWhiteSpace(dto.Latitud) ? null : dto.Latitud,
                Longitud = string.IsNullOrWhiteSpace(dto.Longitud) ? null : dto.Longitud,
                IdResponsable = usuarioActual,
                Activo = true
            };

            _context.Predios.Add(predio);

            if (dto.ImagenesPredio != null && dto.ImagenesPredio.Any())
            {
                foreach (var img in dto.ImagenesPredio)
                {
                    var imagen = new PredioImagen
                    {
                        IdImagen = Guid.NewGuid(),
                        IdPredio = predio.IdPredio,
                        Imagen = img,
                        Activo = true
                    };
                    _context.PredioImagenes.Add(imagen);
                }
            }

            await _context.SaveChangesAsync();

            var detalle = AuditoriaDetalleHelper.GenerarDetalle(predio, "Predio creado");
            await _auditoriaHelper.RegistrarAsync("predio", predio.IdPredio, "INSERT", detalle);

            return MapToViewDto(predio);
        }

        public async Task<PredioViewDto?> PatchAsync(Guid id, PredioPatchDto dto)
        {
            var usuarioActual = _usuarioContextHelper.GetUsuarioActual();

            var predio = await _context.Predios.FindAsync(id);
            if (predio == null) return null;

            var predioAntes = new Predio
            {
                IdPredio = predio.IdPredio,
                IdPredioTipo = predio.IdPredioTipo,
                NombrePredio = predio.NombrePredio,
                Descripcion = predio.Descripcion,
                AreaPredio = predio.AreaPredio,
                Capacidad = predio.Capacidad,
                RegistroAgua = predio.RegistroAgua,
                RegistroLuz = predio.RegistroLuz,
                Direccion = predio.Direccion,
                Ubigeo = predio.Ubigeo,
                Latitud = predio.Latitud,
                Longitud = predio.Longitud,
                Activo = predio.Activo,
                IdResponsable = predio.IdResponsable
            };

            if (dto.NombrePredio != null) predio.NombrePredio = dto.NombrePredio;
            if (dto.Descripcion != null) predio.Descripcion = dto.Descripcion;
            if (dto.AreaPredio.HasValue) predio.AreaPredio = dto.AreaPredio.Value;
            if (dto.Capacidad.HasValue) predio.Capacidad = dto.Capacidad.Value;
            if (dto.RegistroAgua.HasValue) predio.RegistroAgua = dto.RegistroAgua.Value;
            if (dto.RegistroLuz.HasValue) predio.RegistroLuz = dto.RegistroLuz.Value;
            if (dto.Direccion != null) predio.Direccion = dto.Direccion;
            if (dto.Ubigeo != null) predio.Ubigeo = dto.Ubigeo;
            if (dto.Latitud != null) predio.Latitud = dto.Latitud;
            if (dto.Longitud != null) predio.Longitud = dto.Longitud;

            if (dto.ImagenesPredio != null)
            {
                var imagenesAnteriores = _context.PredioImagenes.Where(i => i.IdPredio == id);
                _context.PredioImagenes.RemoveRange(imagenesAnteriores);

                foreach (var img in dto.ImagenesPredio)
                {
                    if (!string.IsNullOrWhiteSpace(img))
                    {
                        var imagen = new PredioImagen
                        {
                            IdImagen = Guid.NewGuid(),
                            IdPredio = id,
                            Imagen = img,
                            Activo = true
                        };
                        _context.PredioImagenes.Add(imagen);
                    }
                }
            }

            predio.IdResponsable = usuarioActual;
            await _context.SaveChangesAsync();

            var detalle = AuditoriaDetalleHelper.GenerarCambios(predioAntes, predio, "Predio actualizado parcialmente");
            await _auditoriaHelper.RegistrarAsync("predio", predio.IdPredio, "PATCH", detalle);

            return MapToViewDto(predio);
        }

        public async Task<PredioViewDto?> UpdateAsync(Guid id, PredioUpdateDto dto)
        {
            var usuarioActual = _usuarioContextHelper.GetUsuarioActual();

            var predio = await _context.Predios.FindAsync(id);
            if (predio == null) return null;

            var predioAntes = new Predio
            {
                IdPredio = predio.IdPredio,
                IdPredioTipo = predio.IdPredioTipo,
                NombrePredio = predio.NombrePredio,
                Descripcion = predio.Descripcion,
                AreaPredio = predio.AreaPredio,
                Capacidad = predio.Capacidad,
                RegistroAgua = predio.RegistroAgua,
                RegistroLuz = predio.RegistroLuz,
                Direccion = predio.Direccion,
                Ubigeo = predio.Ubigeo,
                Latitud = predio.Latitud,
                Longitud = predio.Longitud,
                Activo = predio.Activo,
                IdResponsable = predio.IdResponsable
            };

            predio.NombrePredio = dto.NombrePredio;
            predio.Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? null : dto.Descripcion;
            predio.AreaPredio = dto.AreaPredio;
            predio.Capacidad = dto.Capacidad;
            predio.RegistroAgua = dto.RegistroAgua;
            predio.RegistroLuz = dto.RegistroLuz;
            predio.Direccion = dto.Direccion;
            predio.Ubigeo = dto.Ubigeo;
            predio.Latitud = string.IsNullOrWhiteSpace(dto.Latitud) ? null : dto.Latitud;
            predio.Longitud = string.IsNullOrWhiteSpace(dto.Longitud) ? null : dto.Longitud;

            var imagenesAnteriores = _context.PredioImagenes.Where(i => i.IdPredio == id);
            _context.PredioImagenes.RemoveRange(imagenesAnteriores);

            if (dto.ImagenesPredio != null && dto.ImagenesPredio.Any())
            {
                foreach (var img in dto.ImagenesPredio)
                {
                    if (!string.IsNullOrWhiteSpace(img))
                    {
                        var imagen = new PredioImagen
                        {
                            IdImagen = Guid.NewGuid(),
                            IdPredio = id,
                            Imagen = img,
                            Activo = true
                        };
                        _context.PredioImagenes.Add(imagen);
                    }
                }
            }

            predio.IdResponsable = usuarioActual;
            await _context.SaveChangesAsync();

            var detalle = AuditoriaDetalleHelper.GenerarCambios(predioAntes, predio, "Predio actualizado");
            await _auditoriaHelper.RegistrarAsync("predio", predio.IdPredio, "UPDATE", detalle);

            return MapToViewDto(predio);
        }

        public async Task<bool> DesactivarAsync(Guid id)
        {
            var usuarioActual = _usuarioContextHelper.GetUsuarioActual();

            var predio = await _context.Predios
                .Include(p => p.Imagenes)
                .FirstOrDefaultAsync(p => p.IdPredio == id);

            if (predio == null) return false;

            predio.Activo = false;
            predio.IdResponsable = usuarioActual;

            foreach (var imagen in predio.Imagenes)
            {
                imagen.Activo = false;
            }

            await _context.SaveChangesAsync();

            await _auditoriaHelper.RegistrarAsync(
                "predio",
                predio.IdPredio,
                "DELETE",
                "Predio desactivado (Activo=false, imágenes desactivadas)"
            );

            return true;
        }
    }
}

