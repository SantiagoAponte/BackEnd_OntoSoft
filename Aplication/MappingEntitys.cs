using Aplication.AppoinmentsApp;
using Aplication.ClinicHistoryApp.Dtos;
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
            .ForMember(x => x.tooths, y => y. MapFrom( z => z.toothTypeProcessLink.Select(a => a.Tooth).ToList()));
            // .ForMember(x => x.typeProcesses, y => y. MapFrom( z => z.toothTypeProcessLink.Select(a => a.typeProcess).ToList()))
            // .ForMember(x => x.faceTooths, y => y. MapFrom( z => z.toothTypeProcessLink.Select(a => a.faceTooth).ToList()));

            CreateMap<User, UserPrueba>();

            /*Mapeo de todas las entidades que permiten construir en su totalidad la historia clinica*/
            CreateMap<ClinicHistory, clinicHistoryDto>();
            // .ForMember(x => x.BackgroundMedicals, y => y.MapFrom( z => z.BackgroundMedicalsLink.Select( a => a.BackgroundMedicals).ToList()) )
            // .ForMember(x => x.BackgroundOrals, y => y.MapFrom( z => z.BackgroundOralsLink.Select( a => a.BackgroundOrals).ToList()) )
            // .ForMember(x => x.patientEvolutions, y => y.MapFrom(z => z.patientEvolutionList))
            // .ForMember(x => x.oralRadiographies, y => y.MapFrom(z => z.oralRadiographyList))
            // .ForMember(x => x.treamentPlans, y => y.MapFrom(z => z.treamentPlanList))
            // .ForMember(x => x.Odontograms, y => y.MapFrom(z => z.Odontogram));

            CreateMap<BackgroundMedical, backgroundMedicalDto>();
            CreateMap<BackgroundOral, backgroundOralDto>();
            CreateMap<PatientEvolution, patientEvolutionDto>();
            CreateMap<OralRadiography, oralRadiographyDto>();
            CreateMap<TreamentPlan, treamentPlanDto>();
            CreateMap<typeProcess, typeProcessDto>();
            CreateMap<tooth, toothDto>();
            CreateMap<FaceTooth,faceToothDto>();
            // .ForMember(x => x.faceTooths, y => y.MapFrom( z => z.typeProcessToothLink.Select( a => a.faceTooth).ToList()) );
            // .ForMember(x => x.odontograms, y => y.MapFrom( z => z.odontogramLink.Select( a => a.Odontogram).ToList()) );
            // .ForMember(x => x.tooths, y => y.MapFrom( z => z.typeProcessLink.Select( a => a.typeProcess).ToList()) );

            
            
        }
    }
}