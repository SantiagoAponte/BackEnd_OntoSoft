using System.Collections.Generic;
using Domine;

namespace Aplication.Interfaces.Contracts
{
    public interface IJwtGenerator
    {
         string CreateToken(User user, List<string> roles);
    }
}