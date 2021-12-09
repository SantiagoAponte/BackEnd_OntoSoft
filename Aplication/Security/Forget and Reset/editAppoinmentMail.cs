using System.Threading.Tasks;
using Aplication.Interfaces;
using Aplication.Interfaces.Contracts;
using Domine;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Aplication.Security.Forget_and_Reset
{
    public class editAppoinmentMail : IMailEditAppoinment
    {
         private UserManager<User> _userManger;
        private IConfiguration _configuration;
         private IMailService _mailService;
        public editAppoinmentMail(IConfiguration configuration,UserManager<User> userManager,IMailService mailService)
        {
            _configuration = configuration; 
            _mailService = mailService;
        }
        public async Task<UserManagerResponse> SendEmailAppoinmentAsync(string email, string date, string time)
        {
            //  var user = await _userManger.FindByEmailAsync(email);
            // if (user == null){
            //      throw new ManagerError(HttpStatusCode.NotAcceptable, new {mensaje ="No existe ningun usuario con este Email"});
            // }
            // user.Email = email;
            var apiKey = "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"; //insertar aqui la key  que se usara para cuando se cambie la cita
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress("ontosoft5@gmail.com", "OntoSoft"));
            msg.AddTo(new EmailAddress(email,"Activación OntoSoft"));
            msg.SetTemplateId("d-2a898d9a51704d1ba6a6c34003195938"); //insertar aqui plantilla que se usara para cuando se cambie la cita
            msg.SetTemplateData(new{
                    date = $"{date}",
                    time = $"{time}"
                    });
            var response = await client.SendEmailAsync(msg);

            return new UserManagerResponse
            {
                IsSuccess = true,
                Message = "Se envio el correo de activación al usuario."
            };

            // var apiKey = "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";
            // var client = new SendGridClient(apiKey);
            // var from = new EmailAddress("ontosoft5@gmail.com", "OntoSoft Recupera tu contraseña!");
            // var to = new EmailAddress(toEmail);
            // var msg = MailHelper.CreateSingleTemplateEmail(from, to,"d-17616c7169ba421f966e318f4e620111", dynamicTemplateData);
            // var response = await client.SendEmailAsync(msg);
        } 
    }
}