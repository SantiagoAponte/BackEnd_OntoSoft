using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domine
{
    public class User : IdentityUser
    {
        public override string Id {get;set;}
        public string fullName {get;set;}
        public override string PhoneNumber {get;set;}
        public string phoneEmergency {get;set;}
        public string contactEmergency {get;set;}
        public string addresContact {get;set;}
        public string centerEmergency {get;set;}
        public string eps {get;set;}
        public DateTime dateBirth {get;set;}
        public string city {get;set;}
        public string address {get;set;}
        public string gender {get;set;}
        public string document {get;set;}
        public string height {get;set;}
        public string weight {get;set;}
        public char rh {get;set;}
        public string bloodType {get;set;}
        public ICollection<UserAppoinments> appoinmentsLink {get;set;}
        public string typeDocumentId {get;set;}
        public Odontogram odontogram {get;set;}
        public typeDocument typeDocument {get;set;}
        public ClinicHistory clinicHistory {get;}
      


        
    }
}