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
        public class ListTooth : IRequest<List<odontogramDto>> {}

        public class Manager : IRequestHandler<ListTooth, List<odontogramDto>>
        {
            private readonly OntoSoftContext _context;
            private readonly IMapper _mapper;
            public Manager(OntoSoftContext context, IMapper mapper){
                _context = context;
                _mapper = mapper;
            }
            
            public async Task<List<odontogramDto>> Handle(ListTooth request, CancellationToken cancellationToken)
            {
               var tooths = await _context.Odontogram
                .Include(x=>x.User)
                .Include(x=>x.toothLink)
                .ThenInclude(x=>x.Tooth)
                .ThenInclude(x=>x.typeProcessLink)
                .ThenInclude(x=>x.typeProcess).ToListAsync();
               
               var toothsDto = _mapper.Map<List<Odontogram>, List<odontogramDto>>(tooths);

               return toothsDto;
            }
        }
    }
}