using System;
using System.Collections.Generic;
using Sandbox.ModAPI;

namespace MarcoZechner.ApiLib
{
    public sealed class ApiProviderHost
    {
        private readonly ApiBootstrapConfig _cfg;
        private readonly IApiProvider _setupProvider;
        
        private Func<string, string, ulong, bool> _verify;

        public ApiProviderHost(ApiBootstrapConfig cfg, IApiProvider setupProvider)
        {
            _cfg = cfg;
            _setupProvider = setupProvider;
        }

        public void Load()
        {
            _verify = VerifyApi;
            MyAPIGateway.Utilities.RegisterMessageHandler(_cfg.DiscoveryChannel, OnDiscoveryMessage);
            // broadcast announce once
            SendAnnounce(0UL, "Any");
        }

        public void Unload()
        {
            _verify = null;
            MyAPIGateway.Utilities.UnregisterMessageHandler(_cfg.DiscoveryChannel, OnDiscoveryMessage);
        }

        private void OnDiscoveryMessage(object obj)
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

            if (!ApiCast.TryGet(header, _cfg.HeaderIntentKey, out intent) || intent != _cfg.IntentRequest)
                return;

            if (!ApiCast.TryGet(header, _cfg.HeaderSchemaKey, out schema) || schema != _cfg.SchemaRequest)
                return;

            // respect provider target if present: if target != 0 must match *this provider*
            // since ApiLib is generic, we only enforce "target == 0 OR equals expected provider id if present".
            // The caller can include provider id in header; if you want strict checks, do them outside.
            ulong fromId;
            string fromName;
            ApiCast.TryGet(header, _cfg.HeaderFromModIdKey, out fromId);
            ApiCast.TryGet(header, _cfg.HeaderFromModNameKey, out fromName);

            SendAnnounce(fromId, fromName ?? "Unknown");
        }

        private void SendAnnounce(ulong targetModId, string targetModName)
        {
            var header = new Dictionary<string, object>
            {
                { _cfg.HeaderMagicKey, _cfg.Magic },
                { _cfg.HeaderProtocolKey, _cfg.Protocol },
                { _cfg.HeaderSchemaKey, _cfg.SchemaAnnounce },
                { _cfg.HeaderIntentKey, _cfg.IntentAnnounce },
                { _cfg.HeaderApiVersionKey, _cfg.ApiVersion },

                { _cfg.HeaderTargetModIdKey, targetModId },
                { _cfg.HeaderTargetModNameKey, targetModName ?? "Any" },

                { _cfg.HeaderLayoutKey, "Header, Verify, Data" },
                { _cfg.HeaderTypesKey, "Dict<string,object>, Func<string,string,ulong,bool>, Dict<string,Delegate>" }
            };

            ModMessage.Send(_cfg.DiscoveryChannel, header, _verify, _setupProvider);
        }
        
        private bool VerifyApi(string clientApiVersion, string clientModName, ulong clientModSteamId)
        {
            var client = (clientApiVersion ?? "").Split('.');
            var provider = _cfg.ApiVersion.Split('.');

            if (client.Length != 3 || provider.Length != 3)
                return false;

            return client[0] == provider[0];
        }
    }
}
