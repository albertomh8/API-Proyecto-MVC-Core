using ApiHospital_Alberto.Helpers;
using ApiHospital_Alberto.Repositories;
using HospitalNuget.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace ApiHospital_Alberto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageController : ControllerBase
    {
        IRepositoryHospital repo;
        HelperToken helper;
        public ManageController(IRepositoryHospital repo, IConfiguration configuration)
        {
            this.repo = repo;
            this.helper = new HelperToken(configuration);
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Login(Usuarios login)
        {
            Usuarios usuario = repo.ExisteUsuario(login.Email, login.Password);
            if (usuario != null)
            {
                Claim[] claims = new[]
                {
                    new Claim("UserData", JsonConvert.SerializeObject(usuario))
                };
                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: helper.issuer,
                    audience: helper.audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(120),
                    notBefore: DateTime.UtcNow,
                    signingCredentials: new SigningCredentials(helper.GetKeyToken(), SecurityAlgorithms.HmacSha256));

                return Ok(new { response = new JwtSecurityTokenHandler().WriteToken(token) });
            }
            else return Unauthorized();
        }

        [Authorize]
        [HttpGet]
        [Route("UserLogin")]
        public ActionResult<Usuarios> GetUserLogin()
        {
            List<Claim> claims = HttpContext.User.Claims.ToList();
            string json = claims.SingleOrDefault(x => x.Type == "UserData").Value;
            Usuarios user = JsonConvert.DeserializeObject<Usuarios>(json);

            return repo.GetUserLogin(user.UserId);
        }
    }
}