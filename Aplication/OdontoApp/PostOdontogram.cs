using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domine;
using FluentValidation;
using MediatR;
using Persistence;

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
        public Guid clinicHistoryId{get;set;}
        }

        public class ExecuteValidator : AbstractValidator<Execute>{
            public ExecuteValidator(){
                RuleFor( x => x.observation).NotEmpty().WithMessage("El campo no debe estar vacio");
                RuleFor( x => x.date_register).NotEmpty().WithMessage("El campo no debe estar vacio");
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
                   clinicHistoryId = request.clinicHistoryId,
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
