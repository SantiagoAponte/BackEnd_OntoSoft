using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplication.Interfaces.Contracts;
using Aplication.ManagerExcepcion;
using Domine;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using persistence;

namespace Aplication.Security
{
    public class UserPut
    {
        public class Execute : IRequest<UserData>
        {
            // public string fullname { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Username { get; set; }
            public string fullname {get;set;}
            public ImagenPerfil imagenPerfil { get; set; }
        }

        public class ExecuteValidator : AbstractValidator<Execute>
        {
            public ExecuteValidator()
            {
                // RuleFor(x => x.fullname).NotEmpty();
                RuleFor(x => x.Email).NotEmpty().WithMessage("El campo no debe estar vacio");
                RuleFor(x => x.Password).NotEmpty().WithMessage("El campo no debe estar vacio");
                RuleFor(x => x.Username).NotEmpty().WithMessage("El campo no debe estar vacio");
            }
        }

        public class Manager : IRequestHandler<Execute, UserData>
        {
            private readonly    OntoSoftContext _context;
            private readonly UserManager<User> _userManager;
            private readonly IJwtGenerator _jwtGenerator;

            private readonly IPasswordHasher<User> _passwordHasher;

            public Manager(OntoSoftContext context, UserManager<User> userManager, IJwtGenerator jwtGenerator, IPasswordHasher<User> passwordHasher)
            {
                _context = context;
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
                _passwordHasher = passwordHasher;
            }

            public async Task<UserData> Handle(Execute request, CancellationToken cancellationToken)
            {
                var userIden = await _userManager.FindByNameAsync(request.Username);
                if (userIden == null)
                {
                    throw new ManagerError(HttpStatusCode.NotFound, new { mensaje = "No existe un usuario con este username" });
                }

                var result = await _context.Users.Where(x => x.Email == request.Email && x.UserName != request.Username).AnyAsync();
                if (result)
                {
                    throw new ManagerError(HttpStatusCode.InternalServerError, new { mensaje = "Este email pertenece a otro usuario" });
                }

                if (request.imagenPerfil != null)
                {
                    
                        var resultadoImagen = await _context.Galleries.Where(x => x.ObjectReference == new Guid(userIden.Id)).FirstOrDefaultAsync();
                        if (resultadoImagen == null)
                        {
                            var imagen = new Galleries
                            {
                                Contain = System.Convert.FromBase64String(request.imagenPerfil.Data),
                                Name = request.imagenPerfil.Name,
                                Extension = request.imagenPerfil.Extension,
                                ObjectReference = new Guid(userIden.Id),
                                Id = Guid.NewGuid(),
                                dateCreate = DateTime.UtcNow
                            };
                            _context.Galleries.Add(imagen);
                        }
                        else
                        {
                            resultadoImagen.Contain = System.Convert.FromBase64String(request.imagenPerfil.Data);
                            resultadoImagen.Name = request.imagenPerfil.Name;
                            resultadoImagen.Extension = request.imagenPerfil.Extension;
                        }
                    

                }



                userIden.fullName = request.fullname;
                userIden.PasswordHash = _passwordHasher.HashPassword(userIden, request.Password);
                userIden.Email = request.Email;

                var resultadoUpdate = await _userManager.UpdateAsync(userIden);

                var resultadoRoles = await _userManager.GetRolesAsync(userIden);
                var listRoles = new List<string>(resultadoRoles);

                var imagenPerfil = await _context.Galleries.Where(x => x.ObjectReference == new Guid(userIden.Id)).FirstAsync();
                ImagenPerfil imagenProfile = null;
                if (imagenPerfil != null)
                {
                    imagenProfile = new ImagenPerfil
                    {
                        Data = Convert.ToBase64String(imagenPerfil.Contain),
                        Name = imagenPerfil.Name,
                        Extension = imagenPerfil.Extension
                    };
                }


                if (resultadoUpdate.Succeeded)
                {
                    return new UserData
                    {
                        fullName = userIden.fullName,
                        Username = userIden.UserName,
                        Email = userIden.Email,
                        Token = _jwtGenerator.CreateToken(userIden, listRoles),
                        imagenPerfil = imagenProfile
                    };
                }

                throw new System.Exception("No se pudo actualizar el usuario");

            }
        }
    }
}