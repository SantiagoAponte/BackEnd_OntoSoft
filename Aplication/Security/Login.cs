using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplication.Interfaces.Contracts;
using Aplication.ManagerExcepcion;
using Domine;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using persistence;

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
                RuleFor(x => x.Email).NotEmpty().WithMessage("El campo no debe estar vacio");
                RuleFor(x => x.Password).NotEmpty().WithMessage("El campo no debe estar vacio");
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
                var usuario = await _userManager.FindByEmailAsync(request.Email);
                if (usuario == null)
                {
                    throw new ManagerError(HttpStatusCode.Unauthorized);
                }

                var result = await _signInManager.CheckPasswordSignInAsync(usuario, request.Password, false);
                var resultRoles = await _userManager.GetRolesAsync(usuario);
                var listaRoles = new List<string>(resultRoles);


        if(result.Succeeded){
                // var imagenPerfil = await _context.Documento.Where(x => x.ObjetoReferencia == new Guid(usuario.Id)).FirstOrDefaultAsync();

                // if (resultado.Succeeded)
                // {
                //     if (imagenPerfil != null)
                //     {
                //         var imagenCliente = new ImagenGeneral
                //         {
                //             Data = Convert.ToBase64String(imagenPerfil.Contenido),
                //             Extension = imagenPerfil.Extension,
                //             Nombre = imagenPerfil.Nombre
                //         };
                //         return new UserData
                //         {
                //             NombreCompleto = usuario.NombreCompleto,
                //             Token = _jwtGenerador.CrearToken(usuario, listaRoles),
                //             Username = usuario.UserName,
                //             Email = usuario.Email,
                //             ImagenPerfil = imagenCliente
                //         };
                //     }
                //     else
                    // {
                        return new UserData
                        {
                            fullName = usuario.fullName,
                            Token = _jwtGenerator.CreateToken(usuario, listaRoles),
                            Username = usuario.UserName,
                            Email = usuario.Email,
                            Imagen = null
                        };
                    // }
                }
                throw new ManagerError(HttpStatusCode.Unauthorized);
            }
         }   
    }
}   