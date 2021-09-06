using System;
using Microsoft.AspNetCore.Identity;

namespace Domine
{
    public class User : IdentityUser
    {
        public string fullName {get;set;}
        // public byte[] Image {get;set;}
        // public String Image {get;set;}
        // public String ContactEmergency {get;set;}
        
    }
}