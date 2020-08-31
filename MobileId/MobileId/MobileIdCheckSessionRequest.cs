using System;

namespace MobileId.MobileId
{
    public class MobileIdCheckSessionRequest
    {
        public States state { get; set; }
        public Results? result { get; set; }
        public Signature signature { get; set; }
        public string cert { get; set; }
        public DateTime time { get; set; }
        public string traceId { get; set; }
    }
}