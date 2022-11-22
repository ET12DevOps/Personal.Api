using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaDeControlPersonal.Models;

[Table("Empleado")]
public class Empleado
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Nombre { get; set; }

    [Required]
    [StringLength(50)]
    public string Apellido { get; set; }

    [Required]
    [StringLength(30)]
    public string Tarjeta { get; set; }
}