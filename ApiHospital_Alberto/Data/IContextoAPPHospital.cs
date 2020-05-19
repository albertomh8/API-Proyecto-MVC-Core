using HospitalNuget.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiHospital_Alberto.Data
{
    public interface IContextoAPPHospital
    {
        int SaveChanges();
        DbSet<Usuarios> Usuarios { get; set; }
        DbSet<Paciente> Pacientes { get; set; }
        DbSet<Personal> Personal { get; set; }
        DbSet<Especialidad> Especialidades { get; set; }
        DbSet<Cita> Citas { get; set; }
        DbSet<Informe> Informes { get; set; }
    }
}
