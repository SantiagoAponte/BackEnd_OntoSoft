using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplication.ManagerExcepcion;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplication.Security
{

    public class deleteRol
    {
        public class Execute : IRequest{
            public string Name {get;set;}
        }
        public class ExecuteValidation : AbstractValidator<Execute>{
            public  ExecuteValidation(){
                RuleFor(x => x.Name).NotEmpty().WithMessage(x => "El campo de Name (RolName) no puede estar vacio");
            }
        }

        public class Manager : IRequestHandler<Execute>
        {
            private readonly RoleManager<IdentityRole> _roleManager;
            public Manager(RoleManager<IdentityRole> roleManager){
                _roleManager = roleManager;
            }
            public async Task<Unit> Handle(Execute request, CancellationToken cancellationToken)
            {
                var role = await _roleManager.FindByNameAsync(request.Name);
                if(role == null){
                    throw new Exception("No existe el rol");
                }
                var result = await _roleManager.DeleteAsync(role);
                if(result.Succeeded)
                throw new ManagerError(HttpStatusCode.OK, new {mensaje = "¡Se elimino el rol con exito!"});
                return Unit.Value;
                
                throw new Exception("No se pudo eliminar el rol");
            }
        }
    }
}