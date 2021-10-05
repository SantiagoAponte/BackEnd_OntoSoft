using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domine;
using MediatR;
using Microsoft.EntityFrameworkCore;
using persistence;

namespace Aplication.ClinicHistoryApp.BackgroundMedicalApp
{
    public class getAllBackgroundMedical
    {
        public class ListbackgroundMedical : IRequest<List<BackgroundMedical>> {}
        public class Manager : IRequestHandler<ListbackgroundMedical, List<BackgroundMedical>>
        {
            private readonly OntoSoftContext _context;

            public Manager(OntoSoftContext context){
                _context = context;
            }

            public async Task<List<BackgroundMedical>> Handle(ListbackgroundMedical request, CancellationToken cancellationToken)
            {
                var backgroundMedical =  await _context.backgroundMedicals.ToListAsync();
                return backgroundMedical;
            }
        }
    }
}