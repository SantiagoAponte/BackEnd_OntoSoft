using System;
using System.Collections.Generic;
using Aplication.Security;

namespace Aplication.AppoinmentsApp
{
    public class AppoinmentsDto
    {
        public Guid Id {get;set;}
        public DateTime dateInit {get;set;}
        public DateTime dateFinal {get;set;}
        public string Title {get;set;}
        public string Text {get;set;}
        public ICollection<UserDto> Users {get;set;}
    }
}