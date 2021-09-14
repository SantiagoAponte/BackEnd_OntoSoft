using System;

namespace Domine
{
    public class userOdontogram
    {
         public Guid Id {get;set;}
        public string UserId {get;set;}

        public Odontogram Odontogram {get;set;}

        public Guid OdontogramId {get;set;}

        public User User {get;set;}
    }
}