using System;
using System.Collections.Generic;

namespace Domine
{
    public class ClinicHistory
    {
        public Guid Id {get;set;}
        public string phoneCompanion {get;set;}
        public string nameCompanion {get;set;}
         public DateTime dateRegister {get;set;}
        public User user {get;set;}
        public string UserId {get;set;}
        public bool backgroundMedical {get;set;}
        public ICollection<backgroundMedicalClinicHistory> BackgroundMedicalsLink {get;set;}
        public bool backgroundOral {get;set;}
        public ICollection<backgroundOralClinicHistory> BackgroundOralsLink {get;set;}
        // public ICollection<Odontogram> Odontogram {get;set;}
        public ICollection<OralRadiography> oralRadiographyList {get;set;}
        public ICollection<TreamentPlan> treamentPlanList {get;set;}
        public ICollection<PatientEvolution> patientEvolutionList {get;set;}
    }
}