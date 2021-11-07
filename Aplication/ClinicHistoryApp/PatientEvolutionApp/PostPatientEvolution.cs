using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplication.ManagerExcepcion;
using Domine;
using FluentValidation;
using MediatR;
using persistence;

namespace Aplication.ClinicHistoryApp.PatientEvolutionApp
{
    public class PostPatientEvolution
    {
        public class Execute : IRequest {
        public Guid? Id {get;set;}
        public string observation {get;set;}
        public DateTime dateCreate {get;set;}
        public Guid clinicHistoryId {get;set;}

        }

        public class ExecuteValidator : AbstractValidator<Execute>{
            public ExecuteValidator(){
                RuleFor( x => x.observation).NotNull().WithMessage("El campo observation no debe ser nulo").NotEmpty().WithMessage("El campo observation no debe estar vacio");
                RuleFor( x => x.dateCreate).NotNull().WithMessage("El campo dateCreate no debe ser nulo").NotEmpty().WithMessage("El campo dateCreate no debe estar vacio");
                RuleFor( x => x.clinicHistoryId).NotNull().WithMessage("El campo clinicHistoryId no debe ser nulo").NotEmpty().WithMessage("El campo clinicHistoryId no debe estar vacio");
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
               
               Guid patientEvolutionId = Guid.NewGuid();
               if(request.Id != null){
                 patientEvolutionId = request.Id ?? Guid.NewGuid();
               }
               if(request.clinicHistoryId == null){
                   throw new ManagerError(HttpStatusCode.NotAcceptable, new {mensaje = "Asegurese de primero haber creado una historia clinica para el usuario."});
               }
               
               var patientEvolution = new PatientEvolution {
                   Id = patientEvolutionId,
                   observation = request.observation,
                   dateCreate = DateTime.UtcNow.AddHours(-5),
                   clinicHistoryId = request.clinicHistoryId,
               };
                _context.patientEvolution.Add(patientEvolution);
                
                 var valor = await _context.SaveChangesAsync();
                        if(valor>0){
                        return Unit.Value;
                        }
                     throw new ManagerError(HttpStatusCode.BadRequest, new {mensaje = "No se pudo añadir la observación de evolución al paciente"});
            }
        }
    }
}