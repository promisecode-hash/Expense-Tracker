using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace ExpenseTracker.Api.Middlewares
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Unhandled exception occurred");

            var response = new
            {
                message = "An error occurred while processing your request",
                detail = context.Exception.Message
            };

            context.Result = new Microsoft.AspNetCore.Mvc.ObjectResult(response)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };

            context.ExceptionHandled = true;
        }
    }
}
