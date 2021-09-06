using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class myControllerBase : ControllerBase 
    {
        private IMediator _mediator;

        protected IMediator mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>());
    }
}