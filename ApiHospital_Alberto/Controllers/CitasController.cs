using ApiHospital_Alberto.Repositories;
using HospitalNuget.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ApiHospital_Alberto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitasController : ControllerBase
    {
        IRepositoryHospital repo;
        public CitasController(IRepositoryHospital repo)
        {
            this.repo = repo;
        }

        [Authorize]
        [HttpGet]
        [Route("HistorialCitasPaciente/{userLogged}")]
        public ActionResult<List<Cita>> GetTodasCitasPaciente(int userLogged)
        {
            return repo.GetTodasCitasPaciente(userLogged);
        }

        [Authorize]
        [HttpGet]
        [Route("CitasPaciente/{userLogged}")]
        public ActionResult<List<Cita>> GetCitasPaciente(int userLogged)
        {
            return repo.GetCitasPaciente(userLogged);
        }

        [Authorize]
        [HttpPost]
        [Route("CrearCita")]
        public void CrearCita(Cita cita)
        {
            repo.CrearCita(cita.PacienteId, cita.Fecha, cita.Hora, cita.PersonalId);
        }

        [Authorize]
        [HttpDelete]
        [Route("AnularCita/{citaId}")]
        public void AnularCita(int citaId)
        {
            repo.AnularCita(citaId);
        }

        [Authorize]
        [HttpPut]
        [Route("CambiarCita")]
        public void CambiarCita(Cita cita)
        {
            repo.CambiarCita(cita.CitaId, cita.Fecha, cita.Hora);
        }

        [HttpGet]
        [Route("CitasCaducadas/{userLogged}/{fecha}")]
        public string CheckCitasCaducadas(int userLogged, DateTime fecha)
        {
            return repo.CheckCitasCaducadas(userLogged, fecha);
        }

        [Authorize]
        [HttpGet]
        [Route("CitasDia/{personalId}/{fecha}/{pacienteId?}")]
        public ActionResult<List<Cita>> CheckCitaInDay(int personalId, DateTime fecha, int? pacienteId = null)
        {
            return repo.CheckCitaInDay(personalId, fecha, pacienteId);
        }

        [Authorize]
        [HttpGet]
        [Route("{citaId}")]
        public ActionResult<Cita> FindCita(int citaId)
        {
            return repo.FindCita(citaId);
        }
    }
}