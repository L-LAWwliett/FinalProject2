using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace HMS.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                // ვუშვებთ რექვესთს სისტემაში (კონტროლერისკენ)
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                // თუ სადმე რაიმე ერორი მოხდა, ვიჭერთ აქ!
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // დეფოლტად ვამბობთ, რომ სერვერის ბრალია (500 Internal Server Error)
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            string message = "მოხდა შიდა სერვერული შეცდომა.";

            //  თუ ერორი ჩვენი ბიზნეს ლოგიკიდან მოდის (მაგალითად: "სასტუმროს წაშლა არ შეიძლება, რადგან აქვს ოთახები")
            if (exception is InvalidOperationException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest; // 400 Bad Request
                message = exception.Message;
            }
            // თუ რამე ვერ ვიპოვეთ
            else if (exception.Message.Contains("ვერ მოიძებნა"))
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound; // 404 Not Found
                message = exception.Message;
            }

            // ვაფორმირებთ ჩვენს სტანდარტულ პასუხს
            var response = new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = message
            };

            return context.Response.WriteAsync(response.ToString());
        }
    }
}