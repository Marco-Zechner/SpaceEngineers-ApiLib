using System;
using System.Collections.Generic;
using MarcoZechner.ApiLib;

namespace MarcoZechner.ConfigAPI.Main.Api
{
    /// <summary>
    /// Main API bound to a single consumer mod.
    /// No modId needs to be passed on calls anymore.
    /// </summary>
    public sealed class YourModNameApiImpl : IApiProvider
    {
        private readonly ulong _consumerModId;
        private readonly string _consumerModName;
        private readonly YourModNameCallbackApi _yourModNameCallbackApi;

        public YourModNameApiImpl(ulong consumerModId, string consumerModName, YourModNameCallbackApi yourModNameCallbackApi)
        {
            _consumerModId = consumerModId;
            _consumerModName = consumerModName;
            _yourModNameCallbackApi = yourModNameCallbackApi;
        }

        public void Test()
        {
            // Calls the callback registered by the consumer mod (so just a roundtrip: modA -> your mod with API -> modA)
            _yourModNameCallbackApi.TestCallback();  // this is totally optional! but a good way to test if both directions work
            // could also do useful things here.
        }

        public Dictionary<string, Delegate> ConvertToDict()
        {
            return new Dictionary<string, Delegate>
            {
                { "Test", new Action(Test) }
            };
        }
    }
}