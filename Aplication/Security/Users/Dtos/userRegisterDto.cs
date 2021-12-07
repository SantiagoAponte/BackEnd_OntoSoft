namespace Aplication.Security.Users.Dtos
{
    public class userRegisterDto
    {
        public string Email {get;set;}
        public string Token {get;set;}
        public string Username {get;set;}
        public string fullName {get;set;}
        public string RolName {get;set;}
        public ImagenPerfil imagenPerfil {get;set;}
    }
}