using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplication.ManagerExcepcion;
using Aplication.Security.Users.Dtos;
using AutoMapper;
using Domine;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using persistence;

namespace Aplication.Security.Users
{
    public class getUserDetails
    {
       public class OneDetailUser : IRequest<UserData>{
            public string Id {get;set;}
        }

        public class Manager : IRequestHandler<OneDetailUser, UserData>
        {
            private readonly OntoSoftContext _context;
            private readonly IMapper _mapper;
            private readonly UserManager<User> _userManager;
            public Manager(UserManager<User> userManager, OntoSoftContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
                _userManager = userManager;
            }

            public async Task<UserData> Handle(OneDetailUser request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByIdAsync(request.Id);
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