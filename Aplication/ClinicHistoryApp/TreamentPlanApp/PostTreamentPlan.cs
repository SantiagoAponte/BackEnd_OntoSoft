using System;
using System.Threading;
using System.Threading.Tasks;
using Domine;
using FluentValidation;
using MediatR;
using persistence;

namespace Aplication.ClinicHistoryApp.TreamentPlanApp
{
    public class PostTreamentPlan
    {
         public class Execute : IRequest {
        public Guid? Id {get;set;}
        public string Name {get;set;}
        public string observation {get;set;}
        public Guid clinicHistoryId {get;set;}

        }

        public class ExecuteValidator : AbstractValidator<Execute>{
            public ExecuteValidator(){
                RuleFor( x => x.Name).NotEmpty().WithMessage("El campo no debe estar vacio");
                RuleFor( x => x.observation).NotEmpty().WithMessage("El campo no debe estar vacio");
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
               
               Guid treamentPlanId = Guid.NewGuid();
               if(request.Id != null){
                 treamentPlanId = request.Id ?? Guid.NewGuid();
               }
               
               var treamentPlan = new TreamentPlan {
                   Id = treamentPlanId,
                   Name = request.Name,
                   observation = request.observation,
                   clinicHistoryId = request.clinicHistoryId
                   
               };
                _context.treamentPlan.Add(treamentPlan);
                
                 var valor = await _context.SaveChangesAsync();
                        if(valor>0){
                        return Unit.Value;
                        }
                     throw new Exception("No se pudo añadir el registro de Radiografia oral para el paciente");
            }
        }
    }
}