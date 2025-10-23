# Use a imagem base do SDK para construir
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos de solução e projeto e restaura dependências
COPY FoodHall.sln .
COPY src/Identidade/Identidade.Api/Identidade.Api.csproj src/Identidade/Identidade.Api/
COPY src/Identidade/Identidade.Application/Identidade.Application.csproj src/Identidade/Identidade.Application/
COPY src/Identidade/Identidade.Domain/Identidade.Domain.csproj src/Identidade/Identidade.Domain/
COPY src/Identidade/Identidade.Infrastructure/Identidade.Infrastructure.csproj src/Identidade/Identidade.Infrastructure/
RUN dotnet restore

# Copia todo o código-fonte
COPY . .
WORKDIR /src/src/Identidade/Identidade.Api

# Publica o aplicativo em modo Release
RUN dotnet publish -c Release -o /app/publish

# Use a imagem de runtime para execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Define a porta de exposição padrão
EXPOSE 8080

ENTRYPOINT ["dotnet", "Identidade.Api.dll"]