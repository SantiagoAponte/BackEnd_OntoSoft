using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using SendGrid.Helpers.Mail.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IMailService
    {
         Task SendEmailAsync();
    }
     public class SendGridMailService : IMailService
    {

        public SendGridMailService()
        {
            // _configuration = configuration; 
        }

        public async Task SendEmailAsync()
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress("ontosoft5@gmail.com", "OntoSoft"));
            msg.AddTo(new EmailAddress("", "Apreciado Paciente"));
            msg.SetTemplateId("d-17616c7169ba421f966e318f4e620111 ");
            msg.SetTemplateData(new{
                    url = "{{url}}"
                    });
            var response = await client.SendEmailAsync(msg);

            // var apiKey = "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";
            // var client = new SendGridClient(apiKey);
            // var from = new EmailAddress("ontosoft5@gmail.com", "OntoSoft Recupera tu contraseña!");
            // var to = new EmailAddress(toEmail);
            // var msg = MailHelper.CreateSingleTemplateEmail(from, to,"d-17616c7169ba421f966e318f4e620111", dynamicTemplateData);
            // var response = await client.SendEmailAsync(msg);
            
        }
    }
}