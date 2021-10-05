using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplication.ManagerExcepcion;
using MediatR;
using Persistence;

namespace Aplication.AppoinmentsApp
{
    public class deleteAppoinments
    {
        public class Execute : IRequest {
            public Guid Id {get;set;}
        }

        public class Manager : IRequestHandler<Execute>
        {
            private readonly OntoSoftContext _context;
            public Manager(OntoSoftContext context){
                _context = context;
            }
            public async Task<Unit> Handle(Execute request, CancellationToken cancellationToken)
            {
                /* Elimina todos los usuarios que contiene esa cita*/
                var usersBD = _context.UserAppoinments.Where(x=> x.AppoinmentsId == request.Id);
                foreach(var user in usersBD){
                    _context.UserAppoinments.Remove(user);
                }
                
                var appoinment = await _context.Appoinments.FindAsync(request.Id);
                if(appoinment==null){
                    //throw new Exception("No se puede eliminar curso");
                    throw new ManagerError(HttpStatusCode.NotFound, new {mensaje = "No se encontro la cita"});
                }
                _context.Remove(appoinment);

             

                var result = await _context.SaveChangesAsync();

                if(result>0){
                    return Unit.Value;
                }

                throw new Exception("¡Error! " + "No se pudieron guardar los cambios");
            }
        }
    }
}