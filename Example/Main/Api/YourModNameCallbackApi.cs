using System;
using MarcoZechner.ApiLib;
using MarcoZechner.ConfigAPI.Shared.Api;

namespace MarcoZechner.ConfigAPI.Main.Api
{
    public class YourModNameCallbackApi : IYourModNameCallbackApi
    {
        private readonly Action _testCallback = null;
        
        public YourModNameCallbackApi(IApiProvider yourCallbackApi)
        {
            var dict = yourCallbackApi.ConvertToDict();
            Delegate d;
            // safety check, is technically not needed, since if the versions mismatch it won't get this far.
            // but if you accidentally say it's a minor change, but actually broke backwards compatibility, this will prevent a crash.
            // On the other hand, maybe you want it to crash to notice the issue sooner rather than later.
            if (dict != null && dict.TryGetValue("TestCallback", out d))
                _testCallback = (Action)d;
        }

        public void TestCallback() => _testCallback?.Invoke();
    }
}