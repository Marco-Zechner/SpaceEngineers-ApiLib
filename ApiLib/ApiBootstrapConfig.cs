using System;

namespace MarcoZechner.ApiLib
{
    public sealed class ApiBootstrapConfig
    {
        public long DiscoveryChannel;

        public string Magic;
        public int Protocol;

        public string HeaderMagicKey = "Magic";
        public string HeaderProtocolKey = "Protocol";
        public string HeaderSchemaKey = "Schema";
        public string HeaderIntentKey = "Intent";
        public string HeaderApiVersionKey = "ApiVersion";

        public string HeaderFromModIdKey = "FromModId";
        public string HeaderFromModNameKey = "FromModName";

        public string HeaderTargetModIdKey = "TargetModId";
        public string HeaderTargetModNameKey = "TargetModName";

        public string HeaderLayoutKey = "Layout";
        public string HeaderTypesKey = "Types";

        public string IntentRequest;
        public string IntentAnnounce;

        public string SchemaRequest;
        public string SchemaAnnounce;

        public string ApiVersion;

        // setup API convention keys
        public string SetupKeyConnect = "Connect";
        public string SetupKeyDisconnect = "Disconnect";
    }
}