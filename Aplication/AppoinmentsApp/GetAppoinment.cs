using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domine;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Aplication.AppoinmentsApp
{
    public class GetAppoinment
    {
        public class ListAppoinments : IRequest<List<AppoinmentsDto>> {}

        public class Manager : IRequestHandler<ListAppoinments, List<AppoinmentsDto>>
        {
            private readonly OntoSoftContext _context;
            private readonly IMapper _mapper;
            public Manager(OntoSoftContext context, IMapper mapper){
                _context = context;
                _mapper = mapper;
            }
            
            public async Task<List<AppoinmentsDto>> Handle(ListAppoinments request, CancellationToken cancellationToken)
            {
               var appoinments = await _context.Appoinments
               .Include(x=>x.userLink)
               .ThenInclude(x => x.User).ToListAsync();
               
               var appoinmentsDto = _mapper.Map<List<Appoinments>, List<AppoinmentsDto>>(appoinments);

               return appoinmentsDto;
            }
        }
    }
}