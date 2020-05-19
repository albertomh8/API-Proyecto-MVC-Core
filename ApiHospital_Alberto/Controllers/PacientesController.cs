using ApiHospital_Alberto.Repositories;
using HospitalNuget.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ApiHospital_Alberto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacientesController : ControllerBase
    {
        IRepositoryHospital repo;
        public PacientesController(IRepositoryHospital repo)
        {
            this.repo = repo;
        }

        [Authorize]
        [HttpGet]
        [Route("[action]/{userLogged}")]
        public ActionResult<Paciente> FindPaciente(int userLogged)
        {
            return repo.FindPaciente(userLogged);
        }

        [Authorize]
        [HttpGet]
        [Route("[action]/{pacienteId}")]
        public ActionResult<Paciente> FindPacienteById(int pacienteId)
        {
            return repo.FindPacienteById(pacienteId);
        }

        [Authorize]
        [HttpPost]
        public void CrearPaciente(Paciente paci)
        {
            repo.CrearPaciente(paci.DNI, paci.Nombre, paci.Apellidos, paci.Fecha_Nacimiento, paci.Sexo, paci.Telefono, paci.Ciudad, paci.Direccion,
                paci.CP, paci.Email, paci.NSS, paci.UserId);
        }

        [Authorize]
        [HttpPut]
        public void EditarPaciente(Paciente paci)
        {
            repo.EditarPaciente(paci.PacienteId, paci.DNI, paci.Nombre, paci.Apellidos, paci.Fecha_Nacimiento, paci.Sexo, paci.Telefono, paci.Ciudad, paci.Direccion,
                paci.CP, paci.Email, paci.NSS);
        }

        [HttpGet]
        [Route("[action]/{fechaNacimiento}")]
        public ActionResult<int> EdadPaciente(DateTime fechaNacimiento)
        {
            return repo.EdadPaciente(fechaNacimiento);
        }
    }
}