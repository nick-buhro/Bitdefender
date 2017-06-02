using System;
using System.Threading;

namespace NickBuhro.Bitdefender.Controllers
{
    internal sealed class IdGenerator
    {
        private readonly string _idSuffix;
        private int _idValue;

        public IdGenerator()
        {
            _idValue = 1;
            _idSuffix = Guid.NewGuid()
                .ToString("D")
                .Substring(8);
        }

        public string Next()
        {
            var value = Interlocked.Increment(ref _idValue);
            return value.ToString("X8") + _idSuffix;
        }
    }
}
