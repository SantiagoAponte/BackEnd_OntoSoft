using System;
using System.Collections.Generic;

namespace Domine
{
    public class Appoinments
    {
        public string id {get;set;}
        public DateTime start {get;set;}
        public string title {get;set;}
        public string text {get;set;}
        public ICollection<UserAppoinments> userLink {get;set;}

        
    }
}