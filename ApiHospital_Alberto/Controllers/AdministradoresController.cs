using ApiHospital_Alberto.Repositories;
using HospitalNuget.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiHospital_Alberto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministradoresController : ControllerBase
    {
        IRepositoryHospital repo;
        public AdministradoresController(IRepositoryHospital repo)
        {
            this.repo = repo;
        }

        [Authorize]
        [HttpGet]
        [Route("Usuarios")]
        public ActionResult<List<Usuarios>> GetUsers()
        {
            return repo.GetUsers();
        }

        [Authorize]
        [HttpGet]
        [Route("Usuarios/{userLogged}")]
        public ActionResult<Usuarios> FindUser(int userLogged)
        {
            return repo.FindUser(userLogged);
        }

        [Authorize]
        [HttpGet]
        [Route("[action]/{email}/{password}")]
        public ActionResult<Usuarios> ExisteUsuario(string email, string password)
        {
            Usuarios usuario = repo.ExisteUsuario(email, password);
            return usuario;
        }

        [HttpGet]
        [Route("[action]/{userLogged}")]
        public ActionResult<string> ExisteTipoUsuario(int userLogged)
        {
            return repo.ExisteTipoUsuario(userLogged);
        }

        [HttpGet]
        [Route("[action]/{userLogged}")]
        public ActionResult<string> CrearPerfilUsuario(int userLogged)
        {
            return repo.CrearPerfilUsuario(userLogged);
        }

        [HttpPost("CrearUsuario")]
        public void CrearUsuario(Usuarios user)
        {
            repo.CrearUsuario(user.Email, user.Password, user.ComparePassword, user.Role);
        }

        [Authorize]
        [HttpPut("EditarUsuario")]
        public void EditarUsuario(Usuarios user)
        {
            repo.EditarUsuario(user.UserId, user.Email, user.Password, user.Role, user.Activo);
        }

        [HttpGet]
        [Route("[action]/{email}")]
        public ActionResult<List<ValidationResult>> Validate(string email)
        {
            return repo.Validate(email);
        }
    }
}