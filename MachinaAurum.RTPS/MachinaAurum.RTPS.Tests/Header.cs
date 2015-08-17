namespace MachinaAurum.RTPS.Tests
{
    public class Header
    {
        public ProtocolId Protocol { get; set; }
        public ProtocolVersion Version { get; set; }
        public VendorId Vendor { get; set; }
        public GuidPrefix GuidPrefix { get; set; }
    }
}