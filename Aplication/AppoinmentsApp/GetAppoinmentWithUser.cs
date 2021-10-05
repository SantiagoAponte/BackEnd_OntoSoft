using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplication.ManagerExcepcion;
using Aplication.Security;
using Aplication.Security.Users.Dtos;
using AutoMapper;
using Domine;
using MediatR;
using Microsoft.EntityFrameworkCore;
using persistence;

namespace Aplication.AppoinmentsApp
{
    public class GetAppoinmentWithUser
    {
        public class OneAppoinmentUser : IRequest<UserDto>{
            public string Id {get;set;}
        }

        public class Manager : IRequestHandler<OneAppoinmentUser, UserDto>
        {
            private readonly OntoSoftContext _context;
            private readonly IMapper _mapper;
            public Manager(OntoSoftContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<UserDto> Handle(OneAppoinmentUser request, CancellationToken cancellationToken)
            {
               var user = await _context.User
               .Include(x=>x.appoinmentsLink)
               .ThenInclude(y => y.Appoinments)
               .FirstOrDefaultAsync(a => a.Id == request.Id);

                if(user==null){
                //throw new Exception("No se puede eliminar curso");
                throw new ManagerError(HttpStatusCode.NotFound, new {mensaje = "No se encontro al usuario"});
                }

                var userDto = _mapper.Map<User,UserDto>(user);
               return userDto;
            }
        }
    }
}