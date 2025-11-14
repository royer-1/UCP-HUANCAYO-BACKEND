using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Text;
using UCP_HUANCAYO.Data;
using UCP_HUANCAYO.Dtos.Usuario;
using UCP_HUANCAYO.Helpers;
using UCP_HUANCAYO.Models;

namespace UCP_HUANCAYO.Services
{
    public class UsuarioService
    {
        private readonly ApplicationDbContext _context;
        private readonly AuditoriaHelper _auditoriaHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioService(ApplicationDbContext context, AuditoriaHelper auditoriaHelper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _auditoriaHelper = auditoriaHelper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<UsuarioViewDto>> GetAllAsync()
        {
            return await _context.Usuarios
                .Include(u => u.Dominio)
                .Where(u => u.Activo)
                .AsNoTracking()
                .Select(u => new UsuarioViewDto
                {
                    IdUsuario = u.IdUsuario,
                    IdDominio = u.IdDominio,
                    NombreDominio = u.Dominio != null ? u.Dominio.Nombre : null,
                    Alias = u.Alias,
                    DocIdentTipo = u.DocIdentTipo,
                    DocIdentNro = u.DocIdentNro,
                    Nombres = u.Nombres,
                    Correo = u.Correo,
                    Telefono = u.Telefono,
                    Clave = u.Clave != null ? Encoding.UTF8.GetString(u.Clave) : null,
                })
                .ToListAsync();
        }

        public async Task<UsuarioViewDto?> GetByIdAsync(Guid id)
        {
            return await _context.Usuarios
                .Include(u => u.Dominio)
                .Where(u => u.Activo)
                .AsNoTracking()
                .Select(u => new UsuarioViewDto
                {
                    IdUsuario = u.IdUsuario,
                    IdDominio = u.IdDominio,
                    NombreDominio = u.Dominio != null ? u.Dominio.Nombre : null,
                    Alias = u.Alias,
                    DocIdentTipo = u.DocIdentTipo,
                    DocIdentNro = u.DocIdentNro,
                    Nombres = u.Nombres,
                    Correo = u.Correo,
                    Telefono = u.Telefono,
                    Clave = u.Clave != null ? Encoding.UTF8.GetString(u.Clave) : null,
                })
                .FirstOrDefaultAsync();
        }

        public async Task<UsuarioViewDto> CreateAsync(UsuarioCreateDto dto, Guid usuarioActual)
        {
            var usuario = new Usuario
            {
                IdUsuario = Guid.NewGuid(),
                IdDominio = dto.IdDominio,
                Alias = dto.Alias,
                DocIdentTipo = dto.DocIdentTipo,
                DocIdentNro = dto.DocIdentNro,
                Nombres = dto.Nombres,
                Correo = dto.Correo,
                Telefono = dto.Telefono,
                Clave = string.IsNullOrWhiteSpace(dto.Clave) ? null : Encoding.UTF8.GetBytes(dto.Clave),
                Activo = true,
                IdResponsable = dto.IdResponsable
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            var context = _httpContextAccessor.HttpContext!;

            await _auditoriaHelper.RegistrarDesdeContextoAsync(
                tabla: "usuario",
                idRegistro: usuario.IdUsuario,
                accion: "INSERT",
                detalle: $"Usuario creado: Alias={usuario.Alias}, DocTipo={usuario.DocIdentTipo}, DocNro={usuario.DocIdentNro}, Nombres={usuario.Nombres}, Correo={usuario.Correo}, Telefono={usuario.Telefono}",
                idUsuario: usuarioActual,
                context: context
            );

            return new UsuarioViewDto
            {
                IdUsuario = usuario.IdUsuario,
                Alias = usuario.Alias,
                DocIdentTipo = usuario.DocIdentTipo,
                DocIdentNro = usuario.DocIdentNro,
                Nombres = usuario.Nombres,
                Correo = usuario.Correo,
                Telefono = usuario.Telefono,
                Clave = usuario.Clave != null ? Encoding.UTF8.GetString(usuario.Clave) : null
            };
        }

        public async Task<UsuarioViewDto?> PatchAsync(Guid id, UsuarioPatchDto dto, Guid usuarioActual)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return null;

            var cambios = new List<string>();

            if (dto.Alias != null)
            {
                cambios.Add($"Alias: {usuario.Alias} → {dto.Alias}");
                usuario.Alias = dto.Alias;
            }
            if (dto.Nombres != null)
            {
                cambios.Add($"Nombres: {usuario.Nombres} → {dto.Nombres}");
                usuario.Nombres = dto.Nombres;
            }
            if (dto.Correo != null)
            {
                cambios.Add($"Correo: {usuario.Correo} → {dto.Correo}");
                usuario.Correo = dto.Correo;
            }
            if (dto.Telefono != null)
            {
                cambios.Add($"Telefono: {usuario.Telefono} → {dto.Telefono}");
                usuario.Telefono = dto.Telefono;
            }
            if (dto.Clave != null)
            {
                cambios.Add("Clave: actualizada");
                usuario.Clave = Encoding.UTF8.GetBytes(dto.Clave);
            }

            await _context.SaveChangesAsync();

            var context = _httpContextAccessor.HttpContext!;

            await _auditoriaHelper.RegistrarDesdeContextoAsync(
                tabla: "usuario",
                idRegistro: usuario.IdUsuario,
                accion: "PATCH",
                detalle: string.Join(", ", cambios),
                idUsuario: usuarioActual,
                context: context
            );

            return new UsuarioViewDto
            {
                IdUsuario = usuario.IdUsuario,
                Alias = usuario.Alias,
                DocIdentTipo = usuario.DocIdentTipo,
                DocIdentNro = usuario.DocIdentNro,
                Nombres = usuario.Nombres,
                Correo = usuario.Correo,
                Telefono = usuario.Telefono,
                Clave = usuario.Clave != null ? Encoding.UTF8.GetString(usuario.Clave) : null
            };
        }

        public async Task<UsuarioViewDto?> UpdateAsync(Guid id, UsuarioUpdateDto dto, Guid usuarioActual)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if(usuario == null) return null;

            usuario.Alias = dto.Alias;
            usuario.DocIdentTipo = dto.DocIdentTipo;
            usuario.DocIdentNro = dto.DocIdentNro;
            usuario.Nombres = dto.Nombres;
            usuario.Correo = dto.Correo;
            usuario.Telefono = dto.Telefono;
            usuario.Clave = string.IsNullOrWhiteSpace(dto.Clave) ? null : Encoding.UTF8.GetBytes(dto.Clave);

            await _context.SaveChangesAsync();

            var context = _httpContextAccessor.HttpContext!;

            await _auditoriaHelper.RegistrarDesdeContextoAsync(
                tabla: "usuario",
                idRegistro: usuario.IdUsuario,
                accion: "UPDATE",
                detalle: $"Usuario actualizado: Alias={usuario.Alias}, DocTipo={usuario.DocIdentTipo}, DocNro={usuario.DocIdentNro}, Nombres={usuario.Nombres}, Correo={usuario.Correo}, Telefono={usuario.Telefono}",
                idUsuario: usuarioActual,
                context: context
            );

            return new UsuarioViewDto
            {
                IdUsuario = usuario.IdUsuario,
                Alias = usuario.Alias,
                DocIdentTipo = usuario.DocIdentTipo,
                DocIdentNro = usuario.DocIdentNro,
                Nombres = usuario.Nombres,
                Correo = usuario.Correo,
                Telefono = usuario.Telefono,
                Clave = usuario.Clave != null ? Encoding.UTF8.GetString(usuario.Clave) : null
            };
        }

        public async Task<bool> DesactivarAsync(Guid id, Guid usuarioActual)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return false;

            usuario.Activo = false;
            await _context.SaveChangesAsync();

            var context = _httpContextAccessor.HttpContext!;

            await _auditoriaHelper.RegistrarDesdeContextoAsync(
                tabla: "usuario",
                idRegistro: usuario.IdUsuario,
                accion: "DELETE",
                detalle: "Usuario desactivado (Activo=false)",
                idUsuario: usuarioActual,
                context: context
            );

            return true;
        }
    }
}
