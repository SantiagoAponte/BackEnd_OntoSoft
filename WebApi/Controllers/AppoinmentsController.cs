using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aplication.AppoinmentsApp;
using Aplication.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class AppoinmentsController : myControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<AppoinmentsDto>>> Get(){

            return await mediator.Send(new GetAppoinment.ListAppoinments());
        }

        
        //http://localhost:5000/api/Appoinments/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AppoinmentsDto>> ObtainOneAppoinment(Guid id){
            return await mediator.Send(new GetOneAppoinment.OneAppoinment{Id = id});
        }
        
        //Api que trae todas las citas por el ID de un usuario.
        [HttpGet("users/{id}")]
        public async Task<ActionResult<UserDto>> ObtainOneAppoinmentWithUser(string id){
            return await mediator.Send(new GetAppoinmentWithUser.OneAppoinmentUser{Id = id});
        }

        [HttpPost("add")]
        public async Task<ActionResult<Unit>> CreateAppoinment(PostAppoinment.Execute data){
            return await mediator.Send(data);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> putAppoinment(Guid id, putAppoinments.Execute data){
            data.Id = id;
            return await mediator.Send(data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> deleteAppoinment(Guid id){
            return await mediator.Send(new deleteAppoinments.Execute{Id = id});
        }

        // [HttpPost("report")]
        // public async Task<ActionResult<PaginacionModel>> Report(PaginacionCurso.Ejecuta data){
        //     return await Mediator.Send(data);
        // }

    }
}