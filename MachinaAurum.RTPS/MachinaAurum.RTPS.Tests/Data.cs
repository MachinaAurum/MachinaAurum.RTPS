using System;

namespace MachinaAurum.RTPS.Tests
{
    internal class Data
    {
        public byte[] Octet { get; protected set; }
    }

    internal class IntData : Data
    {
        public IntData(int i)
        {
            Octet = BitConverter.GetBytes(i);
        }
    }
}