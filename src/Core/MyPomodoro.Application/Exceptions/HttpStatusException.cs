using System;
using System.Collections.Generic;
using System.Net;

namespace MyPomodoro.Application.Exceptions
{
    public class HttpStatusException : Exception
    {
        public HttpStatusCode statusCode { get; private set; }
        public List<string> msgs { get; private set; }
        public HttpStatusException(List<string> msgs, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(String.Join("\n", msgs))
        {
            this.statusCode = statusCode;
            this.msgs = msgs;
        }
    }
}