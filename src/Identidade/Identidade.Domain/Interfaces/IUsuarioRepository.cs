using Identidade.Domain.Entidade;

namespace Identidade.Domain.Interfaces;

public interface IUsuarioRepository
{
    Task AdicionarUsuarioAsync(Usuario usuario);
    Task<Usuario> BuscarPorEmailAsync(string email);
    Task<Usuario> BuscarPorIdAsync(Guid id);
}