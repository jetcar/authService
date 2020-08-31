namespace MobileId.Config
{
    public class MobileIdConfig
    {
        public const string Position = "MobileId";
        public string Url { get; set; }
        public string relyingPartyUUID { get; set; }
        public string relyingPartyName { get; set; }
        public string DisplayText { get; set; }
        public string HashType { get; set; }
        public int Timeout { get; set; }
    }
}