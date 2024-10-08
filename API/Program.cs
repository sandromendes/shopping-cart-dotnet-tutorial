using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Business.Services;
using Infrastructure.Repositories;
using Domain.Business.Interfaces;
using Domain.Infrastructure.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to container
builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// config connection string to MySQL Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// config DbContext for MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 29))));

// Registrar o AutoMapper (com os perfis de mapeamento)
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// register services
builder.Services.AddScoped<ICartService, CartService>();

// Register repository
builder.Services.AddScoped<ICartRepository, CartRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShoppingCart API V1");
        c.RoutePrefix = string.Empty; // access swagger from root (http://localhost:<port>/)
    });
}

// config http request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();