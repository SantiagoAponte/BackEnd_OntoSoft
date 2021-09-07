using System;

namespace Aplication.GalleriesArchive
{
    public class galleriesDto
    {
        public Guid Id {get;set;}
        public Guid ObjectReference {get;set;}
        public string Name {get;set;}
        public string Extension {get;set;}
        public string Data {get;set;}
    }
}