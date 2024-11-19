using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaEducativoWeb.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string NombreUsuario { get; set; }

        [Required]
        [MaxLength(100)]
        public string Contraseña { get; set; }

        [Required]
        public int RolId { get; set; }

        [ForeignKey("RolId")]
        public Rol? Rol { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }
    }
}
