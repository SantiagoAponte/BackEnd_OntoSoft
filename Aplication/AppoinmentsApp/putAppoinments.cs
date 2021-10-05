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
using Persistence;

namespace Aplication.AppoinmentsApp
{
    public class putAppoinments
    {
         public class Execute : IRequest
        {
            public Guid Id { get; set; }
             public DateTime dateInit {get;set;}
            public DateTime dateFinal {get;set;}
            public string Title {get;set;}
            public string Text {get;set;}
            public List<string> ListUsers {get;set;}
        }

         public class ExecuteValidator : AbstractValidator<Execute>{
            public ExecuteValidator(){
                RuleFor( x => x.Title).NotEmpty();
                RuleFor( x => x.Text).NotEmpty();
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
                var appoinment = await _context.Appoinments.FindAsync(request.Id);
                  if(
                      appoinment==null){
                    throw new ManagerError(HttpStatusCode.NotFound, new {mensaje = "No se encontro la cita"});
                }
                /*actualizar unicamente la información de la cita*/
                appoinment.dateInit = request.dateInit;
                appoinment.dateFinal = request.dateFinal;
                appoinment.Title = request.Title ?? appoinment.Title;
                appoinment.Text = request.Text ?? appoinment.Text;

                if(request.ListUsers!=null){
                    if(request.ListUsers.Count>0){
                        /*Eliminar los usuarios actuales del curso en la base de datos*/
                        var usersBD = _context.UserAppoinments.Where(x => x.AppoinmentsId == request.Id);
                        foreach(var userDelete in usersBD){
                            _context.UserAppoinments.Remove(userDelete);
                        }
                        /*Fin del procedimiento para eliminar usuarios*/

                        /*Procedimiento para agregar usuarios que provienen del cliente que edita la cita*/
                        foreach(var id in request.ListUsers){
                            var newUser = new UserAppoinments {
                                AppoinmentsId = request.Id,
                                UserId = id
                            };
                            _context.UserAppoinments.Add(newUser);
                        }

                       } /*Fin del procedimiento*/
                    
                }
                 var result = await _context.SaveChangesAsync();

                if (result > 0)
                    return Unit.Value;

                throw new Exception("¡Error! " + "No se pudo guardar los cambios de la cita");
            }
        }
    }
}


    