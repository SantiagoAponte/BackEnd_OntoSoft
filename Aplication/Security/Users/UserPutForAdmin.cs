using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplication.ManagerExcepcion;
using Domine;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using persistence;

namespace Aplication.Security.Users
{
    public class UserPutForAdmin
    {
        public class Execute : IRequest
        {
            public string Id {get;set;}
            public string Email { get; set; }
            public string Username { get; set; }
            public string fullname {get;set;}
             public string PhoneNumber {get;set;}
            public string phoneEmergency {get;set;}
            public string contactEmergency {get;set;}
            public string addresContact {get;set;}
            public string centerEmergency {get;set;}
            public string eps {get;set;}
            public DateTime dateBirth {get;set;}
            public string city {get;set;}
            public string address {get;set;}
            public string gender {get;set;}
            public string document {get;set;}
            public string height {get;set;}
            public string weight {get;set;}
            public char rh {get;set;}
            public string bloodType {get;set;}
            public List<string> typeDocumentId {get;set;}
            public ImagenPerfil imagenPerfil { get; set; }
        }
        public class ExecuteValidator : AbstractValidator<Execute>
        {
            public ExecuteValidator()
            {
                // RuleFor(x => x.fullname).NotEmpty();
                RuleFor(x => x.Email).NotEmpty().NotNull().WithMessage("El campo de Email no puede estar vacio o nulo");
                // RuleFor(x => x.PasswordHash).NotEmpty().NotNull().WithMessage("el campo de Password no puede estar vacio o nulo");
                RuleFor(x => x.Username).NotEmpty().WithMessage("El campo Username no puede estar vacio o nulo");
                RuleFor(x => x.fullname).NotEmpty().NotNull().WithMessage("El campo fullname no puede estar vacio o nulo");
                RuleFor(x => x.PhoneNumber).NotEmpty().NotNull().WithMessage("El campo PhoneNumber no puede estar vacio o nulo");
                RuleFor(x => x.phoneEmergency).NotEmpty().NotNull().WithMessage("El campo phoneEmergency no puede estar vacio o nulo");
                RuleFor(x => x.contactEmergency).NotEmpty().NotNull().WithMessage("El campo contactEmergency no puede estar vacio o nulo");
                RuleFor(x => x.addresContact).NotEmpty().NotNull().WithMessage("El campo addresContact no puede estar vacio o nulo");
                RuleFor(x => x.centerEmergency).NotEmpty().NotNull().WithMessage("El campo centerEmergency no puede estar vacio o nulo");
                RuleFor(x => x.eps).NotEmpty().NotNull().WithMessage("El campo eps no puede estar vacio o nulo");
                RuleFor(x => x.dateBirth).NotEmpty().NotNull().WithMessage("El campo dateBirth no puede estar vacio o nulo");
                RuleFor(x => x.city).NotEmpty().NotNull().WithMessage("El campo city no puede estar vacio o nulo");
                RuleFor(x => x.address).NotEmpty().NotNull().WithMessage("El campo address no puede estar vacio o nulo");
                RuleFor(x => x.gender).NotEmpty().NotNull().WithMessage("El campo gender no puede estar vacio o nulo");
                RuleFor(x => x.document).NotEmpty().NotNull().WithMessage("El campo document no puede estar vacio o nulo");
                RuleFor(x => x.height).NotEmpty().NotNull().WithMessage("El campo height no puede estar vacio o nulo");
                RuleFor(x => x.weight).NotEmpty().NotNull().WithMessage("El campo weight no puede estar vacio o nulo");
                RuleFor(x => x.rh).NotEmpty().NotNull().WithMessage("El campo rh no puede estar vacio o nulo");
                RuleFor(x => x.bloodType).NotEmpty().NotNull().WithMessage("El campo bloodType no puede estar vacio o nulo");
                RuleFor(x => x.typeDocumentId).NotEmpty().NotNull().WithMessage("El campo typeDocumentId no puede estar vacio o nulo");
                

            }
        }
        public class Manager : IRequestHandler<Execute>
        {
            private readonly    OntoSoftContext _context;
            private readonly UserManager<User> _userManager;
     

            public Manager(OntoSoftContext context, UserManager<User> userManager)
            {
                _context = context;
                _userManager = userManager;
            }

            public async Task<Unit> Handle(Execute request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FindAsync(request.Id);
                  if(
                      user==null){
                    throw new ManagerError(HttpStatusCode.NotAcceptable, new {mensaje = "No se encontro al usuario"});
                }
                  var userIden = await _userManager.FindByNameAsync(request.Username);
                if (userIden == null)
                {
                    throw new ManagerError(HttpStatusCode.NotAcceptable, new { mensaje = "No existe un usuario con este username" });
                }

                var result = await _context.Users.Where(x => x.Email == request.Email && x.UserName != request.Username).AnyAsync();
                if (result)
                {
                    throw new ManagerError(HttpStatusCode.NotAcceptable, new { mensaje = "Este email o username pertenece a otro usuario" });
                }

                if (request.imagenPerfil != null)
                {
                    
                        var resultadoImagen = await _context.Galleries.Where(x => x.ObjectReference == new Guid(userIden.Id)).FirstOrDefaultAsync();
                        if (resultadoImagen == null)
                        {
                            var imagen = new Galleries
                            {
                                Contain = Convert.FromBase64String(request.imagenPerfil.Data),
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
                            resultadoImagen.Contain = Convert.FromBase64String(request.imagenPerfil.Data);
                            resultadoImagen.Name = request.imagenPerfil.Name;
                            resultadoImagen.Extension = request.imagenPerfil.Extension;
                        }
                    

                }

                userIden.fullName = request.fullname;
                userIden.Email = request.Email;
                userIden.PhoneNumber = request.PhoneNumber;
                userIden.phoneEmergency = request.phoneEmergency;
                userIden.contactEmergency = request.contactEmergency;
                userIden.addresContact = request.addresContact;
                userIden.centerEmergency = request.centerEmergency;
                userIden.eps = request.eps;
                userIden.dateBirth = request.dateBirth.AddHours(-5);
                userIden.city = request.city;
                userIden.address = request.address;
                userIden.gender = request.gender;
                userIden.document = request.document;
                userIden.height = request.height;
                userIden.weight = request.weight;
                userIden.rh = request.rh;
                userIden.bloodType = request.bloodType;

                userIden.typeDocumentId = "default";
               foreach(var _id in request.typeDocumentId){
                 userIden.typeDocumentId = _id;
               } 
                var resultadoUpdate = await _userManager.UpdateAsync(userIden);

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
        
                var resultFull = await _context.SaveChangesAsync();

                if (resultFull > 0 && resultadoUpdate == IdentityResult.Success){
                throw new ManagerError(HttpStatusCode.BadRequest, new {mensaje = "Error, no se pudo actualizar la información del usuario"});   
                // return Unit.Value;
                }
                throw new ManagerError(HttpStatusCode.OK, new {mensaje = "Se actualizo la información del usuario con exito"});
            }
        }
    }
}