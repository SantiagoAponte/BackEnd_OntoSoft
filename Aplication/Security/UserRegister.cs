using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domine;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using persistence;

namespace Aplication.Security
{
    public class UserRegister
    {
        public class Execute : IRequest<UserData> {
        public string Email {get;set;}
        public string Username {get;set;}
        public string Password {get;set;}
        // public string Image {get;set;}
        }
    public class ExecuteValidator : AbstractValidator<Execute>{
    public ExecuteValidator(){
        RuleFor(x => x.Email).NotEmpty().WithMessage("El campo no debe estar vacio");
        RuleFor(x => x.Username).NotEmpty().WithMessage("El campo no debe estar vacio");
        RuleFor(x => x.Password).NotEmpty().WithMessage("El campo no debe estar vacio");
                // RuleFor(x => x.Password).Null();
                // RuleFor(x => x.Image).Null();
            }
    }
        public class Manager : IRequestHandler<Execute, UserData>
        {
            private readonly OntoSoftContext _context;
            private readonly UserManager<User> _userManager;
            public Manager (OntoSoftContext context, UserManager<User> userManager){
                _context = context;
                _userManager = userManager;
            }

            public async Task<UserData> Handle(Execute request, CancellationToken cancellationToken)
            {
            var exist = await _context.Users.Where( x => x.Email == request.Email).AnyAsync();
               if(exist){
                  throw new Exception("Existe ya un usuario registrado con este email");
               }
               
               var existUserName = await _context.Users.Where( x => x.UserName == request.Username).AnyAsync();
               if(existUserName){
                   throw new Exception("Existe ya un usuario con este username");
               }
                var user = new User {
                    // Id = request.Id,
                    Email = request.Email,
                    UserName = request.Username,
                    PasswordHash = request.Password,
                    // Image = request.Image
                };

               var result = await _userManager.CreateAsync(user, request.Password);
                if(result.Succeeded){
                    return new UserData {
                    Username = user.UserName,
                    Email = user.Email,
                    PasswordHash = request.Password
                    };
                }
                throw new Exception("No se pudo agregar al nuevo usuario, verifique que su contraseña tenga al menos una mayuscula, numeros y un caracter especial");
            }
        }
    }
}

