using System.Net;
using System;
namespace Aplication.ManagerExcepcion
{
    public class ManagerError : Exception
    {
        public HttpStatusCode Codigo  {get;}
        public object Errors {get;}
        public ManagerError(HttpStatusCode codigo, object errors = null) {
            Codigo = codigo;
            Errors = errors;
        }
    }
}