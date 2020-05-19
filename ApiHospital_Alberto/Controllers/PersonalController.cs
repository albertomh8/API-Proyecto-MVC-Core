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
    public class PersonalController : ControllerBase
    {
        IRepositoryHospital repo;
        public PersonalController(IRepositoryHospital repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public ActionResult<List<Personal>> GetPersonal()
        {
            return repo.GetPersonal();
        }

        [HttpGet]
        [Route("PersonalTurno/{turno}")]
        public ActionResult<List<Personal>> GetPersonalByTurno(Turno turno)
        {
            return repo.GetPersonalByTurno(turno);
        }

        [Authorize]
        [HttpGet]
        [Route("[action]/{userLogged}")]
        public ActionResult<Personal> FindPersonal(int userLogged)
        {
            return repo.FindPersonal(userLogged);
        }

        [Authorize]
        [HttpGet]
        [Route("[action]/{personalId}")]
        public ActionResult<Personal> FindPersonalById(int personalId)
        {
            return repo.FindPersonalById(personalId);
        }

        [Authorize]
        [HttpPost]
        public void CrearPersonal(Personal perso)
        {
            repo.CrearPersonal(perso.DNI, perso.Nombre, perso.Apellidos, perso.Fecha_Nacimiento, perso.Telefono, perso.Ciudad,
                perso.Direccion, perso.Email, perso.NumColegiado, perso.Turno, perso.EspecialidadId, perso.UserId);
        }

        [Authorize]
        [HttpPut]
        public void EditarPersonal(Personal perso)
        {
            repo.EditarPersonal(perso.PersonalId, perso.DNI, perso.Nombre, perso.Apellidos, perso.Fecha_Nacimiento, perso.Telefono, perso.Ciudad,
                perso.Direccion, perso.Email, perso.NumColegiado, perso.Turno, perso.EspecialidadId);
        }

        [HttpGet]
        [Route("Turnos")]
        public ActionResult<List<string>> GetAllTurnos()
        {
            return repo.GetAllTurnos();
        }

        [HttpGet]
        [Route("TurnoPersonal/{personalId}")]
        public ActionResult<string> GetTurnoPersonal(int personalId)
        {
            return repo.GetTurnoPersonal(personalId);
        }

        [HttpGet]
        [Route("CitasConcertadas/{personalId}/{fecha}")]
        public ActionResult<List<DateTime>> GetCitasConcertadas(int personalId, DateTime fecha)
        {
            return repo.GetCitasConcertadas(personalId, fecha);
        }

        [HttpGet]
        [Route("CitasLibres/{personalId}/{turno}/{fecha}")]
        public ActionResult<List<DateTime>> GetCitasLibres(int personalId, Turno turno, DateTime fecha)
        {
            return repo.GetCitasLibres(personalId, turno, fecha);
        }
    }
}