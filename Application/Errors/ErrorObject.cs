using System;
using System.Net;

namespace Application.Errors
{
    public class ErrorObject
    {
        public ErrorObject(HttpStatusCode code, string message, string description)
        {
            Code = code;
            Message = message;
            Description = description;

        }
        public HttpStatusCode Code { get; }
        public string Message { get; }
        public string Description { get; }
    }
}