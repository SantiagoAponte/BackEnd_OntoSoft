using System.Collections.Generic;
using System.Threading.Tasks;
using Aplication.Interfaces;
using Aplication.Interfaces.Contracts;
using Aplication.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Aplication.Security.Users;
using Aplication.Security.Users.Dtos;
using Domine;
using MediatR;

namespace WebApi.Controllers
{
     [ApiController]
    // [AllowAnonymous]
        public class userController : myControllerBase
    {
         private IForgetPassword _forgetPassword;
        private IMailService _mailService;
        private IConfiguration _configuration;
        public userController(IForgetPassword forgetPassword, IMailService mailService, IConfiguration configuration)
        {
            _forgetPassword = forgetPassword;
            _mailService = mailService;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<userLoginDto>> Login(Login.Execute data){
            return await mediator.Send(data);
        }
        // https://localhost:5000/api/user/register
        [AllowAnonymous]
        [HttpPost("register")] 
        public async Task<ActionResult<userRegisterDto>> Register(UserRegister.Execute data){
            return await mediator.Send(data);
        }


        //https://localhost:5000/api/user/forgetpassword
        
        [HttpGet("forgetpassword")] [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword(string email, string host)
        {
            if (string.IsNullOrEmpty(email))
                return NotFound();

            var result = await _forgetPassword.ForgetPasswordAsync(email, host);

            if (result.IsSuccess)
                return Ok(result); // 200

            return BadRequest(result); // 400
        }

        [HttpPut("resetpassword")] 
        [AllowAnonymous]
        public async Task<ActionResult<UserManagerResponse>> ResetPassword(ResetPassword.Execute data){
            return await mediator.Send(data);
        }

        //https://localhost:5000/api/user/
        [HttpGet]
        // [Authorize (Roles = "SuperAdmin")]
        // [Authorize (Roles = "Paciente")]
        // [Authorize (Roles = "Recepcionista")]
        // [Authorize (Roles = "Doctor")]
        public async Task<ActionResult<UserData>> ObtainUser (){
            return await mediator.Send(new UserActually.Execute());
        }

        // https://localhost:5000/api/user/edit
        [HttpPut("edit")]
        public async Task<ActionResult<UserData>> Actualizar(UserPut.Execute data){
           return await mediator.Send(data);     
        }
    
        // https://localhost:5000/api/user/allusers
        [HttpGet("allusers")]
        public async Task<ActionResult<List<UserPrueba>>> Get(){
            return await mediator.Send(new getAllUsers.ListUsers());
        }

        // https://localhost:5000/api/user/typedocument
        [HttpGet("typedocument")]
        public async Task<ActionResult<List<typeDocument>>> GetResult(){

            return await mediator.Send(new getTypeDocument.ListTypeDocument());
        }
        [HttpGet("details/{id}")]
        public async Task<ActionResult<UserData>> ObtainDetailsWithUser(string id){
            return await mediator.Send(new getUserDetails.OneDetailUser{Id = id});
        }
       [HttpDelete("delete/{id}")]
        public async Task<ActionResult<Unit>> deleteUser(string id){
            return await mediator.Send(new deleteUser.Execute{Id = id});
        }
        [HttpPost("edit/{id}")]
        public async Task<ActionResult<Unit>> putUserforAdmin(string id, UserPutForAdmin.Execute data){
            data.Id = id;
            return await mediator.Send(data);
        }

    }
}