using Domain.DomainExceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Net;

namespace LockManagementAPI.ExceptionHandlers
{
    public class GlobalExceptionFilter: ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {

            HttpStatusCode statusCode = error(context.Exception.GetType());

            string errorMessage = context.Exception.Message;

            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int)statusCode;
            response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(
                new
                {
                    errorMessage = errorMessage,
                    errorCode = statusCode,
                });

            response.ContentLength = result.Length;
            response.WriteAsync(result);
        }

        private HttpStatusCode error(Type exceptionType)
        {

            if ((exceptionType.Name == typeof(AuditInvalidException).Name) ||
               (exceptionType.Name == typeof(EmailInvalidException).Name) ||
               (exceptionType.Name == typeof(LockInvalidException).Name)  ||
               (exceptionType.Name == typeof(NameInvalidException).Name)  ||
               (exceptionType.Name == typeof(PasswordInvalidException).Name) ||
               (exceptionType.Name == typeof(RoleInvalidException).Name) ||
               (exceptionType.Name == typeof(UserInvalidJwt).Name) ||
               (exceptionType.Name == typeof(UserInvalidException).Name))
                return HttpStatusCode.BadRequest;

            if((exceptionType.Name == typeof(UserNotFoundException).Name) ||
               (exceptionType.Name == typeof(LockNotFoundException).Name))
                return HttpStatusCode.NotFound;


            return HttpStatusCode.InternalServerError;

        }
    }
}
