using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Huntr.SecondFactorAuth.Api.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogTrace($"Request Starting - {DateTime.UtcNow:dd-MM-yyyyThh:mm:ss.ffffffZ} - Request URL: {Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(context.Request)}");
            await _next(context);
            _logger.LogTrace($"Request End - {DateTime.UtcNow:dd-MM-yyyyThh:mm:ss.ffffffZ} - Request URL: {Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(context.Request)}");
        }
    }
}
