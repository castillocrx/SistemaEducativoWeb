using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaEducativoWeb.Models
{
    public class Matricula
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("EstudianteId")]
        public int EstudianteId { get; set; }

        public Estudiante? Estudiante { get; set; }

        [ForeignKey("CursoId")]
        public int CursoId { get; set; }

        public Curso? Curso { get; set; }

        [Required]
        [MaxLength(20)] 
        public string Estado { get; set; }

        [Required] 
        public DateTime FechaInscripcion { get; set; }


    }
}
