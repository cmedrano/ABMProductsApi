# =========================
# BUILD STAGE
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar los csproj de todos los proyectos para aprovechar cache de Docker
COPY Backend-Product/Products.Api.csproj Backend-Product/
COPY Products.Application/Products.Application.csproj Products.Application/
COPY Products.Domain/Products.Domain.csproj Products.Domain/
COPY Products.Infrastructure/Products.Infrastructure.csproj Products.Infrastructure/

# Restaurar sólo a nivel de proyectos referenciados
RUN dotnet restore Backend-Product/Products.Api.csproj

# Copiar el resto del código
COPY Backend-Product/. Backend-Product/
COPY Products.Application/. Products.Application/
COPY Products.Domain/. Products.Domain/
COPY Products.Infrastructure/. Products.Infrastructure/

# Publicar la app
WORKDIR /src/Backend-Product
RUN dotnet publish -c Release -o /app/publish

# =========================
# RUNTIME STAGE
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Instalar herramientas útiles para debugging
RUN apt-get update && apt-get install -y curl

# Copiar publicación
COPY --from=build /app/publish .

# HEALTH CHECK (requerido para Render)
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

EXPOSE 8080
ENTRYPOINT ["dotnet", "Products.Api.dll"]




