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
        private readonly UsuarioContextHelper _usuarioContextHelper;

        public UsuarioService(ApplicationDbContext context, AuditoriaHelper auditoriaHelper, UsuarioContextHelper usuarioContextHelper)
        {
            _context = context;
            _auditoriaHelper = auditoriaHelper;
            _usuarioContextHelper = usuarioContextHelper;
        }

        public async Task<Usuario?> ValidarCredencialesAsync(string alias, string clave)
        {
            var claveBytes = Encoding.UTF8.GetBytes(clave);
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Alias == alias && u.Clave == claveBytes && u.Activo);
        }

        private UsuarioViewDto MapToViewDto(Usuario u)
        {
            return new UsuarioViewDto
            {
                IdUsuario = u.IdUsuario,
                IdDominio = u.IdDominio,
                NombreDominio = u.Dominio?.Nombre,
                Alias = u.Alias,
                DocIdentTipo = u.DocIdentTipo,
                DocIdentNro = u.DocIdentNro,
                Nombres = u.Nombres,
                Correo = u.Correo,
                Telefono = u.Telefono,
                Clave = u.Clave != null ? Encoding.UTF8.GetString(u.Clave) : null,
                Rol = u.Rol
            };
        }

        public async Task<IEnumerable<UsuarioViewDto>> GetAllAsync()
        {
            return await _context.Usuarios
                .Include(u => u.Dominio)
                .Where(u => u.Activo)
                .AsNoTracking()
                .Select(u => MapToViewDto(u))
                .ToListAsync();
        }

        public async Task<UsuarioViewDto?> GetByIdAsync(Guid id)
        {
            return await _context.Usuarios
                .Include(u => u.Dominio)
                .Where(u => u.Activo)
                .AsNoTracking()
                .Select(u => MapToViewDto(u))
                .FirstOrDefaultAsync();
        }

        public async Task<UsuarioViewDto> CreateAsync(UsuarioCreateDto dto)
        {
            var usuarioActual = _usuarioContextHelper.GetUsuarioActual();

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
                Rol = dto.Rol,
                Activo = true,
                IdResponsable = usuarioActual
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            var detalle = AuditoriaDetalleHelper.GenerarDetalle(usuario, "Usuario creado");
            await _auditoriaHelper.RegistrarAsync("usuario", usuario.IdUsuario, "INSERT", detalle);

            return MapToViewDto(usuario);
        }

        public async Task<UsuarioViewDto?> PatchAsync(Guid id, UsuarioPatchDto dto)
        {
            var usuarioActual = _usuarioContextHelper.GetUsuarioActual();

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return null;

            var usuarioAntes = new Usuario
            {
                IdUsuario = usuario.IdUsuario,
                IdDominio = usuario.IdDominio,
                Alias = usuario.Alias,
                DocIdentTipo = usuario.DocIdentTipo,
                DocIdentNro = usuario.DocIdentNro,
                Nombres = usuario.Nombres,
                Correo = usuario.Correo,
                Telefono = usuario.Telefono,
                Clave = usuario.Clave,
                Rol = usuario.Rol,
                Activo = usuario.Activo,
                IdResponsable = usuario.IdResponsable
            };

            if (dto.Alias != null) usuario.Alias = dto.Alias;
            if (dto.Nombres != null) usuario.Nombres = dto.Nombres;
            if (dto.Correo != null) usuario.Correo = dto.Correo;
            if (dto.Telefono != null) usuario.Telefono = dto.Telefono;
            if (dto.Clave != null) usuario.Clave = Encoding.UTF8.GetBytes(dto.Clave);
            if (dto.Rol != null) usuario.Rol = dto.Rol;

            usuario.IdResponsable = usuarioActual;
            await _context.SaveChangesAsync();

            var detalle = AuditoriaDetalleHelper.GenerarCambios(usuarioAntes, usuario, "Usuario actualizado parcialmente");
            await _auditoriaHelper.RegistrarAsync("usuario", usuario.IdUsuario, "PATCH", detalle);

            return MapToViewDto(usuario);
        }

        public async Task<UsuarioViewDto?> UpdateAsync(Guid id, UsuarioUpdateDto dto)
        {
            var usuarioActual = _usuarioContextHelper.GetUsuarioActual();

            var usuario = await _context.Usuarios.FindAsync(id);
            if(usuario == null) return null;

            var usuarioAntes = new Usuario
            {
                IdUsuario = usuario.IdUsuario,
                IdDominio = usuario.IdDominio,
                Alias = usuario.Alias,
                DocIdentTipo = usuario.DocIdentTipo,
                DocIdentNro = usuario.DocIdentNro,
                Nombres = usuario.Nombres,
                Correo = usuario.Correo,
                Telefono = usuario.Telefono,
                Clave = usuario.Clave,
                Rol = usuario.Rol,
                Activo = usuario.Activo,
                IdResponsable = usuario.IdResponsable
            };

            usuario.Alias = dto.Alias;
            usuario.DocIdentTipo = dto.DocIdentTipo;
            usuario.DocIdentNro = dto.DocIdentNro;
            usuario.Nombres = dto.Nombres;
            usuario.Correo = dto.Correo;
            usuario.Telefono = dto.Telefono;
            usuario.Clave = string.IsNullOrWhiteSpace(dto.Clave) ? null : Encoding.UTF8.GetBytes(dto.Clave);
            usuario.Rol = dto.Rol;
            usuario.IdResponsable = usuarioActual;

            await _context.SaveChangesAsync();

            var detalle = AuditoriaDetalleHelper.GenerarCambios(usuarioAntes, usuario, "Usuario actualizado");
            await _auditoriaHelper.RegistrarAsync("usuario", usuario.IdUsuario, "UPDATE", detalle);

            return MapToViewDto(usuario);
        }

        public async Task<bool> DesactivarAsync(Guid id)
        {
            var usuarioActual = _usuarioContextHelper.GetUsuarioActual();

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return false;

            usuario.Activo = false;
            usuario.IdResponsable = usuarioActual;

            await _context.SaveChangesAsync();

            await _auditoriaHelper.RegistrarAsync("usuario", usuario.IdUsuario, "DELETE", "Usuario desactivado (Activo=false)");
            
            return true;
        }
    }
}
