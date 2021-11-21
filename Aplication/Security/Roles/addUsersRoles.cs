using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplication.ManagerExcepcion;
using Domine;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using persistence;

namespace Aplication.Security
{
    public class addUsersRoles
    {
        public class Execute : IRequest {
            public string Id {get;set;}
            public string Email {get;set;}
            public string RolName {get;set;}
        }
        public class ExecuteValidator : AbstractValidator<Execute>{
            public ExecuteValidator(){
                RuleFor(x => x.Email).NotEmpty().WithMessage("El campo de Email no puede estar vacio").NotNull().WithMessage("El campo de Email no puede ser nulo");
                RuleFor(x => x.RolName).NotEmpty().WithMessage("El campo de RolName no puede estar vacio").NotNull().WithMessage("El campo de RolName no puede ser nulo");
                }
            }
            public class Manager : IRequestHandler<Execute>
            {
                private readonly UserManager<User> _userManager;
                private readonly RoleManager<IdentityRole> _roleManager;
                private readonly OntoSoftContext _context;

                public Manager(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,OntoSoftContext context)
                {
                     _userManager = userManager;
                     _roleManager = roleManager;
                     _context = context;
                }

            public async Task<Unit> Handle(Execute request, CancellationToken cancellationToken)
            {
                
                var role = await _roleManager.FindByNameAsync(request.RolName);
                     if(role == null){
                          throw new ManagerError(HttpStatusCode.NotAcceptable, new {mensaje = "El rol no existe"});
                     }
                     var userIden = await _userManager.FindByEmailAsync(request.Email);
                     if(userIden == null){
                         throw new ManagerError(HttpStatusCode.NotAcceptable, new {mensaje = "El usuario no existe"});
                     }
                     var usersBD = _context.UserRoles.Where(x => x.UserId == request.Id);
                        foreach(var userDelete in usersBD){
                            _context.UserRoles.Remove(userDelete);
                        }
                   var result2 = await _context.SaveChangesAsync();     
                   var result =  await _userManager.AddToRoleAsync(userIden, request.RolName);
                   if(result.Succeeded){
                       return Unit.Value;
                   }
                   throw new ManagerError(HttpStatusCode.BadRequest, new {mensaje = "No se pudo agregar el Rol al usuario"});
            }
        }
        }
    }