using Aplication.Interfaces.Contracts;
using Aplication.ManagerExcepcion;
using Aplication.Security;
using Domine;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplication.Interfaces.Contracts


{
    public class ResetPassword
    {
       public class Execute : IRequest<UserManagerResponse>
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
    public class Manager : IRequestHandler<Execute, UserManagerResponse>
    {
        public string Password { get; set; }
        private readonly OntoSoftContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private UserManager<User> _userManager;
        private IConfiguration _configuration;
        private IMailService _mailService; 
        private readonly IJwtGenerator _jwtGenerator;
        public Manager(UserManager<User> userManager, IConfiguration configuration, IMailService mailService, OntoSoftContext context, IPasswordHasher<User> passwordHasher, IJwtGenerator jwtGenerator) 
        {
            _userManager = userManager;
            _configuration = configuration;
            _mailService = mailService;
            _passwordHasher = passwordHasher;
            _context = context;
            _jwtGenerator = jwtGenerator;

        }
        public async Task<UserManagerResponse> Handle(Execute request, CancellationToken cancellationToken)
            {
                var usuarioIden = await _userManager.FindByEmailAsync(request.Email);
            if (usuarioIden == null)
                {
                    throw new ManagerError(HttpStatusCode.NotFound, new { mensaje = "No existe un usuario con este Email" });
                }

                usuarioIden.Email = request.Email;
                usuarioIden.PasswordHash = _passwordHasher.HashPassword(usuarioIden, request.Password);
                
                var resultadoUpdate = await _userManager.UpdateAsync(usuarioIden);
                var resultadoRoles = await _userManager.GetRolesAsync(usuarioIden);
                var listRoles = new List<string>(resultadoRoles);

                if (resultadoUpdate.Succeeded)
                {
                    return new UserManagerResponse
                    {
                        Message = "Contraseña reestablecida con exito!",
                        Token = _jwtGenerator.CreateToken(usuarioIden, listRoles),
                      
                    };
                }

                throw new System.Exception("No se pudo actualizar la contraseña");
            }
        }
    }
}