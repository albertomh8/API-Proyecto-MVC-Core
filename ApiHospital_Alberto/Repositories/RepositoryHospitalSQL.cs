using ApiHospital_Alberto.Data;
using ApiHospital_Alberto.Helpers;
using HospitalNuget.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ApiHospital_Alberto.Repositories
{
    public class IRepositoryHospitalSQL : IRepositoryHospital
    {
        IContextoAPPHospital db;
        public IRepositoryHospitalSQL(IContextoAPPHospital db)
        {
            this.db = db;
        }

        #region Citas
        public List<Cita> GetTodasCitasPaciente(int userLogged)
        {
            var citas = db.Citas
                .Where(c => c.Paciente.User.UserId == userLogged)
                .Select(c => c)
                .ToList();

            return citas;
        }

        public List<Cita> GetCitasPaciente(int userLogged)
        {
            DateTime fechaActual = DateTime.Now.Date;
            DateTime horaActual = DateTime.Now.ToLocalTime();
            var citas = db.Citas
                .Where(c => c.Paciente.User.UserId == userLogged && c.Caducada == false)
                .Select(c => new Cita
                {
                    CitaId = c.CitaId,
                    Fecha = c.Fecha,
                    Hora = c.Hora,
                    Caducada = c.Caducada,
                    Paciente = c.Paciente,
                    PacienteId = c.PacienteId,
                    Personal = c.Personal,
                    PersonalId = c.PersonalId
                }).ToList();


            List<Cita> removeCitas = new List<Cita>();
            foreach (Cita c in citas)
            {
                c.Personal = db.Personal.SingleOrDefault(p => p.PersonalId == c.PersonalId);
                c.Personal.Especialidad = db.Especialidades.SingleOrDefault(e => e.EspecialidadId == c.Personal.EspecialidadId);
                if (c.Fecha < fechaActual || c.Fecha == fechaActual && c.Hora < horaActual)
                {
                    c.Caducada = true;
                    db.SaveChanges();
                    removeCitas.Add(c);
                }
            }
            citas.RemoveAll(c => removeCitas.Contains(c));

            return citas;
        }

        public void CrearCita(int pacienteId, DateTime fecha, DateTime hora, int personalId)
        {
            Cita cita = new Cita();
            cita.CitaId = GetMaxIdCita();
            cita.PacienteId = pacienteId;
            cita.PersonalId = personalId;
            cita.Fecha = fecha;
            cita.Hora = hora;
            cita.Caducada = false;

            db.Citas.Add(cita);
            db.SaveChanges();
        }

        public void AnularCita(int citaId)
        {
            var cita = db.Citas.SingleOrDefault(c => c.CitaId == citaId);

            db.Citas.Remove(cita);
            db.SaveChanges();
        }

        public void CambiarCita(int citaId, DateTime fecha, DateTime hora)
        {
            Cita cita = FindCita(citaId);
            cita.Fecha = fecha;
            cita.Hora = hora;
            cita.Caducada = false;

            db.Citas.Update(cita);
            db.SaveChanges();
        }

        public string CheckCitasCaducadas(int userLogged, DateTime fecha)
        {
            List<Cita> citasPaciente = GetTodasCitasPaciente(userLogged);
            foreach (Cita c in citasPaciente)
            {
                DateTime date = c.Fecha;
                if (fecha > date)
                {
                    c.Caducada = true;
                }
            }

            db.Citas.UpdateRange(citasPaciente);
            db.SaveChanges();
            return "Citas caducadas actualizadas";
        }

        public List<Cita> CheckCitaInDay(int selectedPersonal, DateTime fecha, int? paciente)
        {
            List<Cita> citasMedico;
            if (paciente.GetValueOrDefault() != 0)
            {
                citasMedico = db.Citas
                .Where(c => c.PersonalId == selectedPersonal && c.Fecha == fecha && c.PacienteId == paciente)
                .Select(c => c)
                .ToList();
            }
            else
            {
                citasMedico = db.Citas
                .Where(c => c.PersonalId == selectedPersonal && c.Fecha == fecha)
                .Select(c => new Cita 
                { 
                    Caducada = c.Caducada,
                    CitaId = c.CitaId,
                    Fecha = c.Fecha,
                    Hora = c.Hora,
                    PacienteId = c.PacienteId,
                    Paciente = c.Paciente,
                    PersonalId = c.PersonalId,
                    Personal = c.Personal
                }).ToList();
            }
            return citasMedico;
        }

        public Cita FindCita(int citaId)
        {
            var cita = db.Citas.SingleOrDefault(c => c.CitaId == citaId);

            return cita;
        }

        #endregion

        #region Paciente

        public Paciente FindPaciente(int userLogged)
        {
            var paciente = db.Pacientes.SingleOrDefault(p => p.UserId == userLogged);

            return paciente;
        }

        public Paciente FindPacienteById(int pacienteId)
        {
            var paciente = db.Pacientes.SingleOrDefault(p => p.PacienteId == pacienteId);
            paciente.Informes = db.Informes.Where(i => i.PacienteId == paciente.PacienteId).ToList();

            return paciente;
        }

        public void CrearPaciente(string dni, string nombre, string apellidos, DateTime fecha_nac, Sexo sexo, string telefono, string ciudad,
            string direccion, int cp, string email, Int64 nss, int userId)
        {
            Paciente p = new Paciente();
            p.PacienteId = GetMaxIdPaciente();
            p.DNI = dni;
            p.Nombre = nombre;
            p.Apellidos = apellidos;
            p.Fecha_Nacimiento = fecha_nac;
            p.Sexo = sexo;
            p.Telefono = telefono;
            p.Ciudad = ciudad;
            p.Direccion = direccion;
            p.CP = cp;
            p.Email = email;
            p.NSS = nss;
            p.UserId = userId;

            db.Pacientes.Add(p);
            db.SaveChanges();
        }

        public void EditarPaciente(int pacienteId, string dni, string nombre, string apellidos, DateTime fecha_nac, Sexo sexo, string telefono, string ciudad,
             string direccion, int cp, string email, Int64 nss)
        {
            Paciente p = FindPacienteById(pacienteId);
            p.DNI = dni;
            p.Nombre = nombre;
            p.Apellidos = apellidos;
            p.Fecha_Nacimiento = fecha_nac;
            p.Sexo = sexo;
            p.Telefono = telefono;
            p.Ciudad = ciudad;
            p.Direccion = direccion;
            p.CP = cp;
            p.Email = email;
            p.NSS = nss;

            db.Pacientes.Update(p);
            db.SaveChanges();
        }

        public int EdadPaciente(DateTime fechaNacimiento)
        {
            int edad = DateTime.Today.Year - fechaNacimiento.Year;
            if (DateTime.Today < fechaNacimiento.AddYears(edad))
                --edad;

            return edad;
        }

        #endregion

        #region Personal
        public void CrearPersonal(string dni, string nombre, string apellidos, DateTime fecha_nac, string telefono, string ciudad,
            string direccion, string email, int numColegiado, Turno turno, int especialidadId, int userId)
        {
            Personal p = new Personal();
            p.PersonalId = GetMaxIdPersonal();
            p.DNI = dni;
            p.Nombre = nombre;
            p.Apellidos = apellidos;
            p.Fecha_Nacimiento = fecha_nac;
            p.Telefono = telefono;
            p.Ciudad = ciudad;
            p.Direccion = direccion;
            p.Email = email;
            p.NumColegiado = numColegiado;
            p.Turno = turno;
            p.EspecialidadId = especialidadId;
            p.UserId = userId;

            db.Personal.Add(p);
            db.SaveChanges();
        }

        public Personal FindPersonal(int userLogged)
        {
            var personal = db.Personal.SingleOrDefault(p => p.UserId == userLogged);
            if (personal != null)
            {
                personal.Especialidad = db.Especialidades.Where(e => e.EspecialidadId == personal.EspecialidadId).FirstOrDefault();
            }

            return personal;
        }

        public Personal FindPersonalById(int personalId)
        {
            var personal = db.Personal.SingleOrDefault(p => p.PersonalId == personalId);
            personal.Especialidad = db.Especialidades.Where(e => e.EspecialidadId == personal.EspecialidadId).FirstOrDefault();

            return personal;
        }

        public void EditarPersonal(int personalId, string dni, string nombre, string apellidos, DateTime fecha_nac, string telefono, string ciudad,
            string direccion, string email, int numColegiado, Turno turno, int especialidadId)
        {
            Personal p = FindPersonalById(personalId);
            p.DNI = dni;
            p.Nombre = nombre;
            p.Apellidos = apellidos;
            p.Fecha_Nacimiento = fecha_nac;
            p.Telefono = telefono;
            p.Ciudad = ciudad;
            p.Direccion = direccion;
            p.Email = email;
            p.NumColegiado = numColegiado;
            p.Turno = turno;
            p.EspecialidadId = especialidadId;

            db.Personal.Update(p);
            db.SaveChanges();
        }

        public List<Especialidad> GetEspecialidades()
        {
            var especiliadades = db.Especialidades
                .Select(e => e).Distinct().ToList();

            return especiliadades;
        }

        public Especialidad GetEspecialidadPersonal(int especialidadId)
        {
            var especialidad = db.Especialidades
                .Where(e => e.EspecialidadId == especialidadId)
                .Select(e => e)
                .FirstOrDefault();

            return especialidad;
        }

        public List<string> GetAllTurnos()
        {
            var turnos = Enum.GetNames(typeof(Turno));

            return turnos.ToList();
        }

        public List<DateTime> GetCitasConcertadas(int personalId, DateTime fecha)
        {
            var citas = db.Citas
              .Where(c => c.PersonalId == personalId && c.Fecha == fecha)
              .Select(c => c.Hora)
              .ToList();

            return citas;
        }

        public List<DateTime> GetCitasLibres(int personalId, Turno turno, DateTime fecha)
        {
            //Saca las citas disponibles para el médico seleccionado
            List<DateTime> citas = GetCitasConcertadas(personalId, fecha);
            DateTime startDate;
            DateTime endDate;
            List<DateTime> horas = new List<DateTime>();
            switch (turno)
            {
                case Turno.Mañana:
                    startDate = fecha.AddHours(9);
                    endDate = fecha.AddHours(15);
                    horas = HorasDisponibles(startDate, endDate, citas);
                    break;
                case Turno.Tarde:
                    startDate = fecha.AddHours(15);
                    endDate = fecha.AddHours(21);
                    horas = HorasDisponibles(startDate, endDate, citas);
                    break;
            }

            return horas;
        }

        public List<DateTime> HorasDisponibles(DateTime startDate, DateTime endDate, List<DateTime> citas)
        {
            //Listado de horas disponibles para el médico
            DateTime horaActual;
            List<DateTime> horas = new List<DateTime>();
            horaActual = startDate;

            do
            {
                if (horaActual > DateTime.Now)
                {
                    horas.Add(horaActual);
                }
                horaActual = horaActual.AddMinutes(15);
            } while (horaActual < endDate);

            foreach (DateTime hora in citas)
            {
                horas.Remove(hora);
            }

            return horas;
        }

        public List<Personal> GetPersonal()
        {
            var personal = db.Personal.Select(p => p).ToList();

            return personal;
        }

        public List<Personal> GetPersonalByTurno(Turno turno)
        {
            var personal = db.Personal
                .Where(p => p.NombreTurno == turno.ToString())
                .Select(p => p)
                .ToList();

            return personal;
        }

        public string GetTurnoPersonal(int personalId)
        {
            var turno = db.Personal
                .Where(p => p.PersonalId == personalId)
                .Select(p => p.NombreTurno)
                .FirstOrDefault();

            return turno;
        }

        #endregion

        #region Informes

        public Informe GetDetallesInforme(int informeId)
        {
            var informe = db.Informes
                .Where(i => i.InformeId == informeId)
                .Select(i => new Informe
                {
                    InformeId = i.InformeId,
                    Fecha = i.Fecha,
                    Descripcion = i.Descripcion,
                    Diagnostico = i.Diagnostico,
                    Personal = i.Personal,
                    PersonalId = i.PersonalId,
                    Paciente = i.Paciente,
                    PacienteId = i.PacienteId
                }).FirstOrDefault();
            informe.Personal.Especialidad = db.Especialidades.Where(e => e.EspecialidadId == informe.Personal.EspecialidadId).FirstOrDefault();

            return informe;
        }
        public List<Informe> GetInformes(int pacienteId)
        {
            var informes = db.Informes
                .Where(i => i.Paciente.PacienteId == pacienteId)
                .Select(i => new Informe { 
                    InformeId = i.InformeId,
                    Fecha = i.Fecha,
                    Descripcion = i.Descripcion,
                    Diagnostico = i.Diagnostico,
                    Personal = i.Personal,
                    PersonalId = i.PersonalId,
                    Paciente = i.Paciente,
                    PacienteId = i.PacienteId
                }).ToList();

            return informes;
        }
        public void CrearInforme(int pacienteId, int personalId, DateTime fecha, string descripcion, string diagnostico)
        {
            Informe informe = new Informe();
            informe.InformeId = GetMaxIdInforme();
            informe.PacienteId = pacienteId;
            informe.PersonalId = personalId;
            informe.Fecha = fecha;
            informe.Descripcion = descripcion;
            informe.Diagnostico = diagnostico;

            db.Informes.Add(informe);
            db.SaveChanges();
        }

        public void EditarInforme(int informeId, string descripcion, string diagnostico)
        {
            Informe informe = GetDetallesInforme(informeId);
            informe.Descripcion = descripcion;
            informe.Diagnostico = diagnostico;

            db.Informes.Update(informe);
            db.SaveChanges();
        }

        public int UltimoInformeId()
        {
            int UltimoInformeId = db.Informes
               .OrderByDescending(i => i.InformeId)
               .Select(e => e.InformeId)
               .FirstOrDefault();

            return UltimoInformeId;
        }

        #endregion

        #region Administrador
        public List<ValidationResult> Validate(string email)
        {
            List<ValidationResult> validationResult = new List<ValidationResult>();
            var validateName = db.Usuarios.FirstOrDefault(x => x.Email.ToLower() == email.ToLower());
            if (validateName != null)
            {
                ValidationResult errorMessage = new ValidationResult
                ("Ya existe un usuario registrado con ese email.", new[] { "Email" });
                validationResult.Add(errorMessage);
                return validationResult;
            }
            else
            {
                return validationResult;
            }
        }

        public Usuarios GetUserLogin(int userLogged)
        {
            var user = db.Usuarios.SingleOrDefault(x => x.UserId == userLogged);
            return user;
        }

        public Usuarios ExisteUsuario(string email, string password)
        {
            //Comprobamos si existe el usuario, si existe comprobamos
            //si la contraseña es correcta y de ser así devolvemos el usuario
            Usuarios user = db.Usuarios.SingleOrDefault(e => e.Email == email && e.Activo == true);
            if (user != null)
            {
                string salt = user.Salt;
                string cifrado = HelperCifrado.CifrarPassword(password, salt);
                bool resultado = HelperCifrado.CompararBytes(cifrado, user.Password);
                if (resultado) return user;
                else return null;
            }
            return null;
        }

        public string ExisteTipoUsuario(int userLogged)
        {
            //Buscar el usuario logeado y devuelve un string con el rol del usuario
            var user = FindUser(userLogged);
            var existePaciente = db.Pacientes.SingleOrDefault(p => p.UserId == userLogged);
            var existePersonal = db.Personal.SingleOrDefault(p => p.UserId == userLogged);

            if (existePaciente != null && user.Role == Role.Paciente) return "Paciente";
            else if (existePersonal != null && user.Role == Role.Médico) return "Personal";
            else return "SinPerfil";
        }

        public string CrearPerfilUsuario(int userLogged)
        {
            var user = FindUser(userLogged);
            var existePaciente = db.Pacientes.SingleOrDefault(p => p.UserId == userLogged);

            if (existePaciente == null && user.Role == Role.Paciente) return "PacieNoPerfil";
            else return "PersoNoPerfil";
        }

        public Usuarios FindUser(int userLogged)
        {
            var user = db.Usuarios.SingleOrDefault(u => u.UserId == userLogged);

            return user;
        }

        public List<Usuarios> GetUsers()
        {
            var users = db.Usuarios
                .Select(u => u).ToList();

            return users;
        }

        public void CrearUsuario(string email, string password, string confirmarPass, Role role)
        {
            string salt = HelperCifrado.GenerarSalt();
            string passCifrada = HelperCifrado.CifrarPassword(password, salt);
            string confirmPassCifrada = HelperCifrado.CifrarPassword(confirmarPass, salt);
            Usuarios user = new Usuarios();
            user.UserId = GetMaxIdUsuario();
            user.Email = email;
            user.Password = passCifrada;
            user.ComparePassword = confirmPassCifrada;
            user.Role = role;
            user.Activo = true;
            user.Salt = salt;

            db.Usuarios.Add(user);
            db.SaveChanges();
        }

        public void EditarUsuario(int userId, string email, string password, Role role, bool activo)
        {
            //La password de cofirmacion es la password porque no esta mapeada en la bbdd
            Usuarios user = FindUser(userId);
            user.Email = email;
            user.Password = password;
            user.ComparePassword = password;
            user.Role = role;
            user.Activo = activo;

            db.Usuarios.Update(user);
            db.SaveChanges();
        }

        #endregion

        //Métodos creados para usarlos si la bbdd no tiene autoincremental en las primary key.
        private int GetMaxIdUsuario()
        {
            int ultimoUsuario = db.Usuarios
               .OrderByDescending(e => e.UserId)
               .Select(e => e.UserId)
               .FirstOrDefault();

            return ultimoUsuario + 1;
        }
        private int GetMaxIdPersonal()
        {
            int ultimoPersonal = db.Personal
               .OrderByDescending(e => e.PersonalId)
               .Select(e => e.PersonalId)
               .FirstOrDefault();

            return ultimoPersonal + 1;
        }
        private int GetMaxIdPaciente()
        {
            int ultimoPaciente = db.Pacientes
               .OrderByDescending(e => e.PacienteId)
               .Select(e => e.PacienteId)
               .FirstOrDefault();

            return ultimoPaciente + 1;
        }
        private int GetMaxIdInforme()
        {
            int ultimoInforme = db.Informes
               .OrderByDescending(e => e.InformeId)
               .Select(e => e.InformeId)
               .FirstOrDefault();

            return ultimoInforme + 1;
        }
        private int GetMaxIdEspecialidad()
        {
            int ultimaEspecialidad = db.Especialidades
               .OrderByDescending(e => e.EspecialidadId)
               .Select(e => e.EspecialidadId)
               .FirstOrDefault();

            return ultimaEspecialidad + 1;
        }
        private int GetMaxIdCita()
        {
            int ultimaCita = db.Citas
               .OrderByDescending(e => e.CitaId)
               .Select(e => e.CitaId)
               .FirstOrDefault();

            return ultimaCita + 1;
        }
    }
}
