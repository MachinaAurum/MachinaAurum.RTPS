namespace MachinaAurum.RTPS.Tests
{
    public enum SubMessageKind
    {
        Data,
        DataFragment,
        Gap,
        Heartbeat,
        HeartbeatFragment,
        AckNack,
        NackFragment,
        Pad,
        InfoTs,
        InfoReply,
        InfoDst,
        InfoSource,
    }
}