using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplication.ManagerExcepcion;
using Domine;
using FluentValidation;
using MediatR;
using persistence;

namespace Aplication.AppoinmentsApp
{
    public class PostAppoinment
    {
         public class Execute : IRequest {
        public Guid? Id {get;set;}
        public DateTime date {get;set;}
        public string Title {get;set;}
        public string Text {get;set;}
        public List<string> ListUsers {get;set;}
        }

        public class ExecuteValidator : AbstractValidator<Execute>{
            public ExecuteValidator(){
                RuleFor( x => x.Title).NotNull().WithMessage("El campo Title no debe ser nulo").NotEmpty().WithMessage("El campo Title no debe estar vacio");
                RuleFor( x => x.Text).NotNull().WithMessage("El campo Text no debe ser nulo").NotEmpty().WithMessage("El campo Text no debe estar vacio");
                RuleFor( x => x.date).NotNull().WithMessage("El campo date no debe ser nulo").NotEmpty().WithMessage("El campo date no debe estar vacio");
            }
        }

        public class Manager : IRequestHandler<Execute>
        {
            private readonly OntoSoftContext _context;
            public Manager(OntoSoftContext context){
                _context = context;
            }

            public async Task<Unit> Handle(Execute request, CancellationToken cancellationToken)
            {
               
               Guid appoinmentId = Guid.NewGuid();
               if(request.Id != null){
                 appoinmentId = request.Id ?? Guid.NewGuid();
               }

               var appoinment = new Appoinments {
                   Id = appoinmentId,
                   date = request.date.AddHours(-5),
                   Title = request.Title,
                   Text = request.Text
               };

                _context.Appoinments.Add(appoinment);

                if(request.ListUsers!=null){
                    foreach(var _id in request.ListUsers){
                        var userAppoinments = new UserAppoinments{
                            UserId = _id,
                            AppoinmentsId = appoinmentId
                        };
                        _context.UserAppoinments.Add(userAppoinments);
                    }
                }
                         var valor = await _context.SaveChangesAsync();
                        if(valor>0){
                        return Unit.Value;
                        }
                     throw new ManagerError(HttpStatusCode.BadRequest, new {mensaje = "No se pudo crear la cita"});
            }
        }
    }
}