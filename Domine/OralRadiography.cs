using System;

namespace Domine
{
    public class OralRadiography
    {
        public Guid Id {get;set;}
        public DateTime dateRegister {get;set;}
        public string observation {get;set;}
        public Guid clinicHistoryId {get;set;}
        public ClinicHistory clinicHistory {get;set;}
    }
}