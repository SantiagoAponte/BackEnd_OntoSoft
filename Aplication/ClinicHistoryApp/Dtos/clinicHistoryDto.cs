using System;
using System.Collections.Generic;
using Aplication.OdontoApp.Dtos;
using Domine;

namespace Aplication.ClinicHistoryApp.Dtos
{
    public class clinicHistoryDto
    {
        public Guid Id {get;set;}
        public string phoneCompanion {get;set;}
        public string nameCompanion {get;set;}
        public DateTime dateRegister {get;set;}
        public userDto UserDetails {get;set;}
        public ICollection<odontogramDto> Odontograms {get;set;}
        public bool backgroundMedical {get;set;}
        public ICollection<backgroundMedicalDto> BackgroundMedicals {get;set;}
        public bool backgroundOral {get;set;}
        public ICollection<backgroundOralDto> BackgroundOrals {get;set;}
        public ICollection<oralRadiographyDto> oralRadiographies {get;set;}
        public ICollection<treamentPlanDto> treamentPlans {get;set;}
        public ICollection<patientEvolutionDto> patientEvolutions {get;set;}
    }
}