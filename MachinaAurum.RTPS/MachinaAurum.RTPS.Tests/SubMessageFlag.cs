using System;

namespace MachinaAurum.RTPS.Tests
{
    [Flags]
    public enum SubMessageFlag
    {
        HighEndian = 1,
        InlineQos = 2,
        HasData = 4,
        HasKey = 8,
    }
}