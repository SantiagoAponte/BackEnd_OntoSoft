using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domine;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Aplication.Security.Users
{
    public class getTypeDocument
    {
        public class ListTypeDocument : IRequest<List<typeDocument>> {}
        public class Manager : IRequestHandler<ListTypeDocument, List<typeDocument>>
        {
            private readonly OntoSoftContext _context;

            public Manager(OntoSoftContext context){
                _context = context;
            }

            public async Task<List<typeDocument>> Handle(ListTypeDocument request, CancellationToken cancellationToken)
            {
                var typeDocuments =  await _context.typeDocument.ToListAsync();
                return typeDocuments;
            }
        }
    }
}