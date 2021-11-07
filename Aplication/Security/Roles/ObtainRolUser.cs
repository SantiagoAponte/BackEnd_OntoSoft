using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplication.ManagerExcepcion;
using Domine;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplication.Security
{
    public class ObtainRolUser
    {
        public class Execute : IRequest<List<string>>{
            public string Id {get;set;}
        }

        public class Manager : IRequestHandler<Execute, List<string>>
        {
            private readonly UserManager<User> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;

            public Manager(UserManager<User> userManager, RoleManager<IdentityRole> roleManager){
                _roleManager = roleManager;
                _userManager = userManager;
            }
            public async Task<List<string>> Handle(Execute request, CancellationToken cancellationToken)
            {
                var userIden = await _userManager.FindByIdAsync(request.Id);
                if(userIden == null){
                    throw new ManagerError(HttpStatusCode.NotAcceptable, new {mensaje = "No existe el usuario"});
                }

                var result =await _userManager.GetRolesAsync(userIden);
                return new List<string>(result);
            }
        }

    }
    }