using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aplication.AppoinmentsApp;
using Aplication.OdontoApp;
using Aplication.OdontoApp.Dtos;
using Domine;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class odontogramController : myControllerBase
    {
        [HttpGet("tooths")]
        [Authorize (Roles = "SuperAdmin, Doctor")]
        public async Task<ActionResult<List<tooth>>> GetTooths(){

            return await mediator.Send(new GetTooth.ListTooths());
        }

        [HttpGet("typeprocess")]
        [Authorize (Roles = "SuperAdmin, Doctor")]
        public async Task<ActionResult<List<typeProcess>>> GetTypeProcess(){

            return await mediator.Send(new GetTypeProcess.ListTypeProcess());
        }

        [HttpGet("facetooths")]
        [Authorize (Roles = "SuperAdmin, Doctor")]
        public async Task<ActionResult<List<FaceTooth>>> GetfaceTooths(){

            return await mediator.Send(new GetFaceTooth.ListfaceTooths());
        }

        [HttpPost("addodontogram")]
        [Authorize (Roles = "SuperAdmin, Doctor")]
        public async Task<ActionResult<Unit>> CreateOdontogram(PostOdontogram.Execute data){
            return await mediator.Send(data);
        }

        [HttpGet]
        [Authorize (Roles = "SuperAdmin, Doctor")]
        public async Task<ActionResult<List<odontogramDto>>> GetResultAsync(){

            return await mediator.Send(new GetOdontogram.ListTooth());
        }
        [HttpGet("users/{id}")]
        [Authorize (Roles = "SuperAdmin, Doctor")]
        public async Task<ActionResult<List<odontogramDto>>> ObtainOneOdontogramWithUser(string id){
            return await mediator.Send(new GetOdontogramWithUser.OneOdontogramUser{Id = id});
        }

        [HttpPut("edit/{id}")]
        [Authorize (Roles = "SuperAdmin, Doctor")]
        public async Task<ActionResult<Unit>> putOdontogram(Guid id, putOdontogram.Execute data){
            data.Id = id;
            return await mediator.Send(data);
        }

        // [HttpDelete("delete")]
        // public async Task<ActionResult<Unit>> deleteOdontogram(deleteOdontogram.Execute data){
        //     return await mediator.Send(data);
        // }
         [HttpDelete("delete/{id}")]
         [Authorize (Roles = "SuperAdmin, Doctor")]
        public async Task<ActionResult<Unit>> deleteOdontogram(Guid id){
            return await mediator.Send(new deleteOdontogram.Execute{Id = id});
        }
    }
}