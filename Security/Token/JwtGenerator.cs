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
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id),
                new Claim(JwtRegisteredClaimNames.NameId, usuario.UserName),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
            };

            if(roles != null){
                foreach(var rol  in roles){
                    claims.Add(new Claim(ClaimTypes.Role, rol ));
                    claims.Add(new Claim("fullName", usuario.fullName));
                    claims.Add(new Claim("PhoneNumber", usuario.PhoneNumber));
                    claims.Add(new Claim("phoneEmergency", usuario.phoneEmergency));
                    claims.Add(new Claim("contactEmergency", usuario.contactEmergency));
                    claims.Add(new Claim("addressContact", usuario.addresContact));
                    claims.Add(new Claim("centerEmergency", usuario.centerEmergency));
                    claims.Add(new Claim("eps", usuario.eps));
                    claims.Add(new Claim("dateBirth", usuario.dateBirth.ToString()));
                    claims.Add(new Claim("city", usuario.city));
                    claims.Add(new Claim("address", usuario.address));
                    claims.Add(new Claim("gender", usuario.gender));
                    claims.Add(new Claim("document", usuario.document));
                    claims.Add(new Claim("height", usuario.height));
                    claims.Add(new Claim("weigth", usuario.weight));
                    claims.Add(new Claim("rh", usuario.rh.ToString()));
                    claims.Add(new Claim("bloodType", usuario.bloodType));
                    claims.Add(new Claim("typeDocument", usuario.typeDocumentId));
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