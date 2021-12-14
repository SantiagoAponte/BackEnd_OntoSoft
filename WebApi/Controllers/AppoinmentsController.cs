using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aplication.AppoinmentsApp;
using Aplication.AppoinmentsApp.Dtos;
using Aplication.Security;
using Aplication.Security.Users.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class appoinmentsController : myControllerBase
    {
        [HttpGet]
        [Authorize (Roles = "SuperAdmin, Recepcionista")]
        public async Task<ActionResult<List<AppoinmentsDto>>> Get(){

            return await mediator.Send(new GetAppoinment.ListAppoinments());
        }

        
        //http://localhost:5000/api/appoinments/{id}
        [HttpGet("{id}")]
        [Authorize (Roles = "SuperAdmin, Paciente, Recepcionista, Doctor")]
        public async Task<ActionResult<AppoinmentsDto>> ObtainOneAppoinment(Guid id){
            return await mediator.Send(new GetOneAppoinment.OneAppoinment{Id = id});
        }
        
        //Api que trae todas las citas por el ID de un usuario.
        [HttpGet("users/{id}")]
        [Authorize (Roles = "SuperAdmin, Paciente, Recepcionista, Doctor")]
        public async Task<ActionResult<UserDto>> ObtainOneAppoinmentWithUser(string id){
            return await mediator.Send(new GetAppoinmentWithUser.OneAppoinmentUser{Id = id});
        }

        [HttpPost("add")]
        [Authorize (Roles = "SuperAdmin, Recepcionista")]
        public async Task<ActionResult<Unit>> CreateAppoinment(PostAppoinment.Execute data){
            return await mediator.Send(data);
        }



        [HttpPut("edit/{id}")]
        [Authorize (Roles = "SuperAdmin, Recepcionista")]
        public async Task<ActionResult<Unit>> putAppoinment(Guid id, putAppoinments.Execute data){
            data.Id = id;
            return await mediator.Send(data);
        }

        [HttpDelete("delete/{id}")]
        [Authorize (Roles = "SuperAdmin, Recepcionista")]
        public async Task<ActionResult<Unit>> deleteAppoinment(Guid id){
            return await mediator.Send(new deleteAppoinments.Execute{Id = id});
        }

        //  [HttpGet]
        //  [Route("createdAppoinment")]
        // public async  Task<IActionResult> SendmailCreateAppoinment(string email, string date, string time){
        //     var result = await _mailCreateAppoinment.SendEmailAppoinmentAsync(email, "2020/10/01", "1:00 pm");
            
        //     if (result.IsSuccess)
        //         return Ok(result); // 200

        //     return BadRequest(result); // 400
        // }

        // [HttpPost("report")]
        // public async Task<ActionResult<PaginacionModel>> Report(PaginacionCurso.Ejecuta data){
        //     return await Mediator.Send(data);
        // }

    }
}