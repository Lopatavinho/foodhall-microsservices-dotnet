# Use a imagem base do SDK para construir
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos de solução e projeto e restaura dependências
COPY FoodHall.sln .
COPY src/Catalogo/Catalogo.Api/Catalogo.Api.csproj src/Catalogo/Catalogo.Api/
COPY src/Catalogo/Catalogo.Application/Catalogo.Application.csproj src/Catalogo/Catalogo.Application/
COPY src/Catalogo/Catalogo.Domain/Catalogo.Domain.csproj src/Catalogo/Catalogo.Domain/
COPY src/Catalogo/Catalogo.Infrastructure/Catalogo.Infrastructure.csproj src/Catalogo/Catalogo.Infrastructure/
RUN dotnet restore

# Copia todo o código-fonte
COPY . .
WORKDIR /src/src/Catalogo/Catalogo.Api

# Publica o aplicativo em modo Release
RUN dotnet publish -c Release -o /app/publish

# Use a imagem de runtime para execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Define a porta de exposição padrão
EXPOSE 8080

ENTRYPOINT ["dotnet", "Catalogo.Api.dll"]