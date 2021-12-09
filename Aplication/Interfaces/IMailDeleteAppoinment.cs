using System.Threading.Tasks;
using Aplication.Security;
using Domine;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Aplication.Interfaces
{
    public interface IMailDeleteAppoinment
    {
         Task<UserManagerResponse> SendEmailAsync(string email);
    }
    public class SendMailDeleteAppoinment : IMailDeleteAppoinment
    {
         private UserManager<User> _userManger;
        private IConfiguration _configuration;
        public SendMailDeleteAppoinment(IConfiguration configuration,UserManager<User> userManager)
        {
            _configuration = configuration; 
        }

        public async Task<UserManagerResponse> SendEmailAsync(string email)
        {
            //  var user = await _userManger.FindByEmailAsync(email);
            // if (user == null){
            //      throw new ManagerError(HttpStatusCode.NotAcceptable, new {mensaje ="No existe ningun usuario con este Email"});
            // }
                // return new UserManagerResponse
                // {
                //     IsSuccess = false,
                //     Message = "No existe ningun usuario con este Email",
                // };
                //  user.Email = email;
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress("ontosoft5@gmail.com", "OntoSoft"));
            msg.AddTo(new EmailAddress(email,"Activación OntoSoft"));
            msg.SetTemplateId("d-687fe17dbedb4ac78ab5bfd9ad88a4d8");
            // msg.SetTemplateData(new{
            //         url = "Apreciado Paciente"
            //         });
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