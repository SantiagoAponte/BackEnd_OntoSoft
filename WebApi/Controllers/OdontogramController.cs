using System.Collections.Generic;
using System.Threading.Tasks;
using Aplication.OdontoApp;
using Domine;
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
    }
}