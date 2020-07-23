using System;
using System.IO;
using System.Threading.Tasks;
using AuthServiceTests.Controllers;
using Microsoft.AspNetCore.Http;

namespace AuthServiceTests
{
    public class TestResponse : HttpResponse
    {
        private TestCookies _cookies = new TestCookies();
        public override void OnCompleted(Func<object, Task> callback, object state)
        {
            throw new NotImplementedException();
        }

        public override void OnStarting(Func<object, Task> callback, object state)
        {
            throw new NotImplementedException();
        }

        public override void Redirect(string location, bool permanent)
        {
            throw new NotImplementedException();
        }

        public override Stream Body { get; set; }
        public override long? ContentLength { get; set; }
        public override string ContentType { get; set; }
        public override IResponseCookies Cookies
        {
            get { return _cookies; }
        }
        public override bool HasStarted { get; }
        public override IHeaderDictionary Headers { get; }
        public override HttpContext HttpContext { get; }
        public override int StatusCode { get; set; }
    }
}