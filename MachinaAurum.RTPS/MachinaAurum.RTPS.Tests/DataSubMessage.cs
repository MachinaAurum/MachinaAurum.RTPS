namespace MachinaAurum.RTPS.Tests
{
    internal class DataSubMessage : SubMessage
    {
        public short OctetsToInlineQos { get; set; }
        public EntityId ReaderId { get; set; }
        public EntityId WriterId { get; set; }
        public SequenceNumber WriterSequenceNumber { get; set; }
        public ParameterList InlineQos { get; set; }

        public DataSubMessage(Data data)
        {
            Header = new SubMessageHeader()
            {
                SubMessageId = SubMessageKind.Data,
                Flags = SubMessageFlag.HighEndian,
                SubMessageLength = 4
            };

            AddSubMessageElement(new SerializedPaylodSubMessageElement(data));
        }        
    }
}