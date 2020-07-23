using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AuthServiceTests
{
    internal class TestHttpRequest : HttpRequest
    {
        private IRequestCookieCollection _cookies  = new TestCoockieCollection();

        public override Task<IFormCollection> ReadFormAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public override Stream Body { get; set; }
        public override long? ContentLength { get; set; }
        public override string ContentType { get; set; }

        public override IRequestCookieCollection Cookies
        {
            get => _cookies;
            set => _cookies = value;
        }

        public override IFormCollection Form { get; set; }
        public override bool HasFormContentType { get; }
        public override IHeaderDictionary Headers { get; }
        public override HostString Host { get; set; }
        public override HttpContext HttpContext { get; }
        public override bool IsHttps { get; set; }
        public override string Method { get; set; }
        public override PathString Path { get; set; }
        public override PathString PathBase { get; set; }
        public override string Protocol { get; set; }
        public override IQueryCollection Query { get; set; }
        public override QueryString QueryString { get; set; }
        public override string Scheme { get; set; }
    }
}