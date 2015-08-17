namespace MachinaAurum.RTPS.Tests
{
    public class SubMessageHeader
    {
        public SubMessageKind SubMessageId { get; set; }
        public ushort SubMessageLength { get; set; }
        public SubMessageFlag Flags { get; set; }
    }
}