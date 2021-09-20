using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplication.ManagerExcepcion;
using MediatR;
using persistence;

namespace Aplication.ClinicHistoryApp
{
    public class deleteClinicHistory
    {
        public class Execute : IRequest {
            public Guid Id {get;set;}
            public Guid IdPatient {get;set;}
            public Guid IdRadiography {get;set;}
            public Guid IdTreamentPlan {get;set;}
        }

        public class Manager : IRequestHandler<Execute>
        {
            private readonly OntoSoftContext _context;
            public Manager(OntoSoftContext context){
                _context = context;
            }
            public async Task<Unit> Handle(Execute request, CancellationToken cancellationToken)
            {
                var clinicHistory = await _context.clinicHistories.FindAsync(request.Id);
                if(clinicHistory==null){
                    
                    throw new ManagerError(HttpStatusCode.NotFound, new {mensaje = "No se encontro historia clinica"});
                }
                var patientEvolution = await _context.patientEvolution.FindAsync(request.IdPatient);
                if(patientEvolution==null){
                    throw new ManagerError(HttpStatusCode.NotFound, new {mensaje = "No se encontro la evolución del paciente"});
                }
               
                var oralRadiography = await _context.oralRadiography.FindAsync(request.IdRadiography);
                if(oralRadiography==null){
                    
                    throw new ManagerError(HttpStatusCode.NotFound, new {mensaje = "No se encontro la radiografia del paciente"});
                }
                
                var treamentPlan = await _context.treamentPlan.FindAsync(request.IdTreamentPlan);
                if(treamentPlan==null){
                    
                    throw new ManagerError(HttpStatusCode.NotFound, new {mensaje = "No se encontro el tratamiento del paciente"});
                }
                      

                /*Elimina todos los antecedentes medicos que contiene esa historia clinica*/
                var backgroundMedicalBD = _context.backgroundMedicalClinicHistories.Where(x => x.clinicHistoryId == request.Id);
                        foreach(var relationDelete in backgroundMedicalBD){
                            _context.backgroundMedicalClinicHistories.Remove(relationDelete);
                        }
                /*Elimina todos los antecedentes orales que contiene esa historia clinica*/
                var backgroundOralBD = _context.backgroundOralClinicHistories.Where(x => x.clinicHistoryId == request.Id);
                        foreach(var relationDelete in backgroundOralBD){  
                            _context.backgroundOralClinicHistories.Remove(relationDelete);
                        }

                /*Elimina todos los antecedentes orales que contiene esa historia clinica*/
                var patientBD = _context.patientEvolution.Where(x => x.Id == request.IdPatient);
                        foreach(var relationDelete in patientBD){
                            _context.patientEvolution.Remove(relationDelete);
                        }

                var oralRadiographyBD = _context.oralRadiography.Where(x => x.Id == request.IdRadiography);
                        foreach(var relationDelete in oralRadiographyBD){
                            _context.oralRadiography.Remove(relationDelete);
                        }
                
                var treamentPlanBD = _context.treamentPlan.Where(x => x.Id == request.IdTreamentPlan);
                        foreach(var relationDelete in treamentPlanBD){
                            _context.treamentPlan.Remove(relationDelete);
                        }

                var result = await _context.SaveChangesAsync();

                if(result>0){
                    return Unit.Value;
                }

                throw new Exception("¡Error! " + "No se pudieron guardar los cambios");
            }
        }
    }
}