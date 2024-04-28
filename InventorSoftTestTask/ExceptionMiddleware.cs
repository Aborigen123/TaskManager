using System;
using System.Net;

namespace InventorSoftTestTask
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
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                httpContext.Response.ContentType = "text/plain; charset=utf-8";
                httpContext.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                httpContext.Response.WriteAsync(ex.Message);
            }
        }
    }
}
