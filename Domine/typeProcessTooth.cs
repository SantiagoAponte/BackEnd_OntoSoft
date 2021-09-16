using System;

namespace Domine
{
    public class typeProcessTooth
    {
        public Guid Id {get;set;}
        public Guid ToothId {get;set;}

        public tooth Tooth {get;set;}

        public Guid typeProcessId {get;set;}

        public typeProcess typeProcess {get;set;}
        
    }
}