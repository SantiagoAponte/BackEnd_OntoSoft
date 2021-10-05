using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplication.ClinicHistoryApp.Dtos;
using Aplication.ManagerExcepcion;
using AutoMapper;
using Domine;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Aplication.ClinicHistoryApp
{
    public class getClinicHistoryWithUser
    {
        public class OneClinicHistoryUser : IRequest<clinicHistoryDto>{
            public string Id {get;set;}
        }

        public class Manager : IRequestHandler<OneClinicHistoryUser, clinicHistoryDto>
        {
            private readonly OntoSoftContext _context;
            private readonly IMapper _mapper;
            public Manager(OntoSoftContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<clinicHistoryDto> Handle(OneClinicHistoryUser request, CancellationToken cancellationToken)
            {
               var user = await _context.clinicHistories
               .Include(x=>x.user)
               .Include(x=>x.patientEvolutionList)
               .Include(x=>x.oralRadiographyList)
               .Include(x=>x.treamentPlanList)
               .Include(x=>x.Odontogram)
               .ThenInclude(x=>x.toothTypeProcessLink)
               .ThenInclude(x=>x.Tooth)
               .ThenInclude(x=>x.typeProcessLink)
               .ThenInclude(x=>x.typeProcess)
               .ThenInclude(x=>x.toothLink)
               .ThenInclude(x=>x.faceTooth)
               .Include(x=>x.BackgroundMedicalsLink)
               .ThenInclude(x=>x.BackgroundMedicals)
               .Include(x=>x.BackgroundOralsLink)
               .ThenInclude(x=>x.BackgroundOrals)
               .FirstOrDefaultAsync(a => a.UserId == request.Id);

                if(user==null){
                //throw new Exception("No se puede eliminar curso");
                throw new ManagerError(HttpStatusCode.NotFound, new {mensaje = "No se encontro al usuario"});
                }

                var userDto = _mapper.Map<ClinicHistory,clinicHistoryDto>(user);
               return userDto;
            }
        }
    }
}