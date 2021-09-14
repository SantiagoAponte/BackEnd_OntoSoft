using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
        public List<Guid> ListTypeProcess {get;set;}
        public List<Guid> ListTooths {get;set;}
        public List<string> ListUsers {get;set;} 
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

               var odontogram = new Odontogram {
                   Id = odontogramId,
                   date_register = DateTime.UtcNow,
                   observation = request.observation,
               };
            
                _context.Odontogram.Add(odontogram);

                if(request.ListTooths!=null){
                    foreach(var _id in request.ListTooths){
                        var toothsOdontogram = new toothsOdontogram{
                            ToothId = _id,
                            OdontogramId = odontogramId
                        };
                        _context.toothsOdontogram.Add(toothsOdontogram);
                    }
                }

                if(request.ListTypeProcess!=null){
                    foreach(var _id in request.ListTypeProcess){
                        var typeProcessOdontogram = new typeProcessOdontogram{
                            typeProcessId = _id,
                            OdontogramId = odontogramId
                        };
                        _context.typeProcessOdontogram.Add(typeProcessOdontogram);
                    }
                }

                 if(request.ListUsers!=null){
                    foreach(var _id in request.ListUsers){
                        var userOdontogram = new userOdontogram{
                            UserId = _id,
                            OdontogramId = odontogramId
                        };
                        _context.userOdontogram.Add(userOdontogram);
                    }
                }
                         var valor = await _context.SaveChangesAsync();
                        if(valor>0){
                        return Unit.Value;
                        }
                     throw new Exception("No se pudo crear el Odontograma");
            }
        }
    }
}