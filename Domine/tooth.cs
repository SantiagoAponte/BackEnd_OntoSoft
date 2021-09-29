using System;
using System.Collections.Generic;

namespace Domine
{
    public class tooth
    {
        public Guid Id {get;set;}
        public int num {get;set;}
        public string ubicacion {get;set;}
        // public ICollection<toothsOdontogram> odontogramLink {get;set;}
        public ICollection<typeProcessTooth> typeProcessLink {get;set;}


    }
}