using System;

namespace Domine
{
    public class toothsOdontogram
    {
         public Guid Id {get;set;}
        public Guid OdontogramId {get;set;}

        public Odontogram Odontogram {get;set;}

        public Guid ToothId {get;set;}

        public tooth Tooth {get;set;}
    }
}