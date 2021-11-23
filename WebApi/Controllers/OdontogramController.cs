using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aplication.AppoinmentsApp;
using Aplication.OdontoApp;
using Aplication.OdontoApp.Dtos;
using Domine;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class odontogramController : myControllerBase
    {
        [HttpGet("tooths")]
        public async Task<ActionResult<List<tooth>>> GetTooths(){

            return await mediator.Send(new GetTooth.ListTooths());
        }

        [HttpGet("typeprocess")]
        public async Task<ActionResult<List<typeProcess>>> GetTypeProcess(){

            return await mediator.Send(new GetTypeProcess.ListTypeProcess());
        }

        [HttpGet("facetooths")]
        public async Task<ActionResult<List<FaceTooth>>> GetfaceTooths(){

            return await mediator.Send(new GetFaceTooth.ListfaceTooths());
        }

        [HttpPost("addodontogram")]
        public async Task<ActionResult<Unit>> CreateOdontogram(PostOdontogram.Execute data){
            return await mediator.Send(data);
        }

        [HttpGet]
        public async Task<ActionResult<List<odontogramDto>>> GetResultAsync(){

            return await mediator.Send(new GetOdontogram.ListTooth());
        }
        [HttpGet("users/{id}")]
        public async Task<ActionResult<odontogramDto>> ObtainOneOdontogramWithUser(string id){
            return await mediator.Send(new GetOdontogramWithUser.OneOdontogramUser{Id = id});
        }

        [HttpPut("edit/{id}")]
        public async Task<ActionResult<Unit>> putOdontogram(Guid id, putOdontogram.Execute data){
            data.Id = id;
            return await mediator.Send(data);
        }

        // [HttpDelete("delete")]
        // public async Task<ActionResult<Unit>> deleteOdontogram(deleteOdontogram.Execute data){
        //     return await mediator.Send(data);
        // }
         [HttpDelete("delete/{id}")]
        public async Task<ActionResult<Unit>> deleteAppoinment(Guid id){
            return await mediator.Send(new deleteOdontogram.Execute{Id = id});
        }
    }
}