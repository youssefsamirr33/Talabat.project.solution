
namespace Talabat.APIs.Errors
{
	public class ApiResponse
	{
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiResponse(int statusCode , string? message = null)
        {
			StatusCode = statusCode ;
            Message = message ?? GetMessageWithStatusCode(statusCode);

		}

		private string? GetMessageWithStatusCode(int statusCode)
		{
			return statusCode switch
			{
				400 => "Bad Request ,you have made",
				401 => "Authorized , you are not",
				404 => "Resource are not Found",
				500 => "Error are the path to the dark side , Error lead to anger . Anger leads to hate. Hate leads to career change",
				_ => null
			};
		}
	}
}
