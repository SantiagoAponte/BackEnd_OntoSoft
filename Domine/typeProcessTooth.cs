using System;

namespace Domine
{
    public class typeProcessTooth
    {
        public Guid Id {get;set;}
        public Guid ToothId {get;set;}

        public tooth Tooth {get;set;}
        public Guid OdontogramId {get;set;}
        public Odontogram odontogram {get;set;}
        public Guid faceToothId {get;set;}
        public FaceTooth faceTooth {get;set;}

        public Guid typeProcessId {get;set;} 

        public typeProcess typeProcess {get;set;}
        
    }
}