using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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
                new Claim(JwtRegisteredClaimNames.Name, usuario.fullName),
            };

            if(roles != null){
                foreach(var rol  in roles){
                    claims.Add(new Claim(ClaimTypes.Role, rol ));
                     claims.Add(new Claim("clinicHistoryId", usuario.clinicHistory.Id.ToString()));
                    // claims.Add(new Claim("appoinmentId", usuario.appoinmentsLink.ToList()[0].AppoinmentsId.ToString())); //tal vez no vaya, crece en el tiempo
                    // claims.Add(new Claim("odontogramId", usuario.odontogram.ToList()[0].Id.ToString())); //tal vez no vaya, crece en el tiempo, pero se puede dejar uno solo por user.
                    // claims.Add(new Claim("fullName", usuario.fullName));
                    // claims.Add(new Claim("PhoneNumber", usuario.PhoneNumber));
                    // claims.Add(new Claim("phoneEmergency", usuario.phoneEmergency));
                    // claims.Add(new Claim("contactEmergency", usuario.contactEmergency));
                    // claims.Add(new Claim("addressContact", usuario.addresContact));
                    // claims.Add(new Claim("centerEmergency", usuario.centerEmergency));
                    // claims.Add(new Claim("eps", usuario.eps));
                    // claims.Add(new Claim("dateBirth", usuario.dateBirth.ToString()));
                    // claims.Add(new Claim("city", usuario.city));
                    // claims.Add(new Claim("address", usuario.address));
                    // claims.Add(new Claim("gender", usuario.gender));
                    // claims.Add(new Claim("document", usuario.document));
                    // claims.Add(new Claim("height", usuario.height));
                    // claims.Add(new Claim("weigth", usuario.weight));
                    // claims.Add(new Claim("rh", usuario.rh.ToString()));
                    // claims.Add(new Claim("bloodType", usuario.bloodType));
                    // claims.Add(new Claim("typeDocument", usuario.typeDocumentId));
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Clave ultra secreta OntoSoft"));
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescripcion = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(claims),
                Expires = System.DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = credenciales
            };

            var tokenManejador = new JwtSecurityTokenHandler();
            var token = tokenManejador.CreateToken(tokenDescripcion);

            return tokenManejador.WriteToken(token);
        }
    }
}