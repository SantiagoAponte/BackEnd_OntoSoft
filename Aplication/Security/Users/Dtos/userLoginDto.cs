namespace Aplication.Security.Users.Dtos
{
    public class userLoginDto
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Token {get;set;}
         public string Message {get;set;}
         public ImagenPerfil imagenPerfil {get;set;}
    }
}