using Microsoft.AspNetCore.Mvc;
using Service.HelperClasses;
using System.Net;


namespace Service.ResponseImpl
{
    public class MethodResult<T>
            where T : class
    {

        public ErrorResponse Error { get; set; }
        public HttpStatusCode ErrorStatusCode { get; set; }
        public T Data { get; set; }
        public bool IsError => Error is { };

        public void SetError(string errorMessage, HttpStatusCode statusCode, object errorData = null)
        {
            Error = new ErrorResponse
            {
                ErrorMessage = errorMessage,
                ErrorData = errorData
            };

            ErrorStatusCode = statusCode;
        }

        public ObjectResult DecideWhatToReturn()
        {
            return IsError
                ? SetErrorResultToReturn(Error)
                : new OkObjectResult(Data);
        }

        private ObjectResult SetErrorResultToReturn(ErrorResponse methodResultError)
        {
            return new ObjectResult(methodResultError) { StatusCode = (int)ErrorStatusCode };
        }
    }
}