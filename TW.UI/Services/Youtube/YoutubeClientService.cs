
namespace TW.UI.Services.Youtube
{
    public class YoutubeClientService : IYoutubeClientService
    {
        private readonly string _clientId = "829868223814-gn9dbtit6si40k2vd7thblkfi4a1lv4i.apps.googleusercontent.com";
        private readonly string _verifier;
        private readonly string _challenge;

        public YoutubeClientService()
        {
            //var (verifier, challenge) = PKCEUtil.GenerateCodes();
            //_verifier = verifier;
            //_challenge = challenge;
        }
        public Uri AuthorizeYoutube()
        {
            string myUri = "https://accounts.google.com/o/oauth2/v2/auth?" +
              "scope=https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fyoutube.readonly&" +
              "response_type=code&" +
              "state=security_token%3D138r5719ru3e1%26url%3Dhttps%3A%2F%2Foauth2.example.com%2Ftoken&" +
              "redirect_uri=com.googleusercontent.apps.829868223814-gn9dbtit6si40k2vd7thblkfi4a1lv4i:&" +
              //"redirect_uri=https://oauth2.example.com/code&"+
              $"client_id={_clientId}";
            
            return new Uri(myUri);
        }
    }
}
