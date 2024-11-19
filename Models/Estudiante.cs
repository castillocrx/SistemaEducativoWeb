using System.ComponentModel.DataAnnotations;

namespace SistemaEducativoWeb.Models
{
    public class Estudiante
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

        public string Telefono { get; set; }
        [Required]
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaRegistro { get; set; }

        public ICollection<ProgresoEstudiante>? Progresos { get; set; }
        public ICollection<Evaluacion>? Evaluaciones { get; set; }
        public ICollection<Matricula>? Matriculas { get; set; }
    }
}
