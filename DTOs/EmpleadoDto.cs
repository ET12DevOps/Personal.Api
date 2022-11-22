using System.ComponentModel.DataAnnotations;

namespace SistemaDeControlPersonal.DTOs;
public class EmpleadoDto
{
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