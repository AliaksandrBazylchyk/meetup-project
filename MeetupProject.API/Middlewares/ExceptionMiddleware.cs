using MeetupProject.API.Extensions;
using MeetupProject.Common.Exceptions;

namespace MeetupProject.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext">All http context from server</param>
        /// <returns>if request pass without exceptions continue work with him(request). Any other way cathing exception and handle it.</returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);

            }
        }
        /// <summary>
        /// Exception handler with automatic Status Code respone detection
        /// </summary>
        /// <param name="context">Which request caused the exception</param>
        /// <param name="exception">The exception that happened</param>
        /// <returns>Error Details object with status code and exception information</returns>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var result = new ErrorDetails
            {
                StatusCode = 500,
                Title = exception.Message
            };

            switch (exception)
            {
                case NotFoundException _:
                    result.StatusCode = 404;
                    break;
            }

            context.Response.StatusCode = result.StatusCode;

            await context.Response.WriteAsync(result.ToString());
        }
    }
}
