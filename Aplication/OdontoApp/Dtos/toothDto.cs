using System;
using System.Collections.Generic;
using Domine;

namespace Aplication.OdontoApp.Dtos
{
    public class toothDto
    {
        // public Guid Id {get;set;}
        // public int num {get;set;}
        // public string ubicacion {get;set;}
        // public ICollection<Odontogram> odontograms {get;set;}
        public ICollection<typeProcess> typeProcesses {get;set;}
        // public Guid typeProcessId {get;set;}
    }
}