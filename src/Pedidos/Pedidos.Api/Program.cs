using Pedidos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore; // Adicionar este using

var builder = WebApplication.CreateBuilder(args);

// ... (Resto da configuração padrão) ...

// --- Início da Configuração do Entity Framework Core ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// O GetConnectionString irá buscar a variável de ambiente DB_CONNECTION_STRING
// que definimos no docker-compose.yml!

builder.Services.AddDbContext<PedidosDbContext>(options =>
    options.UseMySql(connectionString,
                     ServerVersion.AutoDetect(connectionString),
                     mysqlOptions => mysqlOptions.MigrationsAssembly(typeof(PedidosDbContext).Assembly.FullName)));
// --- Fim da Configuração do Entity Framework Core ---

// ... (Resto do código) ...

var app = builder.Build();

// ... (Resto do código) ...