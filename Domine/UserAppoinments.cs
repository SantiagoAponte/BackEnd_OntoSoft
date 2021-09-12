using System;

namespace Domine
{
    public class UserAppoinments
    {
        public Guid Id {get;set;}
        public string UserId {get;set;}

        public Appoinments Appoinments {get;set;}

        public Guid AppoinmentsId {get;set;}

        public User User {get;set;}
    }
}