using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplication.Interfaces.Contracts;
using Aplication.ManagerExcepcion;
using Aplication.Security.Users.Dtos;
using Domine;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using persistence;

namespace Aplication.Security
{
    public class UserActually
    {
       public class Execute : IRequest<UserData> { }

        public class Manager : IRequestHandler<Execute, UserData>
        {
            private readonly UserManager<User> _userManager;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IUserSesion _userSesion;

            private readonly OntoSoftContext _context;
            public Manager(UserManager<User> userManager, IJwtGenerator jwtGenerator, IUserSesion usuarioSesion, OntoSoftContext context)
            {
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
                _userSesion = usuarioSesion;
                _context = context;
            }
            public async Task<UserData> Handle(Execute request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(_userSesion.ObtainUserSesion());
                
                var resultRoles = await _userManager.GetRolesAsync(user);
                var listRoles = new List<string>(resultRoles);
                
                
                var imagenPerfil = await _context.Galleries.Where(x => x.ObjectReference == new System.Guid(user.Id)).FirstOrDefaultAsync();
                if (imagenPerfil != null)
                {
                    var imagenUser = new ImagenPerfil
                    {
                        Data = Convert.ToBase64String(imagenPerfil.Contain),
                        Extension = imagenPerfil.Extension,
                        Name = imagenPerfil.Name
                    };

                    return new UserData
                    {
                        fullName = user.fullName,
                        Username = user.UserName,
                        // Token = _jwtGenerator.CreateToken(user, listRoles),
                        Email = user.Email,
                        imagenPerfil = imagenUser,
                        PhoneNumber = user.PhoneNumber,
                        phoneEmergency = user.phoneEmergency,
                        contactEmergency = user.contactEmergency,
                        addresContact = user.addresContact,
                        centerEmergency = user.centerEmergency,
                        eps = user.eps,
                        dateBirth = user.dateBirth,
                        city = user.city,
                        address = user.address,
                        gender = user.gender,
                        document = user.document,
                        height = user.height,
                        weight = user.weight,
                        rh = user.rh,
                        bloodType = user.bloodType,
                        typeDocumentId = user.typeDocumentId
                    };
                }
                else
                {
                    return new UserData
                    {
                        fullName = user.fullName,
                        Username = user.UserName,
                        // Token = _jwtGenerator.CreateToken(user, listRoles),
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        phoneEmergency = user.phoneEmergency,
                        contactEmergency = user.contactEmergency,
                        addresContact = user.addresContact,
                        centerEmergency = user.centerEmergency,
                        eps = user.eps,
                        dateBirth = user.dateBirth,
                        city = user.city,
                        address = user.address,
                        gender = user.gender,
                        document = user.document,
                        height = user.height,
                        weight = user.weight,
                        rh = user.rh,
                        bloodType = user.bloodType,
                        typeDocumentId = user.typeDocumentId
                    };
                }
            }
        }
    }
}