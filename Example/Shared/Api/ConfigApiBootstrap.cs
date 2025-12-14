using MarcoZechner.ApiLib;

namespace MarcoZechner.ConfigAPI.Shared.Api
{
    public class ConfigApiBootstrap : ApiBootstrapConfig
    {
        public override string ApiLibVersion => "1.0.0";
        public override long DiscoveryChannel => 23456; // a unique channel for your mod.
        public override string ApiProviderModId => "2325234235252"; // your mods steam id here.
        public override string ApiVersion => "0.1.0";
    }
}