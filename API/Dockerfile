# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copia os arquivos de projeto e restaura as dependências
COPY ["API/API.csproj", "API/"]
COPY ["Business/Business.csproj", "Business/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Domain/Domain.csproj", "Domain/"]
RUN dotnet restore "API/API.csproj"

# Copia o restante do código e compila a aplicação
COPY . .
WORKDIR "/app/API"
RUN dotnet publish -c Release -o /out

# Etapa de execução
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /out .

# Configura o ambiente de produção para o ASP.NET Core
# ENV ASPNETCORE_ENVIRONMENT Docker
ENV ASPNETCORE_URLS http://+:5000
EXPOSE 5000

# Inicia a aplicação
ENTRYPOINT ["dotnet", "API.dll"]