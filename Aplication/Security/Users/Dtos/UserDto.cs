using System;
using System.Collections.Generic;
using Aplication.AppoinmentsApp;
using Aplication.AppoinmentsApp.Dtos;

namespace Aplication.Security.Users.Dtos
{
    public class UserDto
    {
        public Guid Id {get;set;}
         public string Email {get;set;}
        public string fullName {get;set;}
        public string UserName {get;set;}
        public ICollection<AppoinmentsDto> Appoinments {get;set;}
    }
}