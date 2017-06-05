using System;
using System.Diagnostics;

namespace NickBuhro.Bitdefender.Tests
{
    public static class Configuration
    {
        private static readonly object _lock = new object();

        private static string _bitdefenderApiKey;
        private static bool _bitdefenderApiKeyAssigned;
        
        public static string BitdefenderApiKey
        {
            get
            {
                if (!_bitdefenderApiKeyAssigned)
                {
                    lock (_lock)
                    {
                        if (!_bitdefenderApiKeyAssigned)
                        {
                            _bitdefenderApiKey = Environment.GetEnvironmentVariable("BITDEFENDER_API_KEY");
                            _bitdefenderApiKeyAssigned = true;
                        }
                    }
                }

                Debug.Assert(_bitdefenderApiKeyAssigned);

                if (string.IsNullOrEmpty(_bitdefenderApiKey))
                    throw new Exception("Configure environment variable 'BITDEFENDER_API_KEY' to run tests.");

                return _bitdefenderApiKey;
            }
        }
    }
}
