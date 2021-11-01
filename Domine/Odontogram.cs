using System;
using System.Collections.Generic;

namespace Domine
{
    public class Odontogram
    {
        public Guid Id {get;set;}
        public DateTime date_register {get;set;}
        public string observation {get;set;}
        // public ICollection<toothsOdontogram> toothLink {get;set;}
        public string UserId {get;set;}
        public User User {get;set;}
        // public Guid clinicHistoryId {get;set;}
        // public ClinicHistory clinicHistory {get;set;}
        public List<typeProcessTooth>  toothTypeProcessLink {get;set;}

    }
}