using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aplication.OdontoApp.Dtos;
using AutoMapper;
using Domine;
using MediatR;
using Microsoft.EntityFrameworkCore;
using persistence;

namespace Aplication.OdontoApp
{
    public class GetOdontogram
    {
        public class ListOdontongram : IRequest<List<odontogramDto>> {}

        public class Manager : IRequestHandler<ListOdontongram, List<odontogramDto>>
        {
            private readonly OntoSoftContext _context;
            private readonly IMapper _mapper;
            public Manager(OntoSoftContext context, IMapper mapper){
                _context = context;
                _mapper = mapper;
            }
            
            public async Task<List<odontogramDto>> Handle(ListOdontongram request, CancellationToken cancellationToken)
            {
               var appoinments = await _context.Odontogram
               .Include(x=>x.UserId)
               .Include(x=>x.toothLink)
               .ThenInclude(x => x.Tooth).ToListAsync();
               
               var appoinmentsDto = _mapper.Map<List<Odontogram>, List<odontogramDto>>(appoinments);

               return appoinmentsDto;
            }
        }
    }
}