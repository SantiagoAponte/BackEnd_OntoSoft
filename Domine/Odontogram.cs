using System;
using System.Collections.Generic;

namespace Domine
{
    public class Odontogram
    {
        public Guid Id {get;set;}
        public DateTime date_register {get;set;}
        public string observation {get;set;}
        // public ICollection<typeProcessOdontogram> typeProcessesLink {get;set;}
        public ICollection<toothsOdontogram> toothLink {get;set;}
        // public ICollection<userOdontogram> userLink {get;set;}
        public string UserId {get;set;}
        public User User {get;set;}

    }
}