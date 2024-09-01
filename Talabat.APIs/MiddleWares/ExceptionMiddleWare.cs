using Microsoft.VisualBasic;
using System.Net;
using System.Text.Json;
using Talabat.APIs.Errors;

namespace Talabat.APIs.MiddleWares
{
	public class ExceptionMiddleWare
	{
		private readonly RequestDelegate _next;
		private readonly ILoggerFactory _loggerFactory;
		private readonly IWebHostEnvironment _env;

		public ExceptionMiddleWare(RequestDelegate next , ILoggerFactory loggerFactory , IWebHostEnvironment env)
        {
			_next = next;
			_loggerFactory = loggerFactory;
			_env = env;
		}

		public async Task Invoke(HttpContext httpContext)
		{
			try
			{
				await _next.Invoke(httpContext);
			}
			catch (Exception ex)
			{
				// log exception 
				var logger = _loggerFactory.CreateLogger<ExceptionMiddleWare>();
				logger.LogError(ex.Message);

				// catch response --> header , body 
				//header
				httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				httpContext.Response.ContentType = "application/json";

				// body 
				var response = _env.IsDevelopment() ? new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message
					, ex.StackTrace) : new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);

				// Convert from ApiExceptionresponse typ To Json file 
				var json = JsonSerializer.Serialize(response);
				await httpContext.Response.WriteAsync(json);

			}
		}
    }
}
