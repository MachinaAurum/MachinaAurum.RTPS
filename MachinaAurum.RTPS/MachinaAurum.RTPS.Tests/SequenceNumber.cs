using System;
using System.Threading;

namespace MachinaAurum.RTPS.Tests
{
    public class SequenceNumber
    {
        int Value;

        public SequenceNumber()
        {
            Value = 0;
        }

        internal SequenceNumber(int newValue)
        {
            Value = newValue;
        }

        internal SequenceNumber Increment()
        {
            var newValue = Interlocked.Increment(ref Value);

            return new SequenceNumber(newValue);
        }
    }
}