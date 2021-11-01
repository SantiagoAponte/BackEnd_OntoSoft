using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domine;
using FluentValidation;
using MediatR;
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
                RuleFor( x => x.backgroundMedical).NotEmpty().WithMessage(x => "El campo backgroundMedical no puede estar vacio");
                RuleFor( x => x.backgroundOral).NotEmpty().WithMessage(x => "El campo backgroundOral no puede estar vacio");
                RuleFor( x => x.UserId).NotEmpty().WithMessage(x => "El campo UserId no puede estar vacio");
                RuleFor( x => x.dateRegister).NotEmpty().WithMessage(x => "El campo dateRegister no puede estar vacio");
                RuleFor( x => x.ListBackgroundMedical).NotEmpty().WithMessage(x => "El campo ListBackgroundMedical no puede estar vacio");
                RuleFor( x => x.ListBackgroundOral).NotEmpty().WithMessage(x => "El campo ListBackgroundOral no puede estar vacio");
            }
        }

        public class Manager : IRequestHandler<Execute>
        {
            private readonly OntoSoftContext _context;
            public Manager(OntoSoftContext context){
                _context = context;
            }

            public async Task<Unit> Handle(Execute request, CancellationToken cancellationToken)
            {
               
               Guid clinicHistoryId = Guid.NewGuid();
               if(request.Id != null){
                 clinicHistoryId = request.Id ?? Guid.NewGuid();
               }
               
               var clinicHistory = new ClinicHistory {
                   Id = clinicHistoryId,
                   phoneCompanion = request.phoneCompanion,
                   nameCompanion = request.nameCompanion,
                   dateRegister = DateTime.UtcNow,
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
                     throw new Exception("No se pudo crear la historia clinica");
            }
        }
    }
}