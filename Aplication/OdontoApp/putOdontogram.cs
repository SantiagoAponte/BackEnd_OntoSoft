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

namespace Aplication.OdontoApp
{
    public class putOdontogram
    {
         public class Execute : IRequest
        {
            public Guid Id {get;set;}
            public DateTime date_register {get;set;}
            public string observation {get;set;}
            public List<typeProcessTooth> typeProcessTooth {get;set;}
            public Guid ToothId {get;set;}
        }

         public class ExecuteValidator : AbstractValidator<Execute>{
            public ExecuteValidator(){
                RuleFor( x => x.observation).NotEmpty().WithMessage("El campo de observation no puede ser vacio").NotNull().WithMessage("El campo de observación no puede ser vacio o nulo.");
                RuleFor( x => x.date_register).NotEmpty().WithMessage("El campo de date_register no puede ser vacio").NotNull().WithMessage("El campo de fecha de registro no puede ser vacio o nulo");
                RuleFor( x => x.typeProcessTooth).NotEmpty().WithMessage("El campo de typeProcessTooth no puede ser vacio").NotNull().WithMessage("Asegurese de haber seleccionado al menos un procedimiento en algun diente");
            }
        }

        public class Manejador : IRequestHandler<Execute>
        {
            private readonly OntoSoftContext _context;
            public Manejador(OntoSoftContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Execute request, CancellationToken cancellationToken)
            {
                
                var odontogram = await _context.Odontogram.FindAsync(request.Id);
                  if(
                      odontogram==null){
                    throw new ManagerError(HttpStatusCode.NotAcceptable, new {mensaje = "No se encontro el odontograma"});
                }
                /*actualizar unicamente la información de la cita*/
                odontogram.date_register = DateTime.UtcNow;
                odontogram.observation = request.observation ?? odontogram.observation;
                odontogram.toothTypeProcessLink = request.typeProcessTooth;

                // if(request.Tooths!=null){
                //     if(request.Tooths.Count>0){
                //         /*Eliminar los usuarios actuales del curso en la base de datos*/
                //         var toothsBD = _context.toothsOdontogram.Where(x => x.OdontogramId == request.Id);
                //         foreach(var odontogramDelete in toothsBD){
                //             _context.toothsOdontogram.Remove(odontogramDelete);
                //         }

                //          foreach(var id in request.Tooths){
                //             var toothsOdontogram = new toothsOdontogram {
                //                 OdontogramId = request.Id,
                //                 ToothId = id
                //             };
                //             _context.toothsOdontogram.Add(toothsOdontogram);
                //         }
                //      } /*Fin del procedimiento para  la relación entre el odontograma y dientes*/
                // }
                //     if(request.ToothId!=null){   
                //     if(request.typeProcess.Count>0){
                
                //         /*Eliminar los usuarios actuales del curso en la base de datos*/
                //         var toothTypeProcessBD = _context.typeProcessTooth.Where(x => x.OdontogramId == request.Id);
                //         foreach(var relationDelete in toothTypeProcessBD){
                             
                //             _context.typeProcessTooth.Remove(relationDelete);
                //         }
                //         /*Procedimiento para agregar odontogramas que provienen del doctor que lo edita*/
                //         if(request.typeProcess!=null){
                //         foreach(var _id in request.typeProcess){
                //         var toothTypeProcessLink = new typeProcessTooth{
                //             ToothId =  request.ToothId,
                //             typeProcessId = _id,
                //             OdontogramId = request.Id
                //         };
                //         _context.typeProcessTooth.Add(toothTypeProcessLink); 
                // }
                //        } 
                //     }
                // }/*Fin del procedimiento*/
                var result = await _context.SaveChangesAsync();

                if (result > 0){
                return Unit.Value;
                }
                throw new ManagerError(HttpStatusCode.BadRequest, new {mensaje = "¡Error! " + "No se pudo guardar los cambios en el Odontograma"});
                    }
                     
                }
               
        }
    }