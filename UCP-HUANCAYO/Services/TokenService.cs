using Microsoft.EntityFrameworkCore;
using UCP_HUANCAYO.Data;
using UCP_HUANCAYO.Dtos.Token;
using UCP_HUANCAYO.Models;

namespace UCP_HUANCAYO.Services
{
    public class TokenService
    {
        private readonly ApplicationDbContext _context;

        public TokenService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TokenViewDto>> GetAllAsync()
        {
            return await _context.Tokens
                .AsNoTracking()
                .Select(t => new TokenViewDto
                {
                    IdToken = t.IdToken,
                    IdUsuario = t.IdUsuario,
                    Expiracion = t.Expiracion,
                    Ip = t.Ip,
                    Revocado = t.Revocado
                })
                .ToListAsync();
        }

        public async Task<TokenViewDto?> GetByIdAsync(Guid id)
        {
            return await _context.Tokens
                .AsNoTracking()
                .Where(t => t.IdToken == id)
                .Select(t => new TokenViewDto
                {
                    IdToken = t.IdToken,
                    IdUsuario = t.IdUsuario,
                    Expiracion = t.Expiracion,
                    Ip = t.Ip,
                    Revocado = t.Revocado
                })
                .FirstOrDefaultAsync();
        }

        public async Task<TokenViewDto> CreateAsync(TokenCreateDto dto)
        {
            var token = new Token
            {
                IdToken = Guid.NewGuid(),
                IdUsuario = dto.IdUsuario,
                Expiracion = dto.Expiracion,
                Ip = dto.Ip,
                Revocado = false
            };

            _context.Tokens.Add(token);
            await _context.SaveChangesAsync();

            return new TokenViewDto
            {
                IdToken = token.IdToken,
                IdUsuario = token.IdUsuario,
                Expiracion = token.Expiracion,
                Ip = token.Ip,
                Revocado = token.Revocado
            };
        }

        public async Task<bool> RevocarAsync(Guid id)
        {
            var token = await _context.Tokens.FindAsync(id);
            if (token == null || token.Revocado) return false;

            token.Revocado = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EsTokenValidoAsync(Guid idToken)
        {
            return await _context.Tokens
                .AnyAsync(t => t.IdToken == idToken && t.Expiracion > DateTime.UtcNow && !t.Revocado);
        }
    }
}
