using System;
using System.Collections.Generic;

namespace Domine
{
    public class Appoinments
    {
        public Guid Id {get;set;}
        public DateTime dateInit {get;set;}
        public DateTime dateFinal {get;set;}
        public string Title {get;set;}
        public string Text {get;set;}
        public ICollection<UserAppoinments> userLink {get;set;}

        
    }
}