using System;
using System.Collections.Generic;

namespace Aplication.OdontoApp.Dtos
{
    public class userDto
    {
        public string Email {get;set;}
        public string fullName {get;set;}
        public string UserName {get;set;}
        public ICollection<odontogramDto> Odontograms {get;set;}
    }
}