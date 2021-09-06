using Microsoft.AspNetCore.Authorization;

namespace Aplication.Policies
{
   public class UserNameRequirement : IAuthorizationRequirement
{
    public UserNameRequirement(string username)
    {
        UserName = username;
    }

    public string UserName { get; }
}
}