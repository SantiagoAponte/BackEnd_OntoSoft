using System;
using System.Net;
using System.Threading.Tasks;
using Aplication.ManagerExcepcion;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WebApi.Middleware
{
    public class ManagerErrorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ManagerErrorMiddleware> _logger;
        public ManagerErrorMiddleware(RequestDelegate next, ILogger<ManagerErrorMiddleware> logger) {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context){
            try{
                await _next(context);
            } catch (Exception ex){
                await ManagerErrorAsync(context, ex, _logger);
            }
        }

        private async Task ManagerErrorAsync(HttpContext context, Exception ex, ILogger<ManagerErrorMiddleware> logger){
            object errores = null;
            switch(ex){
                case ManagerError me :
                        logger.LogError(ex, "Manejador de errores en capa aplicación");
                        errores = me.Errors;
                        context.Response.StatusCode = (int)me.Codigo;
                        break;
                case Exception e:
                        logger.LogError(ex, "Error en el servidor, estalló");
                        errores = string.IsNullOrWhiteSpace(e.Message) ? "Error" : e.Message;
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
            }
            context.Response.ContentType = "application/json";
            if(errores!=null){
                var result = JsonConvert.SerializeObject(new {errores});
                await context.Response.WriteAsync(result);
            }

        }
    }
}