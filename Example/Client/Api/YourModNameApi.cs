using System;
using MarcoZechner.ApiLib;
using MarcoZechner.ConfigAPI.Shared.Api;

namespace MarcoZechner.ConfigAPI.Client.Api
{
    public sealed class YourModNameApi : IYourModNameApi
    {
        private readonly Action _test;

        public YourModNameApi(IApiProvider yourModNameApi)
        {
            var dict = yourModNameApi.ConvertToDict();
            
            // here we trust that the apiVersion is set correctly (meaning we only get here if at least major version matches)
            // it could crash if you only change the minor version, but break backwards compatibility.
            // a safe example can be found in Main/Api/CallBackApi.cs
            _test = (Action)dict["Test"];
        }

        public void Test() 
            => _test();
    }
}