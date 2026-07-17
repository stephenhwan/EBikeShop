using System.Net;
using System.Text.Json;

using PollBuilder.Application.Exceptions;

namespace PollBuilder.API
{
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate _next;

		public ExceptionHandlingMiddleware(RequestDelegate next)
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
				var (statusCode, message) = ex switch
				{
					ValidationException fvEx => (
						HttpStatusCode.BadRequest,
						string.Join(" | ", fvEx.Errors.SelectMany(e => e.Value))),

					NotFoundException => (HttpStatusCode.NotFound, ex.Message),

					AlreadyVotedException => (HttpStatusCode.BadRequest, ex.Message),

					UnauthorizedAccessException => (HttpStatusCode.Unauthorized, ex.Message),

					_ => (HttpStatusCode.InternalServerError, "Đã có lỗi xảy ra ở server.")
				};

				context.Response.ContentType = "application/json";
				context.Response.StatusCode = (int)statusCode;

				var payload = JsonSerializer.Serialize(new { error = message });
				await context.Response.WriteAsync(payload);
			}
		}
	}
}
