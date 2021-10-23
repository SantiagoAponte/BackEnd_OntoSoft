using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aplication.Security;
using Domine;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class rolController : myControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
         private readonly UserManager<User> _userManager;
        public rolController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager){
                _roleManager = roleManager;
                _userManager = userManager;
            }

        // [Authorize]
        [HttpPost("addrol")] 
        public async Task<ActionResult<Unit>> CreateRol(addRol.Execute data){
            return await mediator.Send(data);
        }

        [Authorize]
        [HttpDelete("deleterol")]
        public async Task<ActionResult<Unit>> DeleteRol(deleteRol.Execute data){
            return await mediator.Send(data);
        }

        [HttpGet("listroles")]
        public async Task<ActionResult<List<IdentityRole>>> obtainList(){
            return await mediator.Send(new RolList.Execute());
        }

        [HttpPost("adduserrol")]
        public async Task<ActionResult<Unit>>  addUserRol(addUsersRoles.Execute data){
            return await mediator.Send(data);
        }

        [HttpPost("deleteroleuser")]
        public async Task<ActionResult<Unit>> deleteRoleUser(deleteUsersRoles.Execute data){
            return await mediator.Send(data);
        }

        [HttpGet("rolofuser/{Id}")]
        public async Task<ActionResult<List<string>>> obtainRolUser (string id){
            return await mediator.Send(new ObtainRolUser.Execute{Id = id});
        }
         [HttpGet("user/{roleName}")]
        public async Task<ActionResult<List<User>>> obtainUsersRol (string rolename){
            return await mediator.Send(new ObtainUserRol.Execute{roleName = rolename});

        }
    }

}