using System;
using System.Collections.Generic;
using Domine;

namespace Aplication.OdontogramApp.Dtos
{
    public class odontogramDto
    {
        public Guid Id {get;set;}
        public DateTime date_register {get;set;}
        public string observation {get;set;}
        public ICollection<typeProcess> typeProcesses {get;set;}
        public ICollection<tooth> tooths {get;set;}
        public  string UserId {get;set;}
        public Guid typeProcessesId {get;set;}
        public Guid toothId {get;set;}
    }
}