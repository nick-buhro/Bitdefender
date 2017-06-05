using Xunit;

namespace NickBuhro.Bitdefender.Tests
{
    [CollectionDefinition(nameof(BitdefenderClientCollection))]
    public sealed class BitdefenderClientCollection: ICollectionFixture<BitdefenderClientFixture>
    {

    }
}
