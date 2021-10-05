using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domine;
using MediatR;
using Microsoft.EntityFrameworkCore;
using persistence;

namespace Aplication.OdontoApp
{
    public class GetTypeProcess
    {
        public class ListTypeProcess : IRequest<List<typeProcess>> {}
        public class Manager : IRequestHandler<ListTypeProcess, List<typeProcess>>
        {
            private readonly OntoSoftContext _context;

            public Manager(OntoSoftContext context){
                _context = context;
            }

            public async Task<List<typeProcess>> Handle(ListTypeProcess request, CancellationToken cancellationToken)
            {
                var typeProcess =  await _context.typeProcess.ToListAsync();
                return typeProcess;
            }
        }
    }
}