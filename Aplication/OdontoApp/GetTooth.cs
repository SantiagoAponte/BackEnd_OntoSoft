using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domine;
using MediatR;
using Microsoft.EntityFrameworkCore;
using persistence;

namespace Aplication.OdontoApp
{
    public class GetTooth
    {
        public class ListTooths : IRequest<List<tooth>> {}
        public class Manager : IRequestHandler<ListTooths, List<tooth>>
        {
            private readonly OntoSoftContext _context;

            public Manager(OntoSoftContext context){
                _context = context;
            }

            public async Task<List<tooth>> Handle(ListTooths request, CancellationToken cancellationToken)
            {
                var tooths =  await _context.tooth.ToListAsync();
                return tooths;
            }
        }
    }
}