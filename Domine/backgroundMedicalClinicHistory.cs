using System;

namespace Domine
{
    public class backgroundMedicalClinicHistory
    {
        public Guid Id {get;set;}
        public Guid clinicHistoryId {get;set;}

        public ClinicHistory clinicHistory {get;set;}

        public Guid BackgroundMedicalsId {get;set;}

        public BackgroundMedical BackgroundMedicals {get;set;}
    }
}