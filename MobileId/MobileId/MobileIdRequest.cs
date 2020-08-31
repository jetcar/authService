using System;

namespace MobileId.MobileId
{
    public class MobileIdRequest
    {
        public string relyingPartyName { get; set; }
        public string relyingPartyUUID { get; set; }
        public string phoneNumber { get; set; }
        public string nationalIdentityNumber { get; set; }
        public string hash { get; set; }
        public string hashType { get; set; }
        public string language { get; set; }
        public string displayText { get; set; }
        public string DisplayTextFormat { get; set; }
        public string Error { get; set; }
        public DateTime Time { get; set; }
        public string TraceId { get; set; }
    }
}