using ApiHospital_Alberto.Repositories;
using HospitalNuget.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ApiHospital_Alberto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InformesController : ControllerBase
    {
        IRepositoryHospital repo;
        public InformesController(IRepositoryHospital repo)
        {
            this.repo = repo;
        }

        [Authorize]
        [HttpGet("{pacienteId}")]
        public ActionResult<List<Informe>> GetInformes(int pacienteId)
        {
            return repo.GetInformes(pacienteId);
        }

        [Authorize]
        [HttpGet]
        [Route("DetallesInforme/{informeId}")]
        public ActionResult<Informe> GetDetallesInforme(int informeId)
        {
            return repo.GetDetallesInforme(informeId);
        }

        [Authorize]
        [HttpPost]
        public void CrearInforme(Informe informe)
        {
            repo.CrearInforme(informe.PacienteId, informe.PersonalId, informe.Fecha, informe.Descripcion, informe.Diagnostico);
        }

        [Authorize]
        [HttpPut]
        public void EditarInforme(Informe informe)
        {
            repo.EditarInforme(informe.InformeId, informe.Descripcion, informe.Diagnostico);
        }

        [HttpGet]
        [Route("UltimoInforme")]
        public ActionResult<int> UltimoInformeId()
        {
            return repo.UltimoInformeId();
        }
    }
}