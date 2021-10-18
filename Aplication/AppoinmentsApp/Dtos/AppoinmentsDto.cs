using System;
using System.Collections.Generic;
using Aplication.OdontoApp.Dtos;

namespace Aplication.AppoinmentsApp.Dtos
{
    public class AppoinmentsDto
    {
        public string id {get;set;}
        public DateTime start {get;set;}
        public string title {get;set;}
        public string Text {get;set;}
        public ICollection<userDto> Users {get;set;}
    }
}