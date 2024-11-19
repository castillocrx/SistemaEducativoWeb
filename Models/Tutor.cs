using System.ComponentModel.DataAnnotations;

namespace SistemaEducativoWeb.Models
{
    public class Tutor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(100)]
        public string Apellidos { get; set; }

        [Required]
        [EmailAddress]
        public string Correo { get; set; }
        public string? Telefono { get; set; } 
        public string? Especialidad { get; set; } 

        [Required]
        public DateTime FechaRegistro { get; set; }

        public ICollection<Curso>? Cursos { get; set; }
    }
}
