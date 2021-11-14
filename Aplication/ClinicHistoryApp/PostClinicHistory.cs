using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplication.ManagerExcepcion;
using Domine;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using persistence;

namespace Aplication.ClinicHistoryApp
{
    public class PostClinicHistory
    {
        public class Execute : IRequest {
        public Guid? Id {get;set;}
        public string phoneCompanion {get;set;}
        public string nameCompanion {get;set;}
        public DateTime dateRegister {get;set;}
        public string UserId{get;set;}
        public bool backgroundMedical {get;set;}
        public bool backgroundOral {get;set;}
        public List<Guid> ListBackgroundMedical {get;set;}
        public List<Guid> ListBackgroundOral {get;set;}

        }

        public class ExecuteValidator : AbstractValidator<Execute>{
            public ExecuteValidator(){
                RuleFor( x => x.backgroundMedical).NotNull().WithMessage("El campo backgroundMedical no puede ser nulo");
                RuleFor( x => x.backgroundOral).NotNull().WithMessage("El campo backgroundOral no puede ser nulo");
                RuleFor(x => x.UserId).NotNull().WithMessage("El campo UserId no puede estar vacio").NotEmpty().WithMessage("El campo UserId no puede ser nulo");
                RuleFor(x => x.dateRegister).NotEmpty().WithMessage("El campo dateRegister no puede estar vacio").WithMessage("El campo dateRegister no puede ser nulo");
                // RuleFor(x => x.ListBackgroundMedical).NotEmpty().WithMessage("El campo ListBackgroundMedical no puede estar vacio o nulo");
                // RuleFor(x => x.ListBackgroundOral).NotEmpty().WithMessage("El campo ListBackgroundOral no puede estar vacio o nulo" + "debe tener al menos un antecedente");
            }
        }

        public class Manager : IRequestHandler<Execute>
        {
            private readonly UserManager<User> _userManager;
            private readonly OntoSoftContext _context;
            public Manager(OntoSoftContext context,UserManager<User> userManager){
                _context = context;
                 _userManager = userManager;
            }

            public async Task<Unit> Handle(Execute request, CancellationToken cancellationToken)
            {
               
               Guid clinicHistoryId = Guid.NewGuid();
               if(request.Id != null){
                 clinicHistoryId = request.Id ?? Guid.NewGuid();
               }
              var user = await _userManager.FindByIdAsync(request.UserId);
                if (user == null)
                {
                     throw new ManagerError(HttpStatusCode.NotAcceptable, new {mensaje ="Primero debe seleccionar un usuario para realizarle un registro de información"});
                }
               
               var clinicHistory = new ClinicHistory {
                   Id = clinicHistoryId,
                   phoneCompanion = request.phoneCompanion,
                   nameCompanion = request.nameCompanion,
                   dateRegister = request.dateRegister,
                   backgroundMedical = request.backgroundMedical,
                   backgroundOral = request.backgroundOral,
                   UserId = request.UserId,
                   
               };
                _context.clinicHistories.Add(clinicHistory);
                        
                 if(request.ListBackgroundMedical!=null){
                    foreach(var _id in request.ListBackgroundMedical){
                        var backgroundMedicalClinicHistory = new backgroundMedicalClinicHistory{
                            BackgroundMedicalsId = _id,
                            clinicHistoryId = clinicHistoryId
                        };
                        _context.backgroundMedicalClinicHistories.Add(backgroundMedicalClinicHistory);
                    }
                }

                
                 if(request.ListBackgroundOral!=null){
                    foreach(var _id in request.ListBackgroundOral){
                        var backgroundOralClinicHistory = new backgroundOralClinicHistory{
                            BackgroundOralsId = _id,
                            clinicHistoryId =  clinicHistoryId
                        };
                        _context.backgroundOralClinicHistories.Add(backgroundOralClinicHistory);
                    }

                }

                 var valor = await _context.SaveChangesAsync();
                        if(valor>0){
                        return Unit.Value;
                        }
                        throw new ManagerError(HttpStatusCode.BadRequest, new {mensaje ="No se pudo crear la historia clinica"}); 
            }
        }
    }
}