using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaEducativoWeb.Models
{
    public class Evaluacion
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
        public DateTime FechaEvaluacion { get; set; }

        [Range(0, 100)]
        [Required]  
        public int Calificacion { get; set; }

        [MaxLength(500)] 
        public string Comentarios { get; set; }
    }
}
