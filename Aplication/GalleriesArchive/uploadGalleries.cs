using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Domine;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Aplication.GalleriesArchive
{
    public class uploadGalleries
    {
        public class Execute : IRequest {
            public Guid? ObjectReference {get;set;}
            public string Data {get;set;}
            public string Name {get;set;}
            public string Extension {get;set;}
        }

        public class Manager : IRequestHandler<Execute>
        {
            
            private readonly OntoSoftContext _context;
            public Manager(OntoSoftContext context) {
                _context = context;
            }
            public async Task<Unit> Handle(Execute request, CancellationToken cancellationToken)
            {
                var archive = await _context.Galleries.Where(x => x.ObjectReference == request.ObjectReference).FirstOrDefaultAsync();
                if(archive == null) {
                    var arch = new Galleries(){
                        Contain =  Convert.FromBase64String(request.Data),
                        Name = request.Name,
                        Extension = request.Extension,
                        Id = Guid.NewGuid(),
                        dateCreate = DateTime.UtcNow,
                        ObjectReference = request.ObjectReference ?? Guid.Empty
                    };
                    _context.Galleries.Add(arch);
                }else{
                    archive.Contain = Convert.FromBase64String(request.Data);
                    archive.Name = request.Name;
                    archive.Extension = request.Extension;
                    archive.dateCreate = DateTime.UtcNow;
                }

               var resultado = await _context.SaveChangesAsync();
                
                if(resultado>0){
                    return Unit.Value;
                }

                throw new Exception("No se pudo guardar el archivo");
            }
        }
    }
}