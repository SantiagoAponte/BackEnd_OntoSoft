using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplication.ManagerExcepcion;
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
                RuleFor( x => x.Name).NotEmpty().WithMessage( x=> "El campo Name no debe estar vacio");
                RuleFor( x => x.observation).NotEmpty().WithMessage( x=> "El campo observation no debe estar vacio");
                RuleFor( x => x.clinicHistoryId).NotEmpty().WithMessage( x=> "El campo clinicHistoryId no debe estar vacio");
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
                        if(valor>0)
                        throw new ManagerError(HttpStatusCode.OK, new {mensaje = "¡Se creo el plan de tratamiento con exito!"});
                        return Unit.Value;
                        
                     throw new Exception("No se pudo añadir el registro de Radiografia oral para el paciente");
            }
        }
    }
}