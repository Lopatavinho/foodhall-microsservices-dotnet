# Use a imagem base do SDK para construir
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos de solução e projeto e restaura dependências
COPY FoodHall.sln .
COPY src/ApiGateway/ApiGateway.Ocelot/ApiGateway.Ocelot.csproj src/ApiGateway/ApiGateway.Ocelot/
RUN dotnet restore

# Copia todo o código-fonte
COPY . .
WORKDIR /src/src/ApiGateway/ApiGateway.Ocelot

# Publica o aplicativo em modo Release
RUN dotnet publish -c Release -o /app/publish

# Use a imagem de runtime para execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Define a porta de exposição padrão
EXPOSE 8080

ENTRYPOINT ["dotnet", "ApiGateway.Ocelot.dll"]