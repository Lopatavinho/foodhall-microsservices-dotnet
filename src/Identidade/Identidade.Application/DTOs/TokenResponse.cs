namespace Identidade.Application.DTOs;

public class TokenResponse
{
    public string AccessToken { get; set; }
    public long ExpiresIn { get; set; } // Tempo de expiração em segundos
    public string TokenType { get; set; } = "Bearer";
    public Guid UsuarioId { get; set; }
    public string Perfil { get; set; }
}