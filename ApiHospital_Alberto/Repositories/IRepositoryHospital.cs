using HospitalNuget.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiHospital_Alberto.Repositories
{
    public interface IRepositoryHospital
    {
        #region Administrador
        List<Usuarios> GetUsers();
        List<ValidationResult> Validate(string email);
        Usuarios GetUserLogin(int userLogged);
        Usuarios ExisteUsuario(string email, string password);
        Usuarios FindUser(int userId);
        string ExisteTipoUsuario(int userLogged);
        string CrearPerfilUsuario(int userLogged);
        void CrearUsuario(string email, string password, string confirmarPass, Role role);
        void EditarUsuario(int userId, string email, string password, Role role, bool activo);
        #endregion

        #region Pacientes
        Paciente FindPaciente(int userLogged);
        Paciente FindPacienteById(int pacienteId);
        void CrearPaciente(string dni, string nombre, string apellidos, DateTime fecha_nac, Sexo sexo, string telefono, string ciudad,
            string direccion, int cp, string email, Int64 nss, int userId);
        void EditarPaciente(int pacienteId, string dni, string nombre, string apellidos, DateTime fecha_nac, Sexo sexo, string telefono, string ciudad,
            string direccion, int cp, string email, Int64 nss);
        string CheckCitasCaducadas(int userLogged, DateTime fecha);
        List<Cita> GetTodasCitasPaciente(int userLogged);
        List<Cita> GetCitasPaciente(int userLogged);
        List<Cita> CheckCitaInDay(int selectedPersonal, DateTime fecha, int? paciente);
        Cita FindCita(int citaId);
        void CrearCita(int pacienteId, DateTime fecha, DateTime hora, int personalId);
        void AnularCita(int citaId);
        void CambiarCita(int citaId, DateTime fecha, DateTime hora);
        int EdadPaciente(DateTime fechaNacimiento);

        #endregion

        #region Personal
        void CrearPersonal(string dni, string nombre, string apellidos, DateTime fecha_nac, string telefono, string ciudad,
           string direccion, string email, int numColegiado, Turno turno, int especialidadId, int userId);
        Personal FindPersonal(int userLogged);
        Personal FindPersonalById(int persnonalId);
        void EditarPersonal(int personalId, string dni, string nombre, string apellidos, DateTime fecha_nac, string telefono, string ciudad,
            string direccion, string email, int numColegiado, Turno turno, int especialidadId);
        List<Personal> GetPersonal();
        List<Personal> GetPersonalByTurno(Turno turno);
        string GetTurnoPersonal(int personalId);
        List<Especialidad> GetEspecialidades();
        Especialidad GetEspecialidadPersonal(int especialidadId);
        List<string> GetAllTurnos();
        List<DateTime> GetCitasConcertadas(int personalId, DateTime fecha);
        List<DateTime> GetCitasLibres(int personalId, Turno turno, DateTime fecha);
        List<DateTime> HorasDisponibles(DateTime startDate, DateTime endDate, List<DateTime> citas);
        #endregion

        #region Informes
        List<Informe> GetInformes(int pacienteId);
        Informe GetDetallesInforme(int informeId);
        void CrearInforme(int pacienteId, int personalId, DateTime fecha, string descripcion, string diagnostico);
        void EditarInforme(int informeId, string descripcion, string diagnostico);
        int UltimoInformeId();
        #endregion
    }
}
