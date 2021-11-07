using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplication.ManagerExcepcion;
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
                RuleFor( x => x.observation).NotNull().WithMessage("El campo observation no debe ser nulo").NotEmpty().WithMessage("El campo observation no debe estar vacio");
                RuleFor( x => x.dateRegister).NotNull().WithMessage("El campo dateRegister no debe ser nulo").NotEmpty().WithMessage("El campo dateRegister no debe estar vacio");
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
               
               Guid oralRadiographyId = Guid.NewGuid();
               if(request.Id != null){
                 oralRadiographyId = request.Id ?? Guid.NewGuid();
               }
               if(request.clinicHistoryId == null){
                   throw new ManagerError(HttpStatusCode.NotAcceptable, new {mensaje = "Asegurese de primero haber creado una historia clinica para el usuario."});
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
                    throw new ManagerError(HttpStatusCode.BadRequest, new {mensaje = "No se pudo añadir el registro de Radiografia oral para el paciente"});
            }
        }
    }
}