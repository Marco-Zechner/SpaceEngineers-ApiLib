using MarcoZechner.ApiLib;

namespace MarcoZechner.ConfigAPI.Shared.Api
{
    public class ConfigApiBootstrap : ApiBootstrapConfig
    {
        public override long DiscoveryChannel => 23456;
        public override string ApiProviderModId => "MarcoZechner.APIExampleMod";
        public override string ApiVersion => "0.1.0";
    }
}