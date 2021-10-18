using System;

namespace Domine
{
    public class UserAppoinments
    {
        public Guid id {get;set;}
        public string UserId {get;set;}

        public Appoinments Appoinments {get;set;}

        public string Appoinmentsid {get;set;}

        public User User {get;set;}
    }
}