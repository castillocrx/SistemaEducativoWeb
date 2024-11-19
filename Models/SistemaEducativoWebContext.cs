using Microsoft.EntityFrameworkCore;

namespace SistemaEducativoWeb.Models
{
    public class SistemaEducativoWebContext: DbContext
    {
        public SistemaEducativoWebContext(DbContextOptions<SistemaEducativoWebContext> opciones)
            : base(opciones)
        {

        }

        public DbSet<Curso> Curso { get; set; }
        public DbSet<Estudiante> Estudiante { get; set; }
        public DbSet<Evaluacion> Evaluacion { get; set; }
        public DbSet<Programa> Programa { get; set; }
        public DbSet<ProgresoEstudiante> ProgresoEstudiante { get; set; }
        public DbSet<Tutor> Tutor { get; set; }
        public DbSet<Rol> Rol { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Matricula> Matricula { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Curso>(curso =>
            {
                curso.HasKey(c => c.Id);
                curso.Property(c => c.Nombre).IsRequired();
                curso.Property(c => c.Duracion).IsRequired();
                curso.HasOne(c => c.Programa).WithMany(p => p.Cursos).HasForeignKey(c => c.ProgramaId);
                curso.HasOne(c => c.Tutor) .WithMany(t => t.Cursos).HasForeignKey(c => c.TutorId)  .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Estudiante>(estudiante =>
            {
                estudiante.HasKey(e => e.Id);
                estudiante.Property(e => e.Nombre).IsRequired();
                estudiante.Property(e => e.Apellidos).IsRequired();
                estudiante.Property(e => e.Correo).IsRequired();
                estudiante.Property(e => e.FechaNacimiento).IsRequired();
            });

            modelBuilder.Entity<Evaluacion>(evaluacion =>
            {
                evaluacion.HasKey(e => e.Id);
                evaluacion.HasOne(e => e.Curso).WithMany(c => c.Evaluaciones).HasForeignKey(e => e.CursoId);
                evaluacion.HasOne(e => e.Estudiante).WithMany(est => est.Evaluaciones).HasForeignKey(e => e.EstudianteId);
            });

            modelBuilder.Entity<Programa>(programa =>
            {
                programa.HasKey(p => p.Id);
                programa.Property(p => p.Nombre).IsRequired();
            });

            modelBuilder.Entity<ProgresoEstudiante>(progresoEstudiante =>
            {
                progresoEstudiante.HasKey(p => p.Id);
                progresoEstudiante.HasOne(p => p.Estudiante).WithMany(e => e.Progresos).HasForeignKey(p => p.EstudianteId);
                progresoEstudiante.HasOne(p => p.Curso).WithMany(c => c.Progresos).HasForeignKey(p => p.CursoId);
            });

            modelBuilder.Entity<Rol>(rol =>
            {
                rol.HasKey(r => r.Id);
                rol.Property(r => r.NombreRol).IsRequired();
            });

            modelBuilder.Entity<Tutor>(tutor =>
            {
                tutor.HasKey(t => t.Id);
                tutor.Property(t => t.Nombre).IsRequired();
                tutor.Property(t => t.Apellidos).IsRequired();
                tutor.Property(t => t.Correo).IsRequired();
            });

            modelBuilder.Entity<Usuario>(usuario =>
            {
                usuario.HasKey(u => u.Id);
                usuario.Property(u => u.NombreUsuario).IsRequired();
                usuario.Property(u => u.Contraseña).IsRequired();
                usuario.HasOne(u => u.Rol).WithMany(r => r.Usuarios).HasForeignKey(u => u.RolId);
            });

            modelBuilder.Entity<Matricula>(matricula =>
            {
                matricula.HasKey(m => m.Id);
                matricula.Property(m => m.FechaInscripcion).IsRequired();
                matricula.HasOne(m => m.Estudiante).WithMany(e => e.Matriculas).HasForeignKey(m => m.EstudianteId);
                matricula.HasOne(m => m.Curso).WithMany(c => c.Matriculas).HasForeignKey(m => m.CursoId);
            });

        }


    }
}
