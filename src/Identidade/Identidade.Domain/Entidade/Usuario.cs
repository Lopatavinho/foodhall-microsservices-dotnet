using BCrypt.Net; // Adicione este using

namespace Identidade.Domain.Entidade;

public class Usuario
{
    public Guid Id { get; private set; } 
    public string Email { get; private set; }
    public string SenhaHash { get; private set; } // Armazena a senha criptografada
    public string Nome { get; private set; }
    public string PerfilAcesso { get; private set; } 

    // Construtor usado pelo EF Core ou para recriar o objeto
    protected Usuario() { }

    // Construtor usado para criar um novo usuário
    public Usuario(string email, string senha, string nome, string perfilAcesso)
    {
        Id = Guid.NewGuid();
        Email = email.ToLowerInvariant();
        Nome = nome;
        PerfilAcesso = perfilAcesso;
        // Chama o método para criar o hash da senha
        DefinirSenha(senha); 
    }
    
    // Método para criptografar a senha (usado no construtor)
    public void DefinirSenha(string senha)
    {
        // BCrypt cria o hash da senha de forma segura
        SenhaHash = BCrypt.Net.BCrypt.HashPassword(senha, workFactor: 12);
    }
    
    // Método para validar a senha durante o login
    public bool ValidarSenha(string senha)
    {
        // BCrypt verifica se o hash corresponde à senha fornecida
        return BCrypt.Net.BCrypt.Verify(senha, SenhaHash);
    }
}