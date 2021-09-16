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
    public class OdontogramController : myControllerBase
    {
        [HttpGet("tooths")]
        public async Task<ActionResult<List<tooth>>> Get(){

            return await mediator.Send(new GetTooth.ListTooths());
        }

        [HttpGet("typeProcess")]
        public async Task<ActionResult<List<typeProcess>>> GetResult(){

            return await mediator.Send(new GetTypeProcess.ListTypeProcess());
        }

        [HttpPost("addOdontogram")]
        public async Task<ActionResult<Unit>> CreateOdontogram(PostOdontogram.Execute data){
            return await mediator.Send(data);
        }

        [HttpGet]
        public async Task<ActionResult<List<odontogramDto>>> GetResultAsync(){

            return await mediator.Send(new GetOdontogram.ListOdontongram());
        }
    }
}