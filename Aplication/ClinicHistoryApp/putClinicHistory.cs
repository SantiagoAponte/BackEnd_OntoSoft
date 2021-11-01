using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplication.ManagerExcepcion;
using Domine;
using FluentValidation;
using MediatR;
using persistence;

namespace Aplication.ClinicHistoryApp
{
    public class putClinicHistory
    {
        public class Execute : IRequest
        {
            public Guid Id {get;set;}
            public Guid IdPatient {get;set;}
            public Guid IdRadiography {get;set;}
            public Guid IdTreamentPlan {get;set;}
            public string phoneCompanion {get;set;}
            public string nameCompanion {get;set;}
            public DateTime dateRegister {get;set;}
            public string UserId{get;set;}
             public string Name {get;set;}
            public string observationPatient {get;set;}
            public string observationRadiography {get;set;}
            public string observationTreamentPlan {get;set;}
            public DateTime dateCreate {get;set;}
            public bool backgroundMedical {get;set;}
            public bool backgroundOral {get;set;}
            public List<Guid> ListBackgroundMedical {get;set;}
            public List<Guid> ListBackgroundOral {get;set;}
        }

         public class ExecuteValidator : AbstractValidator<Execute>{
            public ExecuteValidator(){
                RuleFor( x => x.backgroundMedical).NotEmpty().WithMessage(x => "El campo backgroundMedical no puede estar vacio");
                RuleFor( x => x.backgroundOral).NotEmpty().WithMessage(x => "El campo backgroundOral no puede estar vacio");
                RuleFor( x => x.UserId).NotEmpty().WithMessage(x => "El campo UserId no puede estar vacio");
                RuleFor( x => x.dateRegister).NotEmpty().WithMessage(x => "El campo dateRegister no puede estar vacio");
                RuleFor( x => x.ListBackgroundMedical).NotEmpty().WithMessage(x => "El campo ListBackgroundMedical no puede estar vacio");
                RuleFor( x => x.ListBackgroundOral).NotEmpty().WithMessage(x => "El campo ListBackgroundOral no puede estar vacio");
                RuleFor( x => x.observationPatient).NotEmpty().WithMessage(x=> "El campo observationPatient no debe estar vacio");
                RuleFor( x => x.observationRadiography).NotEmpty().WithMessage(x=> "El campo observationRadiography no debe estar vacio");
                RuleFor( x => x.observationTreamentPlan).NotEmpty().WithMessage(x=> "El campo observationTreamentPlan no debe estar vacio");
                RuleFor( x => x.Name).NotEmpty().WithMessage(x=>"El campo Name no debe estar vacio");
            }
        }

