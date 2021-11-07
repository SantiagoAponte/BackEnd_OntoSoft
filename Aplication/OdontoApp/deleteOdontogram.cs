using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplication.ManagerExcepcion;
using MediatR;
using persistence;

namespace Aplication.OdontoApp
{
    public class deleteOdontogram
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
                // var odontogramBD = _context.toothsOdontogram.Where(x=> x.OdontogramId == request.Id);
                // foreach(var odontogram in odontogramBD){
                //     _context.toothsOdontogram.Remove(odontogram);
                // }
                
                var odonto = await _context.Odontogram.FindAsync(request.Id);
                if(odonto==null){
                    //throw new Exception("No se puede eliminar curso");
                    throw new ManagerError(HttpStatusCode.NotAcceptable, new {mensaje = "No se encontro el Odontograma"});
                }
                _context.Remove(odonto);

                var processToothBD = _context.typeProcessTooth.Where(x=> x.OdontogramId == request.Id);
                foreach(var processTooth in processToothBD){
                    _context.typeProcessTooth.Remove(processTooth);
                }

                var result = await _context.SaveChangesAsync();

                if(result>0){
                return Unit.Value;
                }
                throw new ManagerError(HttpStatusCode.BadRequest, new {mensaje = "¡Error! " + "No se pudieron guardar los cambios"});
            }
        }
    }
}