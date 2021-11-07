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
    public class ObtainUserRol
    {
         public class Execute : IRequest<List<User>>{
            public string roleName {get;set;}
        }

        public class Manager : IRequestHandler<Execute, List<User>>
        {
            private readonly UserManager<User> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;

            public Manager(UserManager<User> userManager, RoleManager<IdentityRole> roleManager){
                _roleManager = roleManager;
                _userManager = userManager;
            }
            public async Task<List<User>> Handle(Execute request, CancellationToken cancellationToken)
            {
                var roleIden = await _roleManager.FindByNameAsync(request.roleName);
                if(roleIden == null){
                    throw new ManagerError(HttpStatusCode.NotAcceptable, new {mensaje = "No existe el rol"});
                }
                var Name = request.roleName;

                var result = await _userManager.GetUsersInRoleAsync(Name);
                return new List<User>(result);
            }

        
        }
    }
}