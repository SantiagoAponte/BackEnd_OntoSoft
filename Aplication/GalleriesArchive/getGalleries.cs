using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplication.ManagerExcepcion;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Aplication.GalleriesArchive
{
    public class getGalleries
    {
        public class Execute : IRequest<galleriesDto>{
            public Guid Id {get;set;}
        }

        public class Manager : IRequestHandler<Execute, galleriesDto>
        {
            private readonly OntoSoftContext _context;
            public Manager(OntoSoftContext context){
                _context = context;
            }
            
            public async Task<galleriesDto> Handle(Execute request, CancellationToken cancellationToken)
            {
                var archive = await _context.Galleries.Where(x => x.ObjectReference == request.Id).FirstOrDefaultAsync();
                if(archive == null) {
                    throw new ManagerError(HttpStatusCode.NotFound, new {mensaje ="No se encontro la imagen"});
                }

                var archivoGenerico = new galleriesDto{
                    Data =  Convert.ToBase64String(archive.Contain),
                    Name = archive.Name,
                    Extension = archive.Extension
                };

                return archivoGenerico;
            }
        }
    }
}