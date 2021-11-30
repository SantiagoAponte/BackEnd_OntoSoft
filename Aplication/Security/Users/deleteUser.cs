using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplication.ManagerExcepcion;
using MediatR;
using persistence;

namespace Aplication.Security.Users
{
    public class deleteUser
    {
        public class Execute : IRequest {
            public string Id {get;set;}
        }

        public class Manager : IRequestHandler<Execute>
        {
            private readonly OntoSoftContext _context;
            public Manager(OntoSoftContext context){
                _context = context;
            }
            public async Task<Unit> Handle(Execute request, CancellationToken cancellationToken)
            {
                var userId = await _context.Users.FindAsync(request.Id);
                if(userId==null){
                    //throw new Exception("No se puede eliminar curso");
                    throw new ManagerError(HttpStatusCode.NotAcceptable, new {mensaje = "No se encontro el usuario a eliminar"});
                }
                /* Elimina todos los usuarios que contiene esa cita*/
                var usersBD = _context.Users.Where(x=> x.Id == request.Id);
                foreach(var user in usersBD){
                    _context.Users.Remove(user);
                }
                var clinicHistoryBD = _context.clinicHistories.Where(x => x.UserId == request.Id);
                        foreach(var relationDelete in clinicHistoryBD){
                            _context.clinicHistories.Remove(relationDelete);
                        }

                var result = await _context.SaveChangesAsync();

                if(result>0){
                    return Unit.Value;
                }
                throw new ManagerError(HttpStatusCode.BadRequest, new {mensaje ="¡Error! " + "No se pudieron guardar los cambios"});
            }
        }
    }
}