    using Microsoft.EntityFrameworkCore;
    using System.Text;
    using UCP_HUANCAYO.Data;
    using UCP_HUANCAYO.Dtos.Administrado;
    using UCP_HUANCAYO.Helpers;
    using UCP_HUANCAYO.Models;

    namespace UCP_HUANCAYO.Services
    {
        public class AdministradoService
        {
            private readonly ApplicationDbContext _context;
            private readonly AuditoriaHelper _auditoriaHelper;
            private readonly UsuarioContextHelper _usuarioContextHelper;

            public AdministradoService(ApplicationDbContext context, AuditoriaHelper auditoriaHelper, UsuarioContextHelper usuarioContextHelper)
            {
                _context = context;
                _auditoriaHelper = auditoriaHelper;
                _usuarioContextHelper = usuarioContextHelper;
            }

            private AdministradoViewDto MapToViewDto(Administrado a)
            {
                return new AdministradoViewDto
                {
                    IdAdministrado = a.IdAdministrado,
                    DocIdentTipo = a.DocIdentTipo,
                    DocIdentNro = a.DocIdentNro,
                    RazonSocial = a.RazonSocial,
                    Telefono = a.Telefono,
                    Correo = a.Correo != null ? Encoding.UTF8.GetString(a.Correo) : null,
                    Direccion = a.Direccion,
                    Referencia = a.Referencia,
                    Ubigeo = a.Ubigeo
                };
            }

            public async Task<IEnumerable<AdministradoViewDto>> GetAllAsync()
            {
                return await _context.Administrados
                    .Where(a => a.Activo)
                    .AsNoTracking()
                    .Select(a => MapToViewDto(a))
                    .ToListAsync();
            }

            public async Task<AdministradoViewDto?> GetByIdAsync(Guid id)
            {
                return await _context.Administrados
                    .Where(a => a.IdAdministrado == id)
                    .AsNoTracking()
                    .Select(a => MapToViewDto(a))
                    .FirstOrDefaultAsync();
            }

            public async Task<AdministradoViewDto> CreateAsync(AdministradoCreateDto dto)
            {
                var usuarioActual = _usuarioContextHelper.GetUsuarioActual();

                var administrado = new Administrado
                {
                    IdAdministrado = Guid.NewGuid(),
                    DocIdentTipo = dto.DocIdentTipo,
                    DocIdentNro = dto.DocIdentNro,
                    RazonSocial = dto.RazonSocial,
                    Telefono = dto.Telefono,
                    Correo = string.IsNullOrWhiteSpace(dto.Correo) ? null : Encoding.UTF8.GetBytes(dto.Correo),
                    Direccion = dto.Direccion,
                    Referencia = string.IsNullOrWhiteSpace(dto.Referencia) ? null : dto.Referencia,
                    Ubigeo = dto.Ubigeo,
                    Activo = true,
                    IdResponsable = usuarioActual
                };

                _context.Administrados.Add(administrado);
                await _context.SaveChangesAsync();

                var detalle = AuditoriaDetalleHelper.GenerarDetalle(administrado, "Administrado creado");
                await _auditoriaHelper.RegistrarAsync("administrado", administrado.IdAdministrado, "INSERT", detalle);

                return MapToViewDto(administrado);
            }

            public async Task<AdministradoViewDto?> PatchAsync(Guid id, AdministradoPatchDto dto)
            {
                var usuarioActual = _usuarioContextHelper.GetUsuarioActual();

                var administrado = await _context.Administrados.FindAsync(id);
                if (administrado == null) return null;

                var administradoAntes = new Administrado
                {
                    IdAdministrado = administrado.IdAdministrado,
                    DocIdentTipo = administrado.DocIdentTipo,
                    DocIdentNro = administrado.DocIdentNro,
                    RazonSocial = administrado.RazonSocial,
                    Telefono = administrado.Telefono,
                    Correo = administrado.Correo,
                    Direccion = administrado.Direccion,
                    Referencia = administrado.Referencia,
                    Ubigeo = administrado.Ubigeo,
                    IdResponsable = administrado.IdResponsable,
                    Activo = administrado.Activo
                };

                if (dto.RazonSocial != null) administrado.RazonSocial = dto.RazonSocial;
                if (dto.Telefono != null) administrado.Telefono = dto.Telefono;
                if (dto.Correo != null) administrado.Correo = Encoding.UTF8.GetBytes(dto.Correo);
                if (dto.Direccion != null) administrado.Direccion = dto.Direccion;
                if (dto.Referencia != null) administrado.Referencia = dto.Referencia;
                if (dto.Ubigeo != null) administrado.Ubigeo = dto.Ubigeo;

                administrado.IdResponsable = usuarioActual;
                await _context.SaveChangesAsync();

                var detalle = AuditoriaDetalleHelper.GenerarCambios(administradoAntes!, administrado, "Administrado actualizado parcialmente");
                await _auditoriaHelper.RegistrarAsync("administrado", administrado.IdAdministrado, "PATCH", detalle);

                return MapToViewDto(administrado);
            }

            public async Task<AdministradoViewDto?> UpdateAsync(Guid id, AdministradoUpdateDto dto)
            {
                var usuarioActual = _usuarioContextHelper.GetUsuarioActual();

                var administrado = await _context.Administrados.FindAsync(id);
                if (administrado == null) return null;

                var administradoAntes = new Administrado
                {
                    IdAdministrado = administrado.IdAdministrado,
                    DocIdentTipo = administrado.DocIdentTipo,
                    DocIdentNro = administrado.DocIdentNro,
                    RazonSocial = administrado.RazonSocial,
                    Telefono = administrado.Telefono,
                    Correo = administrado.Correo,
                    Direccion = administrado.Direccion,
                    Referencia = administrado.Referencia,
                    Ubigeo = administrado.Ubigeo,
                    IdResponsable = administrado.IdResponsable,
                    Activo = administrado.Activo
                };

                administrado.DocIdentTipo = dto.DocIdentTipo;
                administrado.DocIdentNro = dto.DocIdentNro;
                administrado.RazonSocial = dto.RazonSocial;
                administrado.Telefono = dto.Telefono;
                administrado.Correo = string.IsNullOrWhiteSpace(dto.Correo) ? null : Encoding.UTF8.GetBytes(dto.Correo);
                administrado.Direccion = dto.Direccion;
                administrado.Referencia = string.IsNullOrWhiteSpace(dto.Referencia) ? null : dto.Referencia;
                administrado.Ubigeo = dto.Ubigeo;
                administrado.IdResponsable = usuarioActual;

                await _context.SaveChangesAsync();

                var detalle = AuditoriaDetalleHelper.GenerarCambios(administradoAntes, administrado, "Administrado actualizado");
                await _auditoriaHelper.RegistrarAsync("administrado", administrado.IdAdministrado, "UPDATE", detalle);

                return MapToViewDto(administrado);
        }

            public async Task<bool> DesactivarAsync(Guid id)
            {
                var usuarioActual = _usuarioContextHelper.GetUsuarioActual();

                var administrado = await _context.Administrados.FindAsync(id);
                if (administrado == null) return false;

                administrado.Activo = false;
                administrado.IdResponsable = usuarioActual;
                await _context.SaveChangesAsync();

                await _auditoriaHelper.RegistrarAsync("administrado", administrado.IdAdministrado, "DELETE", "Administrado desactivado (Activo=false)");
                
                return true;
            }

            public async Task<(List<AdministradoFilterDto> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, AdministradoFilterDto filters)
            {
                var query = _context.Administrados
                    .Where(a => a.Activo)
                    .AsNoTracking();

                if (!string.IsNullOrWhiteSpace(filters.SearchTerm))
                {
                    query = query.Where(a =>
                        a.RazonSocial.Contains(filters.SearchTerm) ||
                        a.DocIdentNro.Contains(filters.SearchTerm) ||
                        a.Direccion.Contains(filters.SearchTerm) ||
                        a.Ubigeo.Contains(filters.SearchTerm));
                }

                if (!string.IsNullOrWhiteSpace(filters.DocIdentTipo))
                    query = query.Where(a => a.DocIdentTipo.Contains(filters.DocIdentTipo));

                if (!string.IsNullOrWhiteSpace(filters.DocIdentNro))
                    query = query.Where(a => a.DocIdentNro.Contains(filters.DocIdentNro));

                if (!string.IsNullOrWhiteSpace(filters.RazonSocial))
                    query = query.Where(a => a.RazonSocial.Contains(filters.RazonSocial));

                if (!string.IsNullOrWhiteSpace(filters.Telefono))
                    query = query.Where(a => a.Telefono.Contains(filters.Telefono));

                if (!string.IsNullOrWhiteSpace(filters.Direccion))
                    query = query.Where(a => a.Direccion.Contains(filters.Direccion));

                if (!string.IsNullOrWhiteSpace(filters.Ubigeo))
                    query = query.Where(a => a.Ubigeo.Contains(filters.Ubigeo));

                var filtered = await query.ToListAsync();

                if (!string.IsNullOrWhiteSpace(filters.Correo))
                {
                    filtered = filtered.Where(a =>
                        a.Correo != null &&
                        Encoding.UTF8.GetString(a.Correo).Contains(filters.Correo)).ToList();
                }


                var totalCount = filtered.Count;

                var items = filtered
                    .OrderBy(a => a.RazonSocial)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(a => new AdministradoFilterDto
                    {
                        DocIdentTipo = a.DocIdentTipo,
                        DocIdentNro = a.DocIdentNro,
                        RazonSocial = a.RazonSocial,
                        Telefono = a.Telefono,
                        Correo = a.Correo != null ? Encoding.UTF8.GetString(a.Correo) : null,
                        Direccion = a.Direccion,
                        Referencia = a.Referencia,
                        Ubigeo = a.Ubigeo,
                    })
                    .ToList();

                return (items, totalCount);
            }
        }
    }
