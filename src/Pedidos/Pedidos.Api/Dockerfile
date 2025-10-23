# Use a imagem base do SDK para construir
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos de solução e projeto e restaura dependências
COPY FoodHall.sln .
COPY src/Pedidos/Pedidos.Api/Pedidos.Api.csproj src/Pedidos/Pedidos.Api/
COPY src/Pedidos/Pedidos.Application/Pedidos.Application.csproj src/Pedidos/Pedidos.Application/
COPY src/Pedidos/Pedidos.Domain/Pedidos.Domain.csproj src/Pedidos/Pedidos.Domain/
COPY src/Pedidos/Pedidos.Infrastructure/Pedidos.Infrastructure.csproj src/Pedidos/Pedidos.Infrastructure/
RUN dotnet restore

# Copia todo o código-fonte
COPY . .
WORKDIR /src/src/Pedidos/Pedidos.Api

# Publica o aplicativo em modo Release
RUN dotnet publish -c Release -o /app/publish

# Use a imagem de runtime para execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Define a porta de exposição padrão
EXPOSE 8080

ENTRYPOINT ["dotnet", "Pedidos.Api.dll"]