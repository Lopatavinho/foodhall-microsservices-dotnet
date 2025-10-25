using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// 1. Configura o Ocelot para carregar as rotas do arquivo ocelot.json
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// 2. Adiciona o Ocelot ao container de serviços
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

// 3. Usa o Ocelot no pipeline
await app.UseOcelot();

app.Run();