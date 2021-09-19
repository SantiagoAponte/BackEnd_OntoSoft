using System;
using System.Collections.Generic;
using System.Linq;
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
            public Guid IdtypeProcessTooth {get;set;}
            public DateTime date_register {get;set;}
            public string observation {get;set;}
            public List<Guid> typeProcess {get;set;}
            public List<Guid> Tooths {get;set;}
        }

         public class ExecuteValidator : AbstractValidator<Execute>{
            public ExecuteValidator(){
                RuleFor( x => x.observation).NotEmpty();
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
                    throw new ManagerError(HttpStatusCode.NotFound, new {mensaje = "No se encontro el odontograma"});
                }
                /*actualizar unicamente la información de la cita*/
                odontogram.date_register = DateTime.UtcNow;
                odontogram.observation = request.observation ?? odontogram.observation;

                if(request.Tooths!=null){
                    if(request.Tooths.Count>0){
                        /*Eliminar los usuarios actuales del curso en la base de datos*/
                        var toothsBD = _context.toothsOdontogram.Where(x => x.OdontogramId == request.Id);
                        foreach(var odontogramDelete in toothsBD){
                            _context.toothsOdontogram.Remove(odontogramDelete);
                        }

                         foreach(var id in request.Tooths){
                            var toothsOdontogram = new toothsOdontogram {
                                OdontogramId = request.Id,
                                ToothId = id
                            };
                            _context.toothsOdontogram.Add(toothsOdontogram);
                        }
                     } /*Fin del procedimiento para  la relación entre el odontograma y dientes*/
                }
                    if(request.Tooths!=null){   
                    if(request.Tooths.Count>0){
                
                        /*Eliminar los usuarios actuales del curso en la base de datos*/
                        var toothBD = _context.typeProcessTooth.Where(x => x.Id == request.IdtypeProcessTooth);
                        foreach(var relationDelete in toothBD){
                             
                            _context.typeProcessTooth.Remove(relationDelete);
                        }
                        /*Procedimiento para agregar odontogramas que provienen del doctor que lo edita*/
                        if(request.typeProcess!=null){
                        foreach(var _id in request.typeProcess){
                        foreach(var _id2 in request.Tooths){
                        var typeProcessTooth = new typeProcessTooth{
                            typeProcessId = _id,
                            ToothId =  _id2
                        };
                        _context.typeProcessTooth.Add(typeProcessTooth);
                    }
                }
                       } 
                    }
                }/*Fin del procedimiento*/
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                    return Unit.Value;

                throw new Exception("¡Error! " + "No se pudo guardar los cambios en el Odontograma");
                 
                    }
                     
                }
               
        }
    }