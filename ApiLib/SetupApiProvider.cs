using System;
using System.Collections.Generic;

namespace MarcoZechner.ApiLib
{
    public sealed class SetupApiProvider : IApiProvider
    {
        private readonly Func<ulong, string, Dictionary<string, Delegate>, Dictionary<string, Delegate>> _connect;
        private readonly Action<ulong> _disconnect;

        public SetupApiProvider(
            Func<ulong, string, Dictionary<string, Delegate>, Dictionary<string, Delegate>> connect,
            Action<ulong> disconnect
        )
        {
            _connect = connect;
            _disconnect = disconnect;
        }

        public Dictionary<string, Delegate> ConvertToDict()
        {
            return new Dictionary<string, Delegate>
            {
                { ApiConstants.SETUP_KEY_CONNECT, new Func<ulong, string, Dictionary<string, Delegate>, Dictionary<string, Delegate>>(_connect) },
                { ApiConstants.SETUP_KEY_DISCONNECT, new Action<ulong>(_disconnect) }
            };
        }
    }
}