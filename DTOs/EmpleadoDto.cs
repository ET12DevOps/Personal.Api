using System.ComponentModel.DataAnnotations;

namespace SistemaDeControlPersonal.DTOs;
public class EmpleadoDto
{
    [Required]
    [StringLength(50)]
    public required string Nombre { get; set; }

    [Required]
    [StringLength(50)]
    public required string Apellido { get; set; }

    [Required]
    [StringLength(30)]
    public required string Tarjeta { get; set; }
}