using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using persistence;

namespace Aplication.Security
{
    public class RolList
    {
        public class Execute : IRequest<List<IdentityRole>> {

        }
        public class Manager : IRequestHandler<Execute, List<IdentityRole>>{
            private readonly OntoSoftContext _context;
            public Manager(OntoSoftContext context){
                _context = context;
            }
            public async Task<List<IdentityRole>> Handle(Execute request, CancellationToken cancellationToken)
            {
              var roles = await _context.Roles.ToListAsync();
              return roles;
            }
        }
    }
}