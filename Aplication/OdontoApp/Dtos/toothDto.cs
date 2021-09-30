using System;
using System.Collections.Generic;
using Domine;

namespace Aplication.OdontoApp.Dtos
{
    public class toothDto
    {
       public Guid Id {get;set;}
        public int num {get;set;}
        public string ubicacion {get;set;}
        public ICollection<typeProcessDto> typeProcess {get;set;}
        public ICollection<faceToothDto> faceTooth {get;set;}
    }
}