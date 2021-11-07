using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domine
{
    public class User : IdentityUser
    {
        public override string Id {get;set;}
        public string fullName {get;set;}
        public override string PhoneNumber {get;set;} = "default";
        public string phoneEmergency {get;set;} = "default";
        public string contactEmergency {get;set;} = "default";
        public string addresContact {get;set;} = "default";
        public string centerEmergency {get;set;} = "default";
        public string eps {get;set;} = "default";
        public DateTime dateBirth {get;set;} = DateTime.UtcNow.AddHours(-5);
        public string city {get;set;} = "default";
        public string address {get;set;} = "default";
        public string gender {get;set;} = "default";
        public string document {get;set;} = "default";
        public string height {get;set;} = "default";
        public string weight {get;set;} = "default";
        public char rh {get;set;} = 'x';
        public string bloodType {get;set;} = "default";
        public ICollection<UserAppoinments> appoinmentsLink {get;set;}
        public string typeDocumentId {get;set;}  = "CAC8B6D5-7D79-4455-B903-9F2B3DBC5293";
        public ICollection<Odontogram> odontogram {get;set;}
        public typeDocument typeDocument {get;set;} 
        public ClinicHistory clinicHistory {get;set;}
      


        
    }
}