using System.Resources;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using Aplication.ManagerExcepcion;
using Domine;
using persistence;
using Aplication.Interfaces.Contracts;

namespace Aplication.Security
{
    public class Login
    {
        public class Execute : IRequest<UserData>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class ExecuteValidator : AbstractValidator<Execute>
        {
            public ExecuteValidator()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class Manager : IRequestHandler<Execute, UserData>
        {

            private readonly UserManager<User> _userManager;
            private readonly SignInManager<User> _signInManager;
            private readonly IJwtGenerator _jwtGenerator;

            private readonly OntoSoftContext _context;
            public Manager(UserManager<User> userManager, SignInManager<User> signInManager, IJwtGenerator jwtGenerator, OntoSoftContext context)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _jwtGenerator = jwtGenerator;
                _context = context;
            }
            public async Task<UserData> Handle(Execute request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    throw new ManagerError(HttpStatusCode.Unauthorized);
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
                var resultRoles = await _userManager.GetRolesAsync(user);
                var listRoles = new List<string>(resultRoles);

                var imagenPerfil = await _context.Galleries.Where(x => x.ObjectReference == new Guid(user.Id)).FirstOrDefaultAsync();

                if (result.Succeeded)
                {
                    if (imagenPerfil != null)
                    {
                        var imagenProfile = new ImagenPerfil
                        {
                            Data = Convert.ToBase64String(imagenPerfil.Contain),
                            Extension = imagenPerfil.Extension,
                            Name = imagenPerfil.Name
                        };
                        return new UserData
                        {
                            fullName = user.fullName,
                            Token = _jwtGenerator.CreateToken(user, listRoles),
                            Username = user.UserName,
                            Email = user.Email,
                            imagenPerfil = imagenProfile
                        };
                    }
                    else
                    {
                        return new UserData
                        {
                            fullName = user.fullName,
                            Token = _jwtGenerator.CreateToken(user, listRoles),
                            Username = user.UserName,
                            Email = user.Email,
                            imagenPerfil = null
                        };
                    }
                }






                throw new ManagerError(HttpStatusCode.Unauthorized);
            }
        }

    }
}