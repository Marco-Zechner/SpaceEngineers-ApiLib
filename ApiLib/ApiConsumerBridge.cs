using System;
using System.Collections.Generic;
using Sandbox.ModAPI;

namespace MarcoZechner.ApiLib
{
    public sealed class ApiConsumerBridge
    {
        private readonly ApiBootstrapConfig _cfg;
        private readonly ulong _consumerModId;
        private readonly string _consumerModName;
        private readonly Func<Dictionary<string, Delegate>> _buildCallbackDict;

        private Func<string, string, ulong, bool> _verify;
        private SetupApi _setupApi;

        public bool ApiLoaded { get; private set; }
        public Dictionary<string, Delegate> BoundMainDict { get; private set; }

        public ApiConsumerBridge(
            ApiBootstrapConfig cfg,
            ulong consumerModId,
            string consumerModName,
            Func<Dictionary<string, Delegate>> buildCallbackDict
        )
        {
            _cfg = cfg;
            _consumerModId = consumerModId;
            _consumerModName = consumerModName;
            _buildCallbackDict = buildCallbackDict;
        }

        public void Init()
        {
            MyAPIGateway.Utilities.RegisterMessageHandler(_cfg.DiscoveryChannel, OnProviderMessage);
            SendRequest();
        }

        public void Unload()
        {
            ApiLoaded = false;
            BoundMainDict = null;

            if (_setupApi != null)
                _setupApi.Disconnect(_consumerModId);

            _setupApi = null;

            MyAPIGateway.Utilities.UnregisterMessageHandler(_cfg.DiscoveryChannel, OnProviderMessage);
        }

        private void SendRequest()
        {
            var header = new Dictionary<string, object>
            {
                { _cfg.HeaderMagicKey, _cfg.Magic },
                { _cfg.HeaderProtocolKey, _cfg.Protocol },
                { _cfg.HeaderSchemaKey, _cfg.SchemaRequest },
                { _cfg.HeaderIntentKey, _cfg.IntentRequest },
                { _cfg.HeaderApiVersionKey, _cfg.ApiVersion },

                { _cfg.HeaderFromModIdKey, _consumerModId },
                { _cfg.HeaderFromModNameKey, _consumerModName },

                { _cfg.HeaderTargetModIdKey, 0UL }, // let provider decide; caller may override externally if desired
                { _cfg.HeaderTargetModNameKey, "Any" },

                { _cfg.HeaderLayoutKey, "Header, Verify, Data" },
                { _cfg.HeaderTypesKey, "Dict<string,object>, null, null" }
            };

            object[] payload = { header, null, null };
            MyAPIGateway.Utilities.SendModMessage(_cfg.DiscoveryChannel, payload);
        }

        private void OnProviderMessage(object obj)
        {
            object[] payload;
            if (!ApiCast.Try(obj, out payload) || payload.Length != 3)
                return;

            Dictionary<string, object> header;
            if (!ApiCast.Try(payload[0], out header))
                return;

            string magic;
            int protocol;
            string intent;
            string schema;

            if (!ApiCast.TryGet(header, _cfg.HeaderMagicKey, out magic) || magic != _cfg.Magic)
                return;

            if (!ApiCast.TryGet(header, _cfg.HeaderProtocolKey, out protocol) || protocol != _cfg.Protocol)
                return;

            if (!ApiCast.TryGet(header, _cfg.HeaderIntentKey, out intent) || intent != _cfg.IntentAnnounce)
                return;

            if (!ApiCast.TryGet(header, _cfg.HeaderSchemaKey, out schema) || schema != _cfg.SchemaAnnounce)
                return;

            // Target id: 0 means broadcast/any; otherwise must match us
            ulong targetId;
            if (ApiCast.TryGet(header, _cfg.HeaderTargetModIdKey, out targetId))
            {
                if (targetId != 0UL && targetId != _consumerModId)
                    return;
            }

            Dictionary<string, Delegate> setupDict;
            if (!ApiCast.Try(payload[2], out setupDict))
                return;

            if (!ApiCast.Try(payload[1], out _verify))
                return;

            if (_verify != null && !_verify(_cfg.ApiVersion, _consumerModName, _consumerModId))
                return;

            // Connect
            _setupApi = new SetupApi(setupDict, _cfg.SetupKeyConnect, _cfg.SetupKeyDisconnect);

            var callbackDict = _buildCallbackDict() ?? new Dictionary<string, Delegate>();

            BoundMainDict = _setupApi.Connect(_consumerModId, _consumerModName, callbackDict);
            ApiLoaded = BoundMainDict != null;
        }
    }
}
