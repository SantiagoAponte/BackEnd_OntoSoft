using Aplication.Interfaces.Contracts;
using Aplication.ManagerExcepcion;
using Aplication.Security;
using Domine;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplication.Interfaces.Contracts


{
    public interface IForgetPassword
    {
        Task<UserManagerResponse> ForgetPasswordAsync(string email);
    }
}