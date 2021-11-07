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
using Aplication.Interfaces.Contracts;
using Aplication.Security.Users.Dtos;
using persistence;
using System.Text.RegularExpressions;

namespace Aplication.Security
{
    public class Login
    {
      
        public class Execute : IRequest<userLoginDto>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class ExecuteValidator : AbstractValidator<Execute>
        {
        readonly Regex regEx = new Regex("^(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-_]).{8,}$");
            public ExecuteValidator()
            {
                RuleFor(x => x.Email).NotEmpty().WithMessage("El campo de Email no puede ser vacio").NotNull().WithMessage( "El campo de Email no puede ser nulo");
                RuleFor(x => x.Password).NotEmpty().WithMessage("Contraseña invalida, el campo no puede estar vacio").NotNull().WithMessage("La contraseña no puede ser nula")
                .Matches(regEx).WithMessage("la Contraseña no tiene el formato correcto: Mínimo 8 caracteres, al menos 1 letra, 1 número y 1 carácter especial.");
            }
        }

        public class Manager : IRequestHandler<Execute, userLoginDto>
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
            public async Task<userLoginDto> Handle(Execute request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                     throw new ManagerError(HttpStatusCode.NotAcceptable, new {mensaje ="No existe un usuario registrado con este email"});
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
                if(result == null){
                     throw new ManagerError(HttpStatusCode.NotAcceptable, new {mensaje ="Contraseña invalida, por favor verifique de nuevo"});
                }
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
                        return new userLoginDto
                        {
                            // fullName = user.fullName,
                            Message = "Login exitoso!, Bienvenido a OntoSoft",
                            Token = _jwtGenerator.CreateToken(user, listRoles),
                            
                            // Username = user.UserName,
                            // Email = user.Email,
                            // PhoneNumber = user.PhoneNumber,
                            // phoneEmergency = user.phoneEmergency,
                            // contactEmergency = user.contactEmergency,
                            // addresContact = user.addresContact,
                            // centerEmergency = user.centerEmergency,
                            // eps = user.eps,
                            // dateBirth = user.dateBirth,
                            // city = user.city,
                            // address = user.address,
                            // gender = user.gender,
                            // document = user.document,
                            // height = user.height,
                            // weight = user.weight,
                            // rh = user.rh,
                            // bloodType = user.bloodType,
                            // typeDocumentId = user.typeDocumentId,
                            imagenPerfil = imagenProfile
                        };
                    }
                    else
                    {
                        return new userLoginDto
                        {
                            // fullName = user.fullName,
                            Message = "Login exitoso!, Bienvenido a OntoSoft",
                            Token = _jwtGenerator.CreateToken(user, listRoles)
                            // Username = user.UserName,
                            // Email = user.Email,
                            // imagenPerfil = null,
                            // PhoneNumber = user.PhoneNumber,
                            // phoneEmergency = user.phoneEmergency,
                            // contactEmergency = user.contactEmergency,
                            // addresContact = user.addresContact,
                            // centerEmergency = user.centerEmergency,
                            // eps = user.eps,
                            // dateBirth = user.dateBirth,
                            // city = user.city,
                            // address = user.address,
                            // gender = user.gender,
                            // document = user.document,
                            // height = user.height,
                            // weight = user.weight,
                            // rh = user.rh,
                            // bloodType = user.bloodType,
                            // typeDocumentId = user.typeDocumentId
                        };
                    }
                }
                throw new ManagerError(HttpStatusCode.BadRequest, new {mensaje ="Este usuario no se encuentra registrado en nuestro sistema o no esta permitido su ingreso"});
            }
        }

    }
}