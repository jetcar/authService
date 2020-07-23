using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace AuthServiceTests
{
    public class TestHttpContext : HttpContext
    {
        TestResponse _response = new TestResponse();
        private readonly HttpRequest _request = new TestHttpRequest();
        private IDictionary<object, object> _items = new Dictionary<object, object>();

        public override void Abort()
        {
            throw new NotImplementedException();
        }

        public override ConnectionInfo Connection { get; }
        public override IFeatureCollection Features { get; }

        public override IDictionary<object, object> Items
        {
            get => _items;
            set => _items = value;
        }

        public override HttpRequest Request => _request;

        public override CancellationToken RequestAborted { get; set; }
        public override IServiceProvider RequestServices { get; set; }
        public override HttpResponse Response
        {
            get { return _response; }
        }
        public override ISession Session { get; set; }
        public override string TraceIdentifier { get; set; }
        public override ClaimsPrincipal User { get; set; }
        public override WebSocketManager WebSockets { get; }
    }
}