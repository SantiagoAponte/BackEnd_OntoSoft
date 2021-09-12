using System;
using System.Threading;
using System.Threading.Tasks;
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
                RuleFor(x => x.Name).NotEmpty().WithMessage("El campo no debe estar vacio");
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
                if(result.Succeeded){
                    return Unit.Value;
                }
                throw new Exception("No se pudo eliminar el rol");
            }
        }
    }
}