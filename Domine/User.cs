using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domine
{
    public class User : IdentityUser
    {
        public string fullName {get;set;}

        //falta añadirle la demas info del user, cuando guañarita acepte.
        public ICollection<UserAppoinments> appoinmentsLink {get;set;}

        
    }
}