using Identidade.Application.DTOs;

namespace Identidade.Application.Interfaces;

public interface IAutenticacaoService
{
    Task<TokenResponse> LoginAsync(LoginRequest request);
    Task<bool> RegistrarAsync(string email, string senha, string nome, string perfil);
}