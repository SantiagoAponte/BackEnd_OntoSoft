using System;
using System.Collections.Generic;
using Domine;

namespace Aplication.OdontoApp.Dtos
{
    public class typeProcessToothDto
    {
        public ICollection<tooth> Tooth {get;set;}
        public ICollection<FaceTooth> faceTooth {get;set;}

        public ICollection<typeProcess> typeProcess {get;set;}
    }
}