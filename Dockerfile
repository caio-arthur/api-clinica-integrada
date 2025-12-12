# Estágio 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia os arquivos de projeto respeitando a estrutura de pastas e restaura as dependências
COPY ["src/Apps/WebApi/WebApi.csproj", "src/Apps/WebApi/"]
COPY ["src/Common/Application/Application.csproj", "src/Common/Application/"]
COPY ["src/Common/Domain/Domain.csproj", "src/Common/Domain/"]
COPY ["src/Common/Infrastructure/Infrastructure.csproj", "src/Common/Infrastructure/"]
RUN dotnet restore "src/Apps/WebApi/WebApi.csproj"

# Copia o resto do código fonte
COPY . .
WORKDIR "/app/src/Apps/WebApi"
RUN dotnet build "WebApi.csproj" -c Release -o /app/build

# Estágio 2: Publicação
FROM build AS publish
RUN dotnet publish "WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Estágio 3: Final - Imagem de Produção
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Define a porta que a aplicação vai ouvir DENTRO do contêiner
ENV ASPNETCORE_URLS=http://+:8080

# Ponto de entrada para iniciar a aplicação
ENTRYPOINT ["dotnet", "WebApi.dll"]