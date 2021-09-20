using System;
using System.Collections.Generic;

namespace Domine
{
    public class BackgroundOral
    {
        public Guid Id {get;set;}
        public string description {get;set;}
        public ICollection<backgroundOralClinicHistory> clinicHistoryLink {get;set;}
    }
}