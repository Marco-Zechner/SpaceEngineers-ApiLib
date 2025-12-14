using System;
using System.Collections.Generic;

namespace MarcoZechner.ApiLib
{
    public sealed class SetupApiProvider : IApiProvider
    {
        private readonly string _keyConnect;
        private readonly string _keyDisconnect;

        private readonly Func<ulong, string, Dictionary<string, Delegate>, Dictionary<string, Delegate>> _connect;
        private readonly Action<ulong> _disconnect;

        public SetupApiProvider(
            string keyConnect,
            string keyDisconnect,
            Func<ulong, string, Dictionary<string, Delegate>, Dictionary<string, Delegate>> connect,
            Action<ulong> disconnect
        )
        {
            _keyConnect = keyConnect;
            _keyDisconnect = keyDisconnect;
            _connect = connect;
            _disconnect = disconnect;
        }

        public Dictionary<string, Delegate> ConvertToDict()
        {
            return new Dictionary<string, Delegate>
            {
                { _keyConnect, new Func<ulong, string, Dictionary<string, Delegate>, Dictionary<string, Delegate>>(_connect) },
                { _keyDisconnect, new Action<ulong>(_disconnect) }
            };
        }
    }
}