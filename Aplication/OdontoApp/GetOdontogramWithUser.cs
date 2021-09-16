using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplication.ManagerExcepcion;
using Aplication.OdontoApp.Dtos;
using AutoMapper;
using Domine;
using MediatR;
using Microsoft.EntityFrameworkCore;
using persistence;

namespace Aplication.OdontoApp
{
    public class GetOdontogramWithUser
    {
          public class OneOdontogramUser : IRequest<odontogramDto>{
            public string Id {get;set;}
        }

        public class Manager : IRequestHandler<OneOdontogramUser, odontogramDto>
        {
            private readonly OntoSoftContext _context;
            private readonly IMapper _mapper;
            public Manager(OntoSoftContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<odontogramDto> Handle(OneOdontogramUser request, CancellationToken cancellationToken)
            {
               var user = await _context.Odontogram
               .Include(x=>x.toothLink)
               .ThenInclude(x=>x.Tooth)
               .ThenInclude(x=>x.typeProcessLink)
               .ThenInclude(x=>x.typeProcess)
               .FirstOrDefaultAsync(a => a.UserId == request.Id);

                if(user==null){
                //throw new Exception("No se puede eliminar curso");
                throw new ManagerError(HttpStatusCode.NotFound, new {mensaje = "No se encontro al usuario"});
                }

                var userDto = _mapper.Map<Odontogram,odontogramDto>(user);
               return userDto;
            }
        }
    }
}