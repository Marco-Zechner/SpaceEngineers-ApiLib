using System;
using System.Collections.Generic;

namespace MarcoZechner.ApiLib
{
    public sealed class SetupApi
    {
        private readonly Func<ulong, string, Dictionary<string, Delegate>, Dictionary<string, Delegate>> _connect;
        private readonly Action<ulong> _disconnect;

        public SetupApi(Dictionary<string, Delegate> dict, string keyConnect, string keyDisconnect)
        {
            _connect = (Func<ulong, string, Dictionary<string, Delegate>, Dictionary<string, Delegate>>)dict[keyConnect];
            _disconnect = (Action<ulong>)dict[keyDisconnect];
        }

        public Dictionary<string, Delegate> Connect(ulong modId, string modName, Dictionary<string, Delegate> callbacks)
        {
            return _connect(modId, modName, callbacks);
        }

        public void Disconnect(ulong modId)
        {
            _disconnect(modId);
        }
    }
}