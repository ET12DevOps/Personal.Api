using Microsoft.EntityFrameworkCore;
using SistemaDeControlPersonal.Models;

namespace SistemaDeControlPersonal.Persistencia;

public class PersonalDbContext : DbContext
{
    public PersonalDbContext(DbContextOptions<PersonalDbContext> options) : base(options)
    {

    }

    public DbSet<Empleado> Empleados { get; set; }
    public DbSet<Acceso> Accesos { get; set; }
}