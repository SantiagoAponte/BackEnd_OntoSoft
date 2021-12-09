using System.Threading.Tasks;
using Aplication.Interfaces;
using Microsoft.Extensions.Configuration;
using Domine;
using Microsoft.AspNetCore.Identity;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Aplication.Security
{
    public class createAppoinmentMail : IMailCreateAppoinment
    {
         private UserManager<User> _userManger;
        private IConfiguration _configuration;
         private IMailService _mailService;
        public createAppoinmentMail(IConfiguration configuration,UserManager<User> userManager,IMailService mailService)
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
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress("ontosoft5@gmail.com", "OntoSoft"));
            msg.AddTo(new EmailAddress(email,"Activación OntoSoft"));
            msg.SetTemplateId("d-8bde5f2be4c04e56b15308c42b5b527a");
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