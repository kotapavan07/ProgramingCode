using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace InService.Util
{
    public class InExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            context.Response = context.Request.CreateResponse<InExceptionDetails>(HttpStatusCode.ExpectationFailed,
                new InExceptionDetails(context.Exception.GetType(), context.Exception));
        }
    }

    public class InExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            context.Result = new ResponseMessageResult(context.Request.CreateResponse<InExceptionDetails>(
                HttpStatusCode.ExpectationFailed, new InExceptionDetails(context.Exception.GetType(), context.Exception)));
        }
    }

    public class InExceptionDetails
    {
        public InExceptionDetails() { }
        public InExceptionDetails(Type exType, Exception ex)
        {
            this.ExType = exType;
            this.Ex = ex;
        }

        public Type ExType { get; set; }
        public Exception Ex { get; set; }
    }
}