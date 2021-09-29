using System;
using System.Collections.Generic;
using Domine;

namespace Aplication.OdontoApp.Dtos
{
    public class odontogramDto
    {
        // public Guid Id {get;set;}
        // public DateTime date_register {get;set;}
        // public string observation {get;set;}
        public ICollection<toothDto> tooths {get;set;}
        // public ICollection<typeProcessDto> typeProcesses {get;set;}
        // public ICollection<faceToothDto> faceTooths {get;set;}
    }
}