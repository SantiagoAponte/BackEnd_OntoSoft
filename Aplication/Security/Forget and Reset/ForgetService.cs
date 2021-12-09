using System.Text;
using System.Threading.Tasks;
using Aplication.Interfaces;
using Aplication.Interfaces.Contracts;
using Domine;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration;
using SendGrid;
using Aplication.ManagerExcepcion;
using System.Net;

namespace Aplication.Security
{
    public class ForgetService : IForgetPassword
    {

        private UserManager<User> _userManger;
        private IConfiguration _configuration;
        private IMailService _mailService;
        public ForgetService(UserManager<User> userManager, IConfiguration configuration, IMailService mailService)
        {
            _userManger = userManager;
            _configuration = configuration;
            _mailService = mailService;
        }
        public async Task<UserManagerResponse> ForgetPasswordAsync(string email, string host)
        {
            var user = await _userManger.FindByEmailAsync(email);
            if (user == null){
                 throw new ManagerError(HttpStatusCode.NotAcceptable, new {mensaje ="No existe ningun usuario con este Email"});
            }
                // return new UserManagerResponse
                // {
                //     IsSuccess = false,
                //     Message = "No existe ningun usuario con este Email",
                // };
                 user.Email = email;
            var token = await _userManger.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Encoding.UTF8.GetBytes(token);
            var validToken = WebEncoders.Base64UrlEncode(encodedToken);
            var hostFront = host;
            string url = $"{_configuration["AppUrl"]}{hostFront}/password?token={validToken}&email={email}";
            
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress("ontosoft5@gmail.com", "OntoSoft"));
            msg.AddTo(new EmailAddress(email, "Apreciado Paciente, Recupera tu contraseña"));
            msg.SetTemplateId("d-17616c7169ba421f966e318f4e620111");
            msg.SetTemplateData(new{
                    url = $"{url}",
                    });
            var response = await client.SendEmailAsync(msg);
            return new UserManagerResponse
            {
                IsSuccess = true,
                Message = "La URL de restablecimiento de la contraseña ha sido enviada al correo electrónico con éxito."
            };
        }
    }
}