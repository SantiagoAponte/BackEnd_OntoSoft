using System;

namespace Domine
{
    public class backgroundOralClinicHistory
    {
        public Guid Id {get;set;}
        public Guid clinicHistoryId {get;set;}

        public ClinicHistory clinicHistory {get;set;}

        public Guid BackgroundOralsId {get;set;}

        public BackgroundOral BackgroundOrals {get;set;}
    }
}