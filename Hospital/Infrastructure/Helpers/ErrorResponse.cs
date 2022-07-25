using System.Net;
using Newtonsoft.Json;

namespace Hospital.Infrastructure.Helpers
{
    internal sealed class ErrorResponse
    {
        public ErrorResponse(
            HttpStatusCode statusCode,
            string message,
            string details = null)
        {
            Message = message;
            StatusCode = statusCode;
            Details = details;
        }

        public HttpStatusCode StatusCode { get; }
        public string Message { get; }
        public string Details { get; }
        public string StackTrace { get; private set; }

        public void SetStackTrace(string stackTrace)
        {
            StackTrace = stackTrace;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}