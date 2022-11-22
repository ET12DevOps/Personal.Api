using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaDeControlPersonal.Models;

[Table("Acceso")]
public class Acceso
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    [ForeignKey("IdEmpleado")]
    public Empleado Empleado { get; set; }

    [Required]
    public DateTime Hora { get; set; } = DateTime.Now;
}