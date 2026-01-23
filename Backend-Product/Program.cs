using Backend_Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Products.Application.IServices;
using Products.Application.Services;
using Products.Domain.Interfaces;
using Products.Infrastructure.Data;
using Products.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// En LOCAL (Development): usa launchSettings.json
// En NUBE (Render / Docker): Render define la variable PORT y nosotros la usamos
if (!builder.Environment.IsDevelopment())
{
    var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";

    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(int.Parse(port));
    });
}

// switch connection
var provider = builder.Configuration["DatabaseProvider"];
var connectionString = provider switch
{
    "PostgreSql" => builder.Configuration.GetConnectionString("PostgreSqlConnection"),
    _ => builder.Configuration.GetConnectionString("SqlServerConnectionLocal")
};

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (provider == "PostgreSql")
    {
        options.UseNpgsql(connectionString);
    }
    else
    {
        options.UseSqlServer(connectionString, sql => sql.EnableRetryOnFailure());
    }
});

// Registrar AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Registrar servicios
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IProductService,ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

// Registrar repositorios
builder.Services.AddScoped<IClientRepository,ClientRepository>();
builder.Services.AddScoped<ICategoryRepository,CategoryRepository>();
builder.Services.AddScoped<IProductRepository,ProductRepository>();

// Registrar controladores
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Backend Product API",
        Version = "v1",
        Description = "API para la gestión de productos (DDD + EF Core)"
    });

    c.EnableAnnotations(); // Habilitar anotaciones (Documentacion en el Swagger)
});


var allowedOrigins = builder.Environment.IsDevelopment()
    ? new[] { "http://localhost:4200" } // Angular en local
    : new[] { "https://salessystemfront.pages.dev" }; // Angular en producción (Cloudflare Pages)

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowAngular");

// Swagger solo para desarrollo (local o nube dev)
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("CloudDevelopment"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS solo en local (en la nube lo maneja Cloudflare / Render)
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
