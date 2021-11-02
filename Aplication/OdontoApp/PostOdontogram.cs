using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplication.ManagerExcepcion;
using Domine;
using FluentValidation;
using MediatR;
using persistence;

namespace Aplication.AppoinmentsApp
{
    public class PostOdontogram
    {
        public class Execute : IRequest {
        public Guid? Id {get;set;}
        public DateTime date_register {get;set;}
        public string observation {get;set;}
        public List<typeProcessTooth> typeProcessTooth {get;set;}
        public string UserId {get;set;} 
        // public Guid clinicHistoryId{get;set;}
        }

        public class ExecuteValidator : AbstractValidator<Execute>{
            public ExecuteValidator(){
                RuleFor( x => x.observation).NotEmpty().WithMessage(x => "La observación no puede ser vacia.");
                RuleFor( x => x.date_register).NotEmpty().WithMessage(x => "la fecha de registro no puede ser nula");
                RuleFor( x => x.UserId).NotEmpty().WithMessage(x => "Debe de seleccionar primero un usuario para crear el odontograma.");
                RuleFor( x => x.typeProcessTooth).NotEmpty().WithMessage(x => "Asegurese de haber seleccionado al menos un procedimiento en algun diente");
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
               
               Guid odontogramId = Guid.NewGuid();
               if(request.Id != null){
                 odontogramId = request.Id ?? Guid.NewGuid();
               }
               Guid toothTypeProcessId = Guid.NewGuid();
               if(request.Id != null){
                 toothTypeProcessId = request.Id ?? Guid.NewGuid();
               }

               var odontogram = new Odontogram {
                   Id = odontogramId,
                   date_register = DateTime.UtcNow,
                   observation = request.observation,
                   UserId = request.UserId,
                //    clinicHistoryId = request.clinicHistoryId,
                   toothTypeProcessLink = request.typeProcessTooth,
                                 
                    };
                    _context.Odontogram.Add(odontogram);     
                     
                 var valor = await _context.SaveChangesAsync();
                        if(valor>0){
                        return Unit.Value;
                        }
                     throw new Exception("No se pudo crear el Odontograma");
            }
        }
    }
}
