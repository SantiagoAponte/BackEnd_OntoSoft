using System.Threading.Tasks;
using Aplication.Security;

namespace Aplication.Interfaces.Contracts
{
    public interface IMailEditAppoinment
    {
         Task<UserManagerResponse> SendEmailAppoinmentAsync(string email, string date, string time);
    }
}