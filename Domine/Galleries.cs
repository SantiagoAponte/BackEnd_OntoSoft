using System;

namespace Dominio
{
    public class Galleries
    {
        public Guid Id {get;set;}
        public Guid ObjectReference {get;set;}
        public string Name {get;set;}
        public string Extension {get;set;}
        public byte[] Contain {get;set;}
        public DateTime dateCreate {get;set;}
    }
}