using System;
using System.Collections.Generic;
using MarcoZechner.ApiLib;
using MarcoZechner.ConfigAPI.Shared.Api;

namespace MarcoZechner.ConfigAPI.Client.Api
{
    internal sealed class YourModNameCallbackApiImpl : IYourModNameCallbackApi, IApiProvider
    {
        public void TestCallback()
        {
            // do something
        }
        
        public Dictionary<string, Delegate> ConvertToDict()
        {
            return new Dictionary<string, Delegate>
            {
                { "TestCallback", new Action(TestCallback) },
            };
        }
    }
}