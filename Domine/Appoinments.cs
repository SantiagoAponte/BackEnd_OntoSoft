using System;
using System.Collections.Generic;

namespace Domine
{
    public class Appoinments
    {
        public string id {get;set;}
        public DateTime start {get;set;}
        public string Title {get;set;}
        public string Text {get;set;}
        public ICollection<UserAppoinments> userLink {get;set;}

        
    }
}