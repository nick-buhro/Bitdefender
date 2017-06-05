using System;

namespace NickBuhro.Bitdefender.Tests
{
    public sealed class BitdefenderClientFixture : IDisposable
    {   
        public BitdefenderClient Client { get; }

        public BitdefenderClientFixture()
        {
            Client = new BitdefenderClient(Configuration.BitdefenderApiKey);
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }
}
