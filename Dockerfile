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
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copiamos la salida de publish
COPY --from=build /app/publish .

# EXPONEMOS 8080 (puerto interno del contenedor)
EXPOSE 8080

# ENTRYPOINT: ejecuta la dll
ENTRYPOINT ["dotnet", "Products.Api.dll"]




