using System;
using System.Threading;
using System.Threading.Tasks;
using Domine;
using FluentValidation;
using MediatR;
using persistence;

namespace Aplication.ClinicHistoryApp.OralRadiographyApp
{
    public class PostOralRadiography
    {
        public class Execute : IRequest {
        public Guid? Id {get;set;}
        public string observation {get;set;}
        public DateTime dateRegister {get;set;}
        public Guid clinicHistoryId {get;set;}

        }

        public class ExecuteValidator : AbstractValidator<Execute>{
            public ExecuteValidator(){
                RuleFor( x => x.observation).NotEmpty().WithMessage("El campo no debe estar vacio");
                RuleFor( x => x.dateRegister).NotEmpty().WithMessage("El campo no debe estar vacio");
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
               
               Guid oralRadiographyId = Guid.NewGuid();
               if(request.Id != null){
                 oralRadiographyId = request.Id ?? Guid.NewGuid();
               }
               
               var oralRadiography = new OralRadiography {
                   Id = oralRadiographyId,
                   observation = request.observation,
                   dateRegister = DateTime.UtcNow,
                   clinicHistoryId = request.clinicHistoryId,
                   
               };
                _context.oralRadiography.Add(oralRadiography);
                
                 var valor = await _context.SaveChangesAsync();
                        if(valor>0){
                        return Unit.Value;
                        }
                     throw new Exception("No se pudo añadir el registro de Radiografia oral para el paciente");
            }
        }
    }
}