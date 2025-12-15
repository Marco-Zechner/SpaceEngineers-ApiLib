using MarcoZechner.ApiLib;

namespace MarcoZechner.ConfigAPI.Shared.Api
{
    public class ConfigApiBootstrap : ApiBootstrapConfig
    {
        // the version of the ApiLib your mod is using.
        public override string ApiLibVersion => "1.0.0";
        
        // If you are making a API then set your mod's steam id here.
        public override string ApiProviderModId => "2325234235252";
        public override string ApiVersion => "0.1.0"; // a versioning system for your API, to make sure others have the right version.
        public override long DiscoveryChannel => 23456; // a unique channel for your mod. 
    }
}