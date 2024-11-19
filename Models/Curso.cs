using System.ComponentModel.DataAnnotations;

namespace SistemaEducativoWeb.Models
{
    public class Curso
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        [Required]
        public int Duracion { get; set; }
        public int? TutorId { get; set; } 

        public Tutor? Tutor { get; set; }

        public int ProgramaId { get; set; }  
        public Programa? Programa { get; set; }  

        public ICollection<ProgresoEstudiante>? Progresos { get; set; }
        public ICollection<Evaluacion>? Evaluaciones { get; set; }
        public ICollection<Matricula>? Matriculas { get; set; }
    }
}
