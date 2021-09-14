using System;

namespace Domine
{
    public class typeProcessOdontogram
    {
        public Guid Id {get;set;}
        public Guid OdontogramId {get;set;}

        public Odontogram Odontogram {get;set;}

        public Guid typeProcessId {get;set;}

        public typeProcess typeProcess {get;set;}
    }
}