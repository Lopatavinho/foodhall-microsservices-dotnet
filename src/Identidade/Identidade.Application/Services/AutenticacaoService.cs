using Identidade.Application.DTOs;
using Identidade.Application.Interfaces;
using Identidade.Domain.Entidade;
using Identidade.Domain.Interfaces;
using Microsoft.Extensions.Configuration; // Pacote necessário
using Microsoft.IdentityModel.Tokens; // Pacote necessário
using System.IdentityModel.Tokens.Jwt; // Pacote necessário
using System.Security.Claims;
using System.Text;

namespace Identidade.Application.Services;

public class AutenticacaoService : IAutenticacaoService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IConfiguration _configuration;


    public AutenticacaoService(IUsuarioRepository usuarioRepository, IConfiguration configuration)
    {
        _usuarioRepository = usuarioRepository;
        _configuration = configuration;
    }

    public async Task<TokenResponse> LoginAsync(LoginRequest request)
    {
        var usuario = await _usuarioRepository.BuscarPorEmailAsync(request.Email);

        if (usuario == null || !usuario.ValidarSenha(request.Senha))
        {
            // Poderia lançar uma exceção específica de login falho
            return null; 
        }

        // Se o login for bem-sucedido, gera o token
        return GerarTokenJwt(usuario);
    }
    
    // Método simples de registro (opcional, mas útil para criar o primeiro usuário)
    public async Task<bool> RegistrarAsync(string email, string senha, string nome, string perfil)
    {
        if (await _usuarioRepository.BuscarPorEmailAsync(email) != null)
        {
            return false; // Usuário já existe
        }

        var novoUsuario = new Usuario(email, senha, nome, perfil);
        await _usuarioRepository.AdicionarUsuarioAsync(novoUsuario);
        return true;
    }

    private TokenResponse GerarTokenJwt(Usuario usuario)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        
        // Chave Secreta definida nas configurações (usaremos o Program.cs)
        var secret = _configuration["JwtSettings:Secret"] ?? throw new ArgumentNullException("JwtSettings:Secret not configured.");
        var key = Encoding.ASCII.GetBytes(secret);

        var issuer = _configuration["JwtSettings:Issuer"];
        var audience = _configuration["JwtSettings:Audience"];
        
        // Claims (informações sobre o usuário que serão injetadas no token)
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.PerfilAcesso) // 'Cliente' ou 'Lojista'
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(2), // Token válido por 2 horas
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return new TokenResponse
        {
            AccessToken = tokenHandler.WriteToken(token),
            ExpiresIn = (long)(tokenDescriptor.Expires.Value - DateTime.UtcNow).TotalSeconds,
            UsuarioId = usuario.Id,
            Perfil = usuario.PerfilAcesso
        };
    }
}