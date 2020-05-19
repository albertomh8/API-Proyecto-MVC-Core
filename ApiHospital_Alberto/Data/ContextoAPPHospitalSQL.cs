using ApiHospital_Alberto.MethodExtensions;
using HospitalNuget.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiHospital_Alberto.Data
{
    public class ContextoAPPHospitalSQL : DbContext, IContextoAPPHospital
    {
        public ContextoAPPHospitalSQL(DbContextOptions<ContextoAPPHospitalSQL> options) : base(options) { }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Personal> Personal { get; set; }
        public DbSet<Especialidad> Especialidades { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<Informe> Informes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ////Quitar la pluralización de nombres(En EF Core RC2 no contiene la propiedad para remover
            //la pluralziación de nombres que tenia antes, una de las soluciones es crearse un método personalizado que lo haga).
            modelBuilder.RemovePluralizingTableNameConvention();

            modelBuilder.Entity<Personal>()
                .HasOne(p => p.User)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired();

            modelBuilder.Entity<Paciente>()
                .HasOne(s => s.User)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired();
        }
    }
}
