using System;
using System.Collections.Generic;
using MarcoZechner.ApiLib;
using MarcoZechner.ConfigAPI.Shared.Api;

namespace MarcoZechner.ConfigAPI.Client.Api
{
    public static class ApiLoader
    {
        public static bool ApiLoaded => _bridge != null && _bridge.ApiLoaded;

        private static ApiConsumerBridge _bridge;
        private static YourModNameApi _api;
        private static YourModNameCallbackApiImpl _yourModNameCallback;

        public static YourModNameApi Api => ApiLoaded ? _api : null;

        public static void Init(ulong modId, string modName)
        {
            _yourModNameCallback = new YourModNameCallbackApiImpl();

            _bridge = new ApiConsumerBridge(
                new ConfigApiBootstrap(),
                modId,
                modName,
                BuildCallbackDict,
                SetMainApi
            );
            
            _bridge.Init();
        }

        public static void Unload()
        {
            if (_bridge != null)
            {
                _bridge.Unload();
                _bridge = null;
            }

            _api = null;
            _yourModNameCallback = null;
        }
        
        private static Dictionary<string, Delegate> BuildCallbackDict() 
            => _yourModNameCallback != null ? _yourModNameCallback.ConvertToDict() : new Dictionary<string, Delegate>();

        private static void SetMainApi(Dictionary<string, Delegate> mainApiDict) => _api = new YourModNameApi(mainApiDict);
    }
}