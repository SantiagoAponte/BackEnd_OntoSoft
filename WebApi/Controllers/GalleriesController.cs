using System;
using System.Threading.Tasks;
using Aplication.GalleriesArchive;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    //https://localhost:5000/api/Galleries/upload
    public class galleriesController : myControllerBase
    {
        [HttpPost("upload")]
        [Authorize (Roles = "SuperAdmin, Paciente, Recepcionista, Doctor")]
        public async Task<ActionResult<Unit>> GuardarArchivo(uploadGalleries.Execute data){
            return await mediator.Send(data);
        }
         //https://localhost:5000/api/Galleries/{id}
        [HttpGet("{id}")]
        [Authorize (Roles = "SuperAdmin, Paciente, Recepcionista, Doctor")]
        public async Task<ActionResult<galleriesDto>> ObtenerDocumento(Guid id){
            return await mediator.Send(new getGalleries.Execute { Id = id });
        }

    }
}