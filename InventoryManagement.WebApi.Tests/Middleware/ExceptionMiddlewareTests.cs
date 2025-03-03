using Microsoft.AspNetCore.Http;
using Moq;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Logging;
using InventoryManagement.Middleware;

namespace InventoryManagement.WebApi.Tests.Middleware
{
    public class ExceptionMiddlewareTests
    {
        private readonly Mock<ILogger<ExceptionMiddleware>> _mockLogger;
        private readonly Mock<RequestDelegate> _mockRequestDelegate;
        private readonly ExceptionMiddleware _exceptionMiddleware;

        public ExceptionMiddlewareTests()
        {
            _mockLogger = new Mock<ILogger<ExceptionMiddleware>>();
            _mockRequestDelegate = new Mock<RequestDelegate>();
            _exceptionMiddleware = new ExceptionMiddleware(_mockRequestDelegate.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Invoke_Should_Return_InternalServerError_When_Exception_Occurs()
        {
            // Arrange
            var exceptionMessage = "This is Test exception message";
            var httpContext = new DefaultHttpContext();

            // Create a MemoryStream to capture the response body
            var memoryStream = new MemoryStream();
            httpContext.Response.Body = memoryStream;

            // Mock the RequestDelegate to throw an exception when invoked
            _mockRequestDelegate.Setup(rd => rd.Invoke(It.IsAny<HttpContext>())).ThrowsAsync(new Exception(exceptionMessage));

            // Act
            await _exceptionMiddleware.Invoke(httpContext);

            // Assert InternalServerError (500)
            Assert.Equal((int)HttpStatusCode.InternalServerError, httpContext.Response.StatusCode);

            // Assert "application/json"
            Assert.Equal("application/json", httpContext.Response.ContentType);

            memoryStream.Seek(0, SeekOrigin.Begin);

            var responseBody = new StreamReader(memoryStream).ReadToEnd();

            var errorResponse = JsonConvert.DeserializeObject<dynamic>(responseBody);

            // Assert
            Assert.Equal(exceptionMessage, errorResponse.Message.ToString());
        }
    }
}