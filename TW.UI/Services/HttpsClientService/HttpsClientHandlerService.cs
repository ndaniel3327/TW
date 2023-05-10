//using Javax.Net.Ssl;
//using Xamarin.Android.Net;

//namespace TW.UI.Services.HttpsClientService
//{
//    public class HttpsClientHandlerService
//    {
//        public HttpMessageHandler GetPlatformMessageHandler()
//        {
//#if ANDROID
//            var handler = new AndroidMessageHandler();
//            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
//            {
//                if (cert != null && cert.Issuer.Equals("CN=localhost"))
//                    return true;
//                return errors == System.Net.Security.SslPolicyErrors.None;
//            };
//            return handler;
//#elif IOS
//        var handler = new NSUrlSessionHandler
//        {
//            TrustOverrideForUrl = IsHttpsLocalhost
//        };
//        return handler;
//#else
//     throw new PlatformNotSupportedException("Only Android and iOS supported.");
//#endif
//        }

//#if IOS
//    public bool IsHttpsLocalhost(NSUrlSessionHandler sender, string url, Security.SecTrust trust)
//    {
//        if (url.StartsWith("https://localhost"))
//            return true;
//        return false;
//    }
//#endif
//    }
//    internal sealed class CustomAndroidMessageHandler : AndroidMessageHandler
//    {
//        protected override IHostnameVerifier GetSSLHostnameVerifier(HttpsURLConnection connection)
//            => new CustomHostnameVerifier();

//        private sealed class CustomHostnameVerifier : Java.Lang.Object, IHostnameVerifier
//        {
//            public bool Verify(string hostname, ISSLSession session)
//                => HttpsURLConnection.DefaultHostnameVerifier.Verify(hostname, session)
//                    || (hostname == "10.0.2.2" && session.PeerPrincipal.Name == "CN=localhost");
//        }
//    }
//}
