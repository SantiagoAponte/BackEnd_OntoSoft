using Aplication.AppoinmentsApp;
using Aplication.OdontoApp.Dtos;
using Aplication.Security;
using Aplication.Security.Users.Dtos;
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

            /*Mapeo de todos los dientes con sus caracteristicas para cargarlos al odontograma */
            CreateMap<Odontogram, odontogramDto>()
            .ForMember(x => x.tooths, y => y.MapFrom( z => z.toothLink.Select( a => a.Tooth).ToList()));

            CreateMap<User, UserPrueba>();
            
            // CreateMap<typeProcess, typeProcessDto>()
            // .ForMember(x => x.tooths, y => y.MapFrom( z => z.toothLink.Select( a => a.Tooth).ToList()) );
          

            // CreateMap<tooth, toothDto>()
            // .ForMember(x => x.odontograms, y => y.MapFrom( z => z.odontogramLink.Select( a => a.Odontogram).ToList()) )
            // .ForMember(x => x.typeProcesses, y => y.MapFrom( z => z.typeProcessLink.Select( a => a.typeProcess).ToList()) );

            
            
        }
    }
}