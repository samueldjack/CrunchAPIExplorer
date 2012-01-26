namespace CrunchApiExplorer.Crunch
{
    public class CrunchAuthorisationParameters
    {
        private string _consumerKey;
        private string _sharedSecret;
        private string _requestTokenEndpoint;
        private string _accessTokenEndpoint;
        private string _userAuthorizationEndpoint;

        public CrunchAuthorisationParameters(string consumerKey, string sharedSecret, string requestTokenEndpoint, string accessTokenEndpoint, string userAuthorizationEndpoint)
        {
            _consumerKey = consumerKey;
            _sharedSecret = sharedSecret;
            _requestTokenEndpoint = requestTokenEndpoint;
            _accessTokenEndpoint = accessTokenEndpoint;
            _userAuthorizationEndpoint = userAuthorizationEndpoint;
        }

        public string ConsumerKey
        {
            get { return _consumerKey; }
        }

        public string SharedSecret
        {
            get { return _sharedSecret; }
        }

        public string RequestTokenEndpoint
        {
            get { return _requestTokenEndpoint; }
        }

        public string AccessTokenEndpoint
        {
            get { return _accessTokenEndpoint; }
        }

        public string UserAuthorizationEndpoint
        {
            get { return _userAuthorizationEndpoint; }
        }
    }
}