        public class Manejador : IRequestHandler<Execute>
        {
            private readonly OntoSoftContext _context;
            public Manejador(OntoSoftContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Execute request, CancellationToken cancellationToken)
            {
                
                var clinicHistory = await _context.clinicHistories.FindAsync(request.Id);
                  if(
                      clinicHistory==null){
                    throw new ManagerError(HttpStatusCode.NotFound, new {mensaje = "No se encontro la historia clinica"});
                }
                var patientEvolution = await _context.patientEvolution.FindAsync(request.IdPatient);
                  if(
                      patientEvolution==null){
                    throw new ManagerError(HttpStatusCode.NotFound, new {mensaje = "No se encontro la evolución del paciente"});
                }
                var treamentPlan = await _context.treamentPlan.FindAsync(request.IdTreamentPlan);
                  if(
                      treamentPlan==null){
                    throw new ManagerError(HttpStatusCode.NotFound, new {mensaje = "No se encontro el tratamiento del paciente"});
                }
                /*actualizar unicamente la información de la cita*/
                clinicHistory.dateRegister = DateTime.UtcNow;
                clinicHistory.phoneCompanion = request.phoneCompanion ?? clinicHistory.phoneCompanion;
                clinicHistory.nameCompanion = request.nameCompanion ?? clinicHistory.nameCompanion;
                // clinicHistory.UserId = request.UserId ?? clinicHistory.UserId;
                clinicHistory.backgroundMedical = request.backgroundMedical;
                clinicHistory.backgroundOral = request.backgroundOral;


                if(request.ListBackgroundMedical!=null){
                    if(request.ListBackgroundMedical.Count>0){
                        /*Eliminar los usuarios actuales del curso en la base de datos*/
                        var backgroundMedicalBD = _context.backgroundMedicalClinicHistories.Where(x => x.clinicHistoryId == request.Id);
                        foreach(var relationDelete in backgroundMedicalBD){
                            _context.backgroundMedicalClinicHistories.Remove(relationDelete);
                        }

                         foreach(var id in request.ListBackgroundMedical){
                            var backgroundMedical = new backgroundMedicalClinicHistory{
                            BackgroundMedicalsId = id,
                            clinicHistoryId = request.Id
                        };
                            _context.backgroundMedicalClinicHistories.Add(backgroundMedical);
                        }
                     } /*Fin del procedimiento para  la relación entre el odontograma y dientes*/
                }
                    if(request.ListBackgroundOral!=null){   
                    if(request.ListBackgroundOral.Count>0){
                
                        /*Eliminar los usuarios actuales del curso en la base de datos*/
                        var backgroundOralBD = _context.backgroundOralClinicHistories.Where(x => x.clinicHistoryId == request.Id);
                        foreach(var relationDelete in backgroundOralBD){
                             
                            _context.backgroundOralClinicHistories.Remove(relationDelete);
                        }
                        /*Procedimiento para agregar odontogramas que provienen del doctor que lo edita*/
                      foreach(var id in request.ListBackgroundOral){
                            var backgroundOral = new backgroundOralClinicHistory {
                                BackgroundOralsId = id,
                                clinicHistoryId = request.Id
                            };
                            _context.backgroundOralClinicHistories.Add(backgroundOral);
                        }

                       } /*Fin del procedimiento*/
                    
                }/*Fin del procedimiento*/

                var patientBD = _context.patientEvolution.Where(x => x.Id == request.IdPatient);
                        foreach(var relationDelete in patientBD){
                            _context.patientEvolution.Remove(relationDelete);
                        }

                        var NewpatientEvolution = new PatientEvolution {
                            Id = request.IdPatient,
                            observation = request.observationPatient,
                            dateCreate = DateTime.UtcNow,
                            clinicHistoryId = request.Id
                        };
                        _context.patientEvolution.Add(NewpatientEvolution);

                var oralRadiographyBD = _context.oralRadiography.Where(x => x.Id == request.IdRadiography);
                        foreach(var relationDelete in oralRadiographyBD){
                            _context.oralRadiography.Remove(relationDelete);
                        }

                        var NewOralRadiography = new OralRadiography {
                            Id = request.IdRadiography,
                            observation = request.observationRadiography,
                            dateRegister = DateTime.UtcNow,
                            clinicHistoryId = request.Id
                        };
                        _context.oralRadiography.Add(NewOralRadiography);

                        var treamentPlanBD = _context.treamentPlan.Where(x => x.Id == request.IdTreamentPlan);
                        foreach(var relationDelete in treamentPlanBD){
                            _context.treamentPlan.Remove(relationDelete);
                        }

                        var NewTreamentPlan = new TreamentPlan {
                            Id = request.IdTreamentPlan,
                            observation = request.observationTreamentPlan,
                            Name = request.Name,
                            clinicHistoryId = request.Id
                        };
                        _context.treamentPlan.Add(NewTreamentPlan);


                var result = await _context.SaveChangesAsync();

                if (result > 0)
                    return Unit.Value;

                throw new Exception("¡Error! " + "No se pudo guardar los cambios en el Odontograma");
                 
                    }
                     
                }
    }
}