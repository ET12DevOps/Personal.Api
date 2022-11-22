using System.ComponentModel.DataAnnotations;

namespace SistemaDeControlPersonal.DTOs;
public class AccesoDto
{
    [Required]
    [StringLength(20)]
    public string Tarjeta { get; set; }

}