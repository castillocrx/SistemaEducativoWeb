using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaEducativoWeb.Models
{
    public class ProgresoEstudiante
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EstudianteId { get; set; }

        [ForeignKey("EstudianteId")]
        public Estudiante? Estudiante { get; set; }

        [Required]
        public int CursoId { get; set; }

        [ForeignKey("CursoId")]
        public Curso? Curso { get; set; }

        [Required]
        public DateTime FechaAvance { get; set; }

        [Required]
        [MaxLength(500)] 
        public string Descripcion { get; set; }

        [Range(0, 100)]
        public int PorcentajeCompletado { get; set; }
    }
}
