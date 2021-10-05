using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domine;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Aplication.OdontoApp
{
    public class GetFaceTooth
    {
        public class ListfaceTooths : IRequest<List<FaceTooth>> {}
        public class Manager : IRequestHandler<ListfaceTooths, List<FaceTooth>>
        {
            private readonly OntoSoftContext _context;

            public Manager(OntoSoftContext context){
                _context = context;
            }

            public async Task<List<FaceTooth>> Handle(ListfaceTooths request, CancellationToken cancellationToken)
            {
                var tooths =  await _context.faceTooth.ToListAsync();
                return tooths;
            }
        }
    }
}