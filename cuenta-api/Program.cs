using cuenta_api.CuentaMappers;
using cuenta_api.Data;
using cuenta_api.Repositorio;
using cuenta_api.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Crear la conexión a la base de datos SQL
builder.Services.AddDbContext<ApplicationDbContext>(Opciones =>
{
    Opciones.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSQL"));
});

//Agregamos las clase de repositorios
builder.Services.AddScoped<ICuentaRepositorio, CuentaRepositorio>();
builder.Services.AddScoped<IMovimientoRepositorio, MovimientoRepositorio>();
builder.Services.AddScoped<IReporteRepositorio, ReporteRepositorio>();

//Agregarmos el automapper para la implementación de Dtos esquema repositorio paterm
builder.Services.AddAutoMapper(typeof(CuentasMapper));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
