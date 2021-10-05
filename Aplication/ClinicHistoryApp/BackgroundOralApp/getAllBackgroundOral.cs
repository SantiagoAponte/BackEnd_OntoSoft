using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domine;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Aplication.ClinicHistoryApp.BackgroundOralApp
{
    public class getAllBackgroundOral
    {
        public class ListbackgroundOral : IRequest<List<BackgroundOral>> {}
        public class Manager : IRequestHandler<ListbackgroundOral, List<BackgroundOral>>
        {
            private readonly OntoSoftContext _context;

            public Manager(OntoSoftContext context){
                _context = context;
            }

            public async Task<List<BackgroundOral>> Handle(ListbackgroundOral request, CancellationToken cancellationToken)
            {
                var backgroundOral =  await _context.backgroundOrals.ToListAsync();
                return backgroundOral;
            }
        }
    }
}