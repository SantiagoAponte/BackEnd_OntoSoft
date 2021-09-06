using System.Linq;
using System.Threading.Tasks;
using Domine;
using Microsoft.AspNetCore.Identity;
using persistence;

namespace Persistence
{
    public class DataPrueba
    {
        public static async Task InsertData(OntoSoftContext context, UserManager<User> userManager){
            if(!userManager.Users.Any()){
                var user = new User{UserName="santiago", Email="santiago@gmail.com"};
                await userManager.CreateAsync(user, "Password123$");
            }
        }
    }
}