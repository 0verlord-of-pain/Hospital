using System;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Hospital.Infrastructure.Helpers
{
    internal static class ErrorHelper
    {
        public static ErrorResponse CreateErrorResponse(Exception exception, bool includeStackTrace = false)
        {
            if (exception == null) return null;

            ErrorResponse errorResponse;

            switch (exception)
            {
                case ValidationException ex:
                    errorResponse = new ErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                    break;

                case ArgumentNullException ex:
                    errorResponse = new ErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                    break;

                case ArgumentException ex:
                    errorResponse = new ErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                    break;

                default:
                    return null;
            }

            if (includeStackTrace) errorResponse.SetStackTrace(exception.StackTrace);

            return errorResponse;
        }
    }
}