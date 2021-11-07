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
        public Guid clinicHistoryId{get;set;}
        }

        public class ExecuteValidator : AbstractValidator<Execute>{
            public ExecuteValidator(){
                RuleFor( x => x.observation).NotEmpty().WithMessage("El campo de observación no puede ser vacio").NotNull().WithMessage("El campo de observación no puede ser nulo.");
                RuleFor( x => x.date_register).NotEmpty().WithMessage("El campo de date_register no puede ser vacio").NotNull().WithMessage("El campo de fecha de registro no puede ser vacio o nulo");
                RuleFor( x => x.typeProcessTooth).NotEmpty().WithMessage("El campo de typeProcessTooth no puede ser vacio, pinte al menos un diente").NotNull().WithMessage("El campo de typeProcessTooth no puede ser nulo");
                RuleFor( x => x.UserId).NotEmpty().WithMessage("El campo de UserId no puede ser vacio").NotNull().WithMessage("Debe de seleccionar primero un usuario para crear el odontograma.");
                RuleFor( x => x.clinicHistoryId).NotEmpty().WithMessage("El campo de clinicHistoryId no puede ser vacio").NotNull().WithMessage("Para registrar el odontograma, El usuario primero debe de tener una historia clinica creada.").ToString();
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
               if(request.clinicHistoryId == null){
                   throw new ManagerError(HttpStatusCode.NotAcceptable, new {mensaje = "Asegurese de primero haber creado una historia clinica para el usuario."});
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
                        throw new ManagerError(HttpStatusCode.BadRequest, new {mensaje = "No se pudo crear el Odontograma"});
            }
        }
    }
}
