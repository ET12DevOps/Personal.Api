using Microsoft.EntityFrameworkCore;
using SistemaDeControlPersonal.DTOs;
using SistemaDeControlPersonal.Models;
using SistemaDeControlPersonal.Persistencia;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("personal_db");

var mysqlVersion = new MySqlServerVersion("8.0.30");

builder.Services.AddDbContext<PersonalDbContext>(options => options.UseMySql(connectionString, mysqlVersion));

builder.Services.AddScoped<PersonalDbContext>();

var options = new DbContextOptionsBuilder<PersonalDbContext>();

options.UseMySql(connectionString, mysqlVersion);

var personalDb = new PersonalDbContext(options.Options);

await personalDb.Database.EnsureCreatedAsync();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("appPolicy",
                      builder =>
                      {
                          builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                      });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("appPolicy");

app.MapGet("/api/empleado", async (PersonalDbContext db) =>
{
    return await db.Empleados.OrderByDescending(x => x.Id).Take(5).ToListAsync();
});

app.MapPost("/api/empleado", async (PersonalDbContext db, EmpleadoDto empleado) =>
{
    var nuevoEmpleado = new Empleado
    {
        Nombre = empleado.Nombre,
        Apellido = empleado.Apellido,
        Tarjeta = empleado.Tarjeta
    };
    await db.Empleados.AddAsync(nuevoEmpleado);
    await db.SaveChangesAsync();
    return Results.Created($"/api/empleado/{nuevoEmpleado.Id}", nuevoEmpleado);
});

app.MapDelete("/api/empleado/{id:int}", async (PersonalDbContext db, int id) =>
{
    var empleado = await db.Empleados.FindAsync(id);
    if (empleado is null) return Results.NotFound();
    db.Empleados.Remove(empleado);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.MapGet("/api/acceso", async (PersonalDbContext db) =>
{
    var accesos = await db.Accesos.Include(x => x.Empleado).OrderByDescending(x => x.Id).Take(5).ToListAsync();

    return accesos.Select(x => new { x.Empleado.Nombre, x.Empleado.Apellido, x.Empleado.Tarjeta, Hora = x.Hora.ToString("yyyy-MM-dd HH':'mm':'ss") });
});

app.MapGet("/api/acceso/dia", (PersonalDbContext db) =>
{
    var accesos = from acceso in db.Accesos
                  group acceso by acceso.Hora.DayOfWeek into totalDia
                  orderby totalDia.Key ascending
                  select new { Dia = totalDia.Key, Accesos = totalDia.Count() };

    var accesosSemana = new List<(string, int)>();

    for (int i = 0; i < 7; i++)
    {
        var accesoDia = accesos.FirstOrDefault(x => ((int)x.Dia) == i);
        var cantidad = accesoDia is not null ? accesoDia.Accesos : 0;
        accesosSemana.Add(new(((DayOfWeek)i).ToString(), cantidad));
    }


    return Results.Ok(accesosSemana.Select(x => new { dia = x.Item1, accesos = x.Item2 }));
});

app.MapPost("/api/acceso", async (PersonalDbContext db, AccesoDto acceso) =>
{
    var empleado = await db.Empleados.FirstOrDefaultAsync(x => x.Tarjeta == acceso.Tarjeta);
    if (empleado is null) return Results.NotFound();
    var nuevoAcceso = new Acceso
    {
        Empleado = empleado,
    };
    await db.Accesos.AddAsync(nuevoAcceso);
    await db.SaveChangesAsync();
    return Results.Created($"/api/acceso/{nuevoAcceso.Id}", nuevoAcceso);
});

app.Run();
