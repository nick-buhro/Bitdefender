using System;
using Xunit;

namespace NickBuhro.Bitdefender.Tests
{
    public class BitdefenderClientTests
    {
        [Fact]
        public void EmptyCtorTest()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var client = new BitdefenderClient(null);
                client.Dispose();
            });
        }

        [Fact]
        public void CtorTest1()
        {
            using (var client = new BitdefenderClient(Configuration.BitdefenderApiKey))
            {
                Assert.NotNull(client.AccessUrl);
                Assert.StartsWith("http", client.AccessUrl.AbsoluteUri);                
            }
        }
        
        [Fact]
        public void CtorTest2()
        {
            const string url = "http://www.elko.by";
            using (var client = new BitdefenderClient(Configuration.BitdefenderApiKey, url))
            {
                Assert.NotNull(client.AccessUrl);
                Assert.Equal(url, client.AccessUrl.OriginalString);                
            }
        }               
    }
}
