namespace MachinaAurum.RTPS.Tests
{
    internal class SerializedPaylodSubMessageElement : SubMessageElement
    {
        public byte[] Octet { get; private set; }

        public SerializedPaylodSubMessageElement(Data data)
        {
            Octet = data.Octet;
        }
    }
}