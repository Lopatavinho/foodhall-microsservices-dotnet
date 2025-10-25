// ----------------------------------------------------------------------
// USINGS: DEVEM SEMPRE ESTAR NO TOPO DO ARQUIVO
// ----------------------------------------------------------------------
using Identidade.Infrastructure.Data;
using Identidade.Infrastructure.Repository;
using Identidade.Domain.Interfaces;
using Identidade.Application.Interfaces;
using Identidade.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// ----------------------------------------------------------------------

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao contêiner.

// 1. Configuração do Swagger/OpenAPI
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// 2. Configuração do Entity Framework Core e MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Garante que a string de conexão foi lida (para evitar erro de nulidade no AutoDetect)
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("A ConnectionString 'DefaultConnection' não foi configurada.");
}

builder.Services.AddDbContext<IdentidadeDbContext>(options =>
    options.UseMySql(connectionString,
                     ServerVersion.AutoDetect(connectionString),
                     mysqlOptions => mysqlOptions.MigrationsAssembly(typeof(IdentidadeDbContext).Assembly.FullName)));


// 3. Injeção de Dependência (Clean Architecture)
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IAutenticacaoService, AutenticacaoService>();


// 4. Configuração da Autenticação JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Mudar para true em produção
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        // A chave deve ser longa o suficiente (mínimo 256 bits / 32 bytes)
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings["Secret"]!))
    };
});


var app = builder.Build();

// Configura o pipeline de requisições HTTP.

// Aplica as Migrações Pendentes na Inicialização (Opcional, mas útil no desenvolvimento)
using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<IdentidadeDbContext>();
    dataContext.Database.Migrate();

    // ---------------------------------------------------------------
    // CÓDIGO OPCIONAL PARA CRIAR UM USUÁRIO PADRÃO (SE NÃO EXISTIR)
    // Usaremos 'cliente@foodhall.com' e '123456'
    // ---------------------------------------------------------------
    var repository = scope.ServiceProvider.GetRequiredService<IUsuarioRepository>();
    if (repository.BuscarPorEmailAsync("cliente@foodhall.com").Result == null)
    {
        var authService = scope.ServiceProvider.GetRequiredService<IAutenticacaoService>();
        authService.RegistrarAsync("cliente@foodhall.com", "123456", "Cliente Teste", "Cliente").Wait();
        Console.WriteLine(">>> Usuário Cliente Padrão criado: cliente@foodhall.com / 123456");
    }
    if (repository.BuscarPorEmailAsync("lojista@foodhall.com").Result == null)
    {
        var authService = scope.ServiceProvider.GetRequiredService<IAutenticacaoService>();
        authService.RegistrarAsync("lojista@foodhall.com", "123456", "Lojista Teste", "Lojista").Wait();
        Console.WriteLine(">>> Usuário Lojista Padrão criado: lojista@foodhall.com / 123456");
    }
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Habilita a autenticação (deve vir antes do UseAuthorization)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();