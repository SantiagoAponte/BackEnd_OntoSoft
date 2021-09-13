using System;
using System.Collections.Generic;

namespace Domine
{
    public class Odontogram
    {
        public Guid Id {get;set;}
        public DateTime date_register {get;set;}
        public string observation {get;set;}
        public ICollection<typeProcess> typeProcessesList {get;set;}
        public ICollection<tooth> toothList {get;set;}
        public  User user {get;set;}
    }
}