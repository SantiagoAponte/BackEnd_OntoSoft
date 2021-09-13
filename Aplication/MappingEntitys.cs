using Aplication.AppoinmentsApp;
using Aplication.OdontogramApp.Dtos;
using Aplication.Security;
using AutoMapper;
using Domine;
using System.Linq;

namespace Aplication
{
    public class MappingEntitys : Profile
    {
        public MappingEntitys() {
            
            /* Mapeo de las citas para encontrarla y traer todos los usuarios asignadas a ellas */
            CreateMap<Appoinments, AppoinmentsDto>()
            .ForMember(x => x.Users, y => y.MapFrom( z => z.userLink.Select( a => a.User).ToList()) );

            /*Mapeo de las citas para encontrarlas por el ID de usuario, es decir unicamente las citas de un usuario */
            CreateMap<User, UserDto>()
            .ForMember(x => x.Appoinments, y => y.MapFrom(z => z.appoinmentsLink.Select(a => a.Appoinments).ToList()));

            /*Mapeo de todos los dientes con sus caracteristicas para cargarlos al ontograma */
            CreateMap<Odontogram, odontogramDto>()
            .ForMember(x => x.typeProcesses, y => y.MapFrom(z => z.typeProcessesList))
            .ForMember(x => x.tooths, y => y.MapFrom(z => z.toothList));
        }
    }
}