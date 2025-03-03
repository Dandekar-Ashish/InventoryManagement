using Newtonsoft.Json;
using System.Net;

namespace InventoryManagement.Middleware
{
    public class ExceptionMiddleware
    {
        public RequestDelegate requestDelegate;
        private readonly ILogger _logger;

        public ExceptionMiddleware(RequestDelegate requestDelegate, ILogger<ExceptionMiddleware> logger)
        {
            this.requestDelegate = requestDelegate;
            this._logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await requestDelegate(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }
        private Task HandleException(HttpContext context, Exception ex)
        {
            var errorMessage = JsonConvert.SerializeObject(new { Message = ex.Message, InnerException = ex.InnerException, StackTrace = ex.StackTrace });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            _logger.LogError(errorMessage);
            return context.Response.WriteAsync(errorMessage);
        }
    }
}
