using System.Threading.Tasks;
using Aplication.Security;


namespace Aplication.Interfaces
{
     public interface IMailCreateAppoinment
    {
     Task<UserManagerResponse> SendEmailAppoinmentAsync(string email, string date, string time);
    }
}