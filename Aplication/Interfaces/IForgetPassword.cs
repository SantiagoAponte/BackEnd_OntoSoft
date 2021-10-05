using Aplication.Security;
using System.Threading.Tasks;

namespace Aplication.Interfaces.Contracts


{
    public interface IForgetPassword
    {
        Task<UserManagerResponse> ForgetPasswordAsync(string email);
    }
}