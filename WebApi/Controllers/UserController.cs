using System.Threading.Tasks;
using Aplication.Interfaces;
using Aplication.Interfaces.Contracts;
using Aplication.Security;
using Domine;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace WebApi.Controllers
{
     [ApiController]
    // [AllowAnonymous]
        public class UserController : myControllerBase
    {
         private IForgetPassword _forgetPassword;
        private IMailService _mailService;
        private IConfiguration _configuration;
        public UserController(IForgetPassword forgetPassword, IMailService mailService, IConfiguration configuration)
        {
            _forgetPassword = forgetPassword;
            _mailService = mailService;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<UserData>> Login(Login.Execute data){
            return await mediator.Send(data);
        }
        // https://localhost:5000/api/user/register
        [AllowAnonymous]
        [HttpPost("Register")] 
        public async Task<ActionResult<UserData>> Register(UserRegister.Execute data){
            return await mediator.Send(data);
        }


        //https://localhost:5000/api/User/ForgetPassword
        
        [HttpPost("ForgetPassword")] [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                return NotFound();

            var result = await _forgetPassword.ForgetPasswordAsync(email);

            if (result.IsSuccess)
                return Ok(result); // 200

            return BadRequest(result); // 400
        }

        [HttpPut("ResetPassword")] 
        [AllowAnonymous]
        public async Task<ActionResult<UserManagerResponse>> ResetPassword(ResetPassword.Execute data){
            return await mediator.Send(data);
        }

        //https://localhost:5000/api/User/
        [HttpGet]
        [Authorize (Roles = "SuperAdmin")]
        [Authorize (Roles = "Paciente")]
        [Authorize (Roles = "Recepcionista")]
        [Authorize (Roles = "Doctor")]
        public async Task<ActionResult<UserData>> ObtainUser (){
            return await mediator.Send(new UserActually.Execute());
        }

        // https://localhost:5000/api/user/edit
        [HttpPut("Edit")]
        public async Task<ActionResult<UserData>> Actualizar(UserPut.Execute data){
           return await mediator.Send(data);     
        }

        // [HttpGet]
        // [Route("Sendmail")]
        // public async  Task<IActionResult> Sendmail(){
        //     await _mailService.SendEmailAsync("ontosoft5@gmail.com", "Prueba Mail", "<h1> Esta es una prueba del servicio mail</h1>");
        //     return Ok();
        // }
        
    
        // https://localhost:5000/api/user/
        // [HttpGet]
        // public async Task<ActionResult<UserData>> ReturnUser(){
        //     return await Mediator.Send(new UserData.Execute());
        // }

    }
}