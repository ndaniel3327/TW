using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;
using System;

namespace TW.Application.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                //await HandleExceptionAsync(context, ex);
            }
        }
        //public Task HandleExceptionAsync(HttpContext context, Exception ex)
        //{
        //    //context.Response.ContentType = "application/json";
        //    //HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
        //    //var errorDetails = new ErrorDetails
        //    //{
        //    //    ErrorType = "Failure",
        //    //    ErrorMessage = ex.Message,
        //    //};
        //    //switch (ex)
        //    //{
        //    //    case (NotFoundException notFoundException):
        //    //        statusCode = HttpStatusCode.NotFound;
        //    //        errorDetails.ErrorType = "Not Found";
        //    //        break;
        //    //    case (CommandValidationException commandValidationException):
        //    //        statusCode = HttpStatusCode.BadRequest;
        //    //        errorDetails.ErrorType = "Bad Request ! Incorrect values submited.";
        //    //        errorDetails.ErrorMessage = string.Join(" ", commandValidationException.ErrorsList);
        //    //        break;
        //    //    default:
        //    //        break;
        //    //}
        //    //string response = JsonConvert.SerializeObject(errorDetails);
        //    //context.Response.StatusCode = (int)statusCode;
        //    //return context.Response.WriteAsync(response);
        //}
    }
}
