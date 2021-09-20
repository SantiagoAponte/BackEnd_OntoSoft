using System;

namespace Domine
{
    public class PatientEvolution
    {
        public Guid Id {get;set;}
        public string observation {get;set;}
        public DateTime dateCreate {get;set;}
        public Guid clinicHistoryId {get;set;}
        public ClinicHistory clinicHistory {get;set;}
    }
}