using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domine;
using FluentValidation;
using MediatR;
using persistence;

namespace Aplication.AppoinmentsApp
{
    public class PostAppoinment
    {
         public class Execute : IRequest {
        public Guid? id {get;set;}
        public DateTime start {get;set;}
        public string title {get;set;}
        public string text {get;set;}
        public List<string> ListUsers {get;set;}
        }

        public class ExecuteValidator : AbstractValidator<Execute>{
            public ExecuteValidator(){
                RuleFor( x => x.title).NotEmpty().WithMessage("El campo no debe estar vacio");
                RuleFor( x => x.text).NotEmpty().WithMessage("El campo no debe estar vacio");
                RuleFor( x => x.start).NotEmpty().WithMessage("El campo no debe estar vacio");
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
               if(request.id != null){
                 appoinmentId = request.id ?? Guid.NewGuid();
               }

               var appoinment = new Appoinments {
                   id = appoinmentId.ToString(),
                   start = request.start,
                   title = request.title,
                   text = request.text
               };

                _context.Appoinments.Add(appoinment);

                if(request.ListUsers!=null){
                    foreach(var _id in request.ListUsers){
                        var userAppoinments = new UserAppoinments{
                            UserId = _id,
                            Appoinmentsid = appoinmentId.ToString()
                        };
                        _context.UserAppoinments.Add(userAppoinments);
                    }
                }
                         var valor = await _context.SaveChangesAsync();
                        if(valor>0){
                        return Unit.Value;
                        }
                     throw new Exception("No se pudo crear la cita");
            }
        }
    }
}