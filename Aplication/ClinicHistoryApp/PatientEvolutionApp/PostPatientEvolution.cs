using System;
using System.Threading;
using System.Threading.Tasks;
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
                RuleFor( x => x.observation).NotEmpty().WithMessage("El campo no debe estar vacio");
                RuleFor( x => x.dateCreate).NotEmpty().WithMessage("El campo no debe estar vacio");
                RuleFor( x => x.clinicHistoryId).NotEmpty().WithMessage("El campo no debe estar vacio");
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
               
               var patientEvolution = new PatientEvolution {
                   Id = patientEvolutionId,
                   observation = request.observation,
                   dateCreate = DateTime.UtcNow,
                   clinicHistoryId = request.clinicHistoryId,
               };
                _context.patientEvolution.Add(patientEvolution);
                
                 var valor = await _context.SaveChangesAsync();
                        if(valor>0){
                        return Unit.Value;
                        }
                     throw new Exception("No se pudo añadir la observación de evolución al paciente");
            }
        }
    }
}