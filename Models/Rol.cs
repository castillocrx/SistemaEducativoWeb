using System.ComponentModel.DataAnnotations;

namespace SistemaEducativoWeb.Models
{
    public class Rol
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string NombreRol { get; set; }

        [MaxLength(200)] 
        public string Descripcion { get; set; }

        public ICollection<Usuario>? Usuarios { get; set; }
    }
}
