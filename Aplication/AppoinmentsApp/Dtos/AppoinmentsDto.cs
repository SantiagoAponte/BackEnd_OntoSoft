using System;
using System.Collections.Generic;
using Aplication.OdontoApp.Dtos;

namespace Aplication.AppoinmentsApp.Dtos
{
    public class AppoinmentsDto
    {
        public Guid Id {get;set;}
        public DateTime date {get;set;}
        public string Title {get;set;}
        public string Text {get;set;}
        public ICollection<userDto> Users {get;set;}
    }
}