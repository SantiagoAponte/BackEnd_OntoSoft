using System;

namespace Aplication.OdontogramApp.Dtos
{
    public class toothsOdontogramDto
    {
        public string UserId {get;set;}
        public Guid OdontogramId {get;set;}
        public Guid toothId {get;set;}
    }
}