using Identidade.Application.DTOs;
using Identidade.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Identidade.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAutenticacaoService _autenticacaoService;

    public AuthController(IAutenticacaoService autenticacaoService)
    {
        _autenticacaoService = autenticacaoService;
    }
    
    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar([FromBody] RegistroRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        // Simplesmente para testar. O ideal é ter um DTO de Registro.
        // Assumindo RegistroRequest tem Email, Senha, Nome e PerfilAcesso
        bool sucesso = await _autenticacaoService.RegistrarAsync(request.Email, request.Senha, request.Nome, request.PerfilAcesso);

        if (!sucesso)
        {
            return BadRequest(new { message = "E-mail já registrado." });
        }
        
        // Após registrar, faz o login para retornar o token
        var loginRequest = new LoginRequest { Email = request.Email, Senha = request.Senha };
        var tokenResponse = await _autenticacaoService.LoginAsync(loginRequest);

        return Ok(tokenResponse);
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var tokenResponse = await _autenticacaoService.LoginAsync(request);

        if (tokenResponse == null)
        {
            return Unauthorized(new { message = "Credenciais inválidas." });
        }

        return Ok(tokenResponse);
    }
}

// Classe DTO de Registro (Crie este arquivo também: RegistroRequest.cs)
public class RegistroRequest
{
    public string Email { get; set; }
    public string Senha { get; set; }
    public string Nome { get; set; }
    public string PerfilAcesso { get; set; } // Ex: "Cliente" ou "Lojista"
}