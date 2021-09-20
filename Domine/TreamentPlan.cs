using System;

namespace Domine
{
    public class TreamentPlan
    {
        public Guid Id {get;set;}
        public string Name {get;set;}
        public string observation {get;set;}
        public Guid clinicHistoryId {get;set;}
        public ClinicHistory clinicHistory {get;set;}
    }
}