using Identidade.Domain.Entidade;
using Identidade.Domain.Interfaces;
using Identidade.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Identidade.Infrastructure.Repository;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly IdentidadeDbContext _context;

    public UsuarioRepository(IdentidadeDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarUsuarioAsync(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task<Usuario> BuscarPorEmailAsync(string email)
    {
        // Busca ignorando case, já que salvamos tudo em minúsculas
        return await _context.Usuarios
                             .AsNoTracking()
                             .FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant());
    }

    public async Task<Usuario> BuscarPorIdAsync(Guid id)
    {
        return await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
    }
}