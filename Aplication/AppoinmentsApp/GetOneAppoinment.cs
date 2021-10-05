using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplication.ManagerExcepcion;
using AutoMapper;
using Domine;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Aplication.AppoinmentsApp
{
    public class GetOneAppoinment
    {
        public class OneAppoinment : IRequest<AppoinmentsDto>{
            public Guid Id {get;set;}
        }

        public class Manager : IRequestHandler<OneAppoinment, AppoinmentsDto>
        {
            private readonly OntoSoftContext _context;
            private readonly IMapper _mapper;
            public Manager(OntoSoftContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<AppoinmentsDto> Handle(OneAppoinment request, CancellationToken cancellationToken)
            {
               var appoinment = await _context.Appoinments
               .Include(x=>x.userLink)
               .ThenInclude(y => y.User)
               .FirstOrDefaultAsync(a => a.Id == request.Id);

                if(appoinment==null){
                //throw new Exception("No se puede eliminar curso");
                throw new ManagerError(HttpStatusCode.NotFound, new {mensaje = "No se encontro la cita"});
                }

                var appoinmentDto = _mapper.Map<Appoinments,AppoinmentsDto>(appoinment);
               return appoinmentDto;
            }
        }
    }
}