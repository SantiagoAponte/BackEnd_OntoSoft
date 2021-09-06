using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplication.Interfaces.Contracts;
using Aplication.ManagerExcepcion;
using Domine;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using persistence;

namespace Aplication.Security
{
    public class UserPut
    {
        public class Ejecuta : IRequest<UserData>
        {
            // public string fullname { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Username { get; set; }
            public ImagenPerfil ImagenPerfil { get; set; }
        }

        public class EjecutaValidador : AbstractValidator<Ejecuta>
        {
            public EjecutaValidador()
            {
                // RuleFor(x => x.fullname).NotEmpty();
                RuleFor(x => x.Email).NotEmpty().WithMessage("El campo no debe estar vacio");
                RuleFor(x => x.Password).NotEmpty().WithMessage("El campo no debe estar vacio");
                RuleFor(x => x.Username).NotEmpty().WithMessage("El campo no debe estar vacio");
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, UserData>
        {
            private readonly    OntoSoftContext _context;
            private readonly UserManager<User> _userManager;
            private readonly IJwtGenerator _jwtGenerator;

            private readonly IPasswordHasher<User> _passwordHasher;

            public Manejador(OntoSoftContext context, UserManager<User> userManager, IJwtGenerator jwtGenerator, IPasswordHasher<User> passwordHasher)
            {
                _context = context;
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
                _passwordHasher = passwordHasher;
            }

            public async Task<UserData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuarioIden = await _userManager.FindByNameAsync(request.Username);
                if (usuarioIden == null)
                {
                    throw new ManagerError(HttpStatusCode.NotFound, new { mensaje = "No existe un usuario con este username" });
                }

                var resultado = await _context.Users.Where(x => x.Email == request.Email && x.UserName != request.Username).AnyAsync();
                if (resultado)
                {
                    throw new ManagerError(HttpStatusCode.InternalServerError, new { mensaje = "Este email pertenece a otro usuario" });
                }

                // if (request.ImagenPerfil != null)
                // {
                    
                //         var resultadoImagen = await _context.Galleries.Where(x => x.ObjectReference == new Guid(usuarioIden.Id)).FirstOrDefaultAsync();
                //         if (resultadoImagen == null)
                //         {
                //             var imagen = new Galleries
                //             {
                //                 Contain = System.Convert.FromBase64String(request.ImagenPerfil.Data),
                //                 Name = request.ImagenPerfil.Name,
                //                 Extension = request.ImagenPerfil.Extension,
                //                 ObjectReference = new Guid(usuarioIden.Id),
                //                 Id = Guid.NewGuid(),
                //                 dateCreate = DateTime.UtcNow
                //             };
                //             _context.Galleries.Add(imagen);
                //         }
                //         else
                //         {
                //             resultadoImagen.Contenido = System.Convert.FromBase64String(request.ImagenPerfil.Data);
                //             resultadoImagen.Nombre = request.ImagenPerfil.Name;
                //             resultadoImagen.Extension = request.ImagenPerfil.Extension;
                //         }
                    

                // }



                // usuarioIden.fullName = request.fullname;
                usuarioIden.PasswordHash = _passwordHasher.HashPassword(usuarioIden, request.Password);
                usuarioIden.Email = request.Email;

                var resultadoUpdate = await _userManager.UpdateAsync(usuarioIden);

                var resultadoRoles = await _userManager.GetRolesAsync(usuarioIden);
                var listRoles = new List<string>(resultadoRoles);

                // var imagenPerfil = await _context.Galleries.Where(x => x.ObjetoReferencia == new Guid(usuarioIden.Id)).FirstAsync();
                // ImagenPerfil imagenGeneral = null;
                // if (imagenPerfil != null)
                // {
                //     imagenGeneral = new ImagenPerfil
                //     {
                //         Data = Convert.ToBase64String(imagenPerfil.Contenido),
                //         Name = imagenPerfil.Name,
                //         Extension = imagenPerfil.Extension
                //     };
                // }


                if (resultadoUpdate.Succeeded)
                {
                    return new UserData
                    {
                        // fullname = usuarioIden.fullName,
                        Username = usuarioIden.UserName,
                        Email = usuarioIden.Email,
                        Token = _jwtGenerator.CreateToken(usuarioIden, listRoles),
                        // Imagen = imagenPerfil
                    };
                }

                throw new System.Exception("No se pudo actualizar el usuario");

            }
        }
    }
}