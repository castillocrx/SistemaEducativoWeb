using Microsoft.EntityFrameworkCore;

namespace SistemaEducativoWeb.Models
{
    [Keyless]
    public class ReporteCursos
    {
        public string Curso { get; set; }
        public int Estudiantes { get; set; }
    }
}
