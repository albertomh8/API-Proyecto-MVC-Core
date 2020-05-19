using ApiHospital_Alberto.Repositories;
using HospitalNuget.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ApiHospital_Alberto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspecialidadesController : ControllerBase
    {
        IRepositoryHospital repo;
        public EspecialidadesController(IRepositoryHospital repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public ActionResult<List<Especialidad>> GetEspecialidades()
        {
            return repo.GetEspecialidades();
        }

        [HttpGet]
        [Route("{especialidadId}")]
        public ActionResult<Especialidad> GetEspecialidadPersonal(int especialidadId)
        {
            return repo.GetEspecialidadPersonal(especialidadId);
        }
    }
}