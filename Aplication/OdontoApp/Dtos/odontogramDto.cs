using System;
using System.Collections.Generic;
using Domine;

namespace Aplication.OdontoApp.Dtos
{
    public class odontogramDto
    {
        public Guid Id {get;set;}
        public DateTime date_register {get;set;}
        public string observation {get;set;}
        public ICollection<typeProcess> typeProcesses {get;set;}
        public ICollection<tooth> tooths {get;set;}
        public  ICollection<User> Users {get;set;}
        public Guid typeProcessId {get;set;}
        public Guid ToothId {get;set;}
        public string UserId {get;set;}
    }
}