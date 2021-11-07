using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplication.ManagerExcepcion;
using Domine;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplication.Security
{
    public class deleteUsersRoles
    {
        public class Execute : IRequest{
            public string Email {get;set;}
            public string RolName {get;set;}
        }

        public class ExecuteValidator : AbstractValidator<Execute>{
            public ExecuteValidator(){
                RuleFor(x => x.Email).NotEmpty().WithMessage("El campo de Email no puede ser vacio").NotNull().WithMessage("El campo de Email no puede ser nulo");
                RuleFor(x => x.RolName).NotEmpty().WithMessage("El campo de RolName no deberia ser vacio")
                .NotNull().WithMessage("El campo de RolName no puede ser nulo");
            }
        }
        public class Manager : IRequestHandler<Execute>
        {
            private readonly UserManager<User> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;

            public Manager(UserManager<User> userManager, RoleManager<IdentityRole> roleManager){
                _userManager = userManager;
                _roleManager = roleManager;
            }
            public async Task<Unit> Handle(Execute request, CancellationToken cancellationToken)
            {
                    var role = await _roleManager.FindByNameAsync(request.RolName);
                    if(role == null){
                        throw new ManagerError(HttpStatusCode.NotAcceptable, new {mensaje = "No se encontro el rol"});
                    }

                    var userIden = await _userManager.FindByEmailAsync(request.Email);
                    if(userIden == null){
                        throw new ManagerError(HttpStatusCode.NotAcceptable, new {mensaje = "No se encontro al usuario"});
                    }

                    var result = await _userManager.RemoveFromRoleAsync(userIden, request.RolName);
                    if(result.Succeeded){
                    return Unit.Value;
                    }
                    throw new ManagerError(HttpStatusCode.BadRequest, new {mensaje = "No se pudo eliminar el rol del usuario"});
            }
        }
    }
}