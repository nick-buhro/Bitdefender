using System;

namespace NickBuhro.Bitdefender.Controllers
{
    /// <summary>
    /// JSON-RPC 2.0 response error.
    /// </summary>
    [Serializable]
    public sealed class JsonRpcException : Exception
    {
        public int Code => (int)Data["Code"];

        internal JsonRpcException(JsonRpcError error)
            : base(error.ToString())
        {
            Data.Add("Code", error.Code);
            Data.Add("Message", error.Message);
            foreach (var p in error?.Data)
            {
                Data.Add(p.Key, p.Value);
            }
        }
    }
}
