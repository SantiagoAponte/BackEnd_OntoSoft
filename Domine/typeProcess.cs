using System;
using System.Collections.Generic;

namespace Domine
{
    public class typeProcess
    {
        public Guid Id {get;set;}
        public string name {get;set;}
        public ICollection<typeProcessTooth> toothLink {get;set;}




    }
}