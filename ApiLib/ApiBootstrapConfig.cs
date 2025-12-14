using System;

namespace MarcoZechner.ApiLib
{
    public sealed class ApiBootstrapConfig
    {
        public readonly long DiscoveryChannel;
        public readonly string ApiProviderModId;
        public readonly string ApiVersion;

        public ApiBootstrapConfig(long discoveryChannel, string apiProviderModId, string apiVersion)
        {
            DiscoveryChannel = discoveryChannel;
            ApiProviderModId = apiProviderModId;
            ApiVersion = apiVersion;
        }
    }
}