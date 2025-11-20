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
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<IProductService,ProductService>();

// Registrar repositorios
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

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
});

var app = builder.Build();

app.UseCors("AllowAngular");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
