using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplication.ManagerExcepcion;
using Aplication.Security.Users.Dtos;
using AutoMapper;
using Domine;
using MediatR;
using Microsoft.EntityFrameworkCore;
using persistence;

namespace Aplication.Security.Users
{
    public class getUserDetails
    {
       public class OneDetailUser : IRequest<UserData>{
            public string Id {get;set;}
        }

        public class Manager : IRequestHandler<OneDetailUser, UserData>
        {
            private readonly OntoSoftContext _context;
            private readonly IMapper _mapper;
            public Manager(OntoSoftContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<UserData> Handle(OneDetailUser request, CancellationToken cancellationToken)
            {
               var user = await _context.User
               .FirstOrDefaultAsync(a => a.Id == request.Id);

                if(user==null){
                //throw new Exception("No se puede eliminar curso");
                throw new ManagerError(HttpStatusCode.NotFound, new {mensaje = "No se encontro al usuario"});
                }

                var userDto = _mapper.Map<User,UserData>(user);
               return userDto;
            }
        }
    }
}