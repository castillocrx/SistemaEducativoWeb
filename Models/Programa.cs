using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaEducativoWeb.Models
{
    public class Programa
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        [Required]
        [Range(1, 60)]
        public int Duracion { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        public bool Estado { get; set; } 

        public ICollection<Curso>? Cursos { get; set; }
    }
}
