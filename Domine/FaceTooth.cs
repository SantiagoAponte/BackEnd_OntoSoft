using System;
using System.Collections.Generic;

namespace Domine
{
    public class FaceTooth
    {
        public Guid Id {get;set;}
        public string description {get;set;}
        public ICollection<typeProcessTooth> faceToothsLink {get;set;}
    }
}