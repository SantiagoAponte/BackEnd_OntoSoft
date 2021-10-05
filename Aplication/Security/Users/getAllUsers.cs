using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aplication.Security.Users.Dtos;
using AutoMapper;
using Domine;
using MediatR;
using Microsoft.EntityFrameworkCore;
using persistence;

namespace Aplication.Security.Users
{
    public class getAllUsers
    {
        public class ListUsers : IRequest<List<UserPrueba>> {}

        public class Manager : IRequestHandler<ListUsers, List<UserPrueba>>
        {
            private readonly OntoSoftContext _context;
            private readonly IMapper _mapper;
            public Manager(OntoSoftContext context, IMapper mapper){
                _context = context;
                _mapper = mapper;
                
            }
            
            public async Task<List<UserPrueba>> Handle(ListUsers request, CancellationToken cancellationToken)
            {
               
               var detailsUser = await _context.User.ToListAsync();
               var userDto = _mapper.Map<List<User>, List<UserPrueba>>(detailsUser);

               return userDto;
            }
        }
    }
}