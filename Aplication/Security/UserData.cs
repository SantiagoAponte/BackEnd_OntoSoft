using System;

namespace Aplication.Security
{
    public class UserData
    {

        // public string fullname {get;set;}
        public string Email {get;set;}
        public string PasswordHash {get;set;}
        public string fullName {get;set;}
        public string Token {get;set;}
        public string Username {get;set;}
        public string Imagen {get;set;}

        public ImagenPerfil imagenGeneral {get;set;}
    }
}
