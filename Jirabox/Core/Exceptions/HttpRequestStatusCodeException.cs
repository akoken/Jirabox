using System;
using System.Net;

namespace Jirabox.Core.ExceptionExtension
{
    public class HttpRequestStatusCodeException : Exception
    {
         public HttpStatusCode StatusCode { get; set; }

         public HttpRequestStatusCodeException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
    }
}
