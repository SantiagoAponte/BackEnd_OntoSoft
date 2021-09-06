using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Aplication.Interfaces.Contracts;
using Domine;
using Microsoft.IdentityModel.Tokens;

namespace Security.Token
{
    public class JwtGenerator : IJwtGenerator
    {
        public string CreateToken(User usuario, List<string> roles)
        {
            var claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.NameId, usuario.UserName)
            };

            if(roles != null){
                foreach(var rol  in roles){
                    claims.Add(new Claim(ClaimTypes.Role, rol ));
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Clave ultra secreta OntoSoft"));
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescripcion = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(30),
                SigningCredentials = credenciales
            };

            var tokenManejador = new JwtSecurityTokenHandler();
            var token = tokenManejador.CreateToken(tokenDescripcion);

            return tokenManejador.WriteToken(token);
        }
    }
}