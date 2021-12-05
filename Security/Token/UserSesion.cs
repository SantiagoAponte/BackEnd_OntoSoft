using System.Linq;
using System.Security.Claims;
using Aplication.Interfaces.Contracts;
using Microsoft.AspNetCore.Http;

namespace Security.Token
{
    public class UserSesion : IUserSesion
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserSesion(IHttpContextAccessor httpContextAccessor){
            _httpContextAccessor = httpContextAccessor;
        }
        public string ObtainUserSesion()
        {
            var userName = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type==ClaimTypes.Email)?.Value;
            return userName;
        }
    }
}