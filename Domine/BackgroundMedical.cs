using System;
using System.Collections.Generic;

namespace Domine
{
    public class BackgroundMedical
    {
        public Guid Id {get;set;}
        public string description {get;set;}
        public ICollection<backgroundMedicalClinicHistory> clinicHistoryLink {get;set;}
    }
}