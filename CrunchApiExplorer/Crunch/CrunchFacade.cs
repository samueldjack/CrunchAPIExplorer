using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using CrunchApiExplorer.Properties;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.OAuth.ChannelElements;
using CrunchApiExplorer.Framework.Extensions;

namespace CrunchApiExplorer.Crunch
{
    class CrunchFacade : ICrunchFacade
    {
        private readonly IVerifyUserRequestToken _requestTokenVerifier;

        public event EventHandler<EventArgs> ConnectionStatusChanged;

        public CrunchFacade(IVerifyUserRequestToken requestTokenVerifier)
        {
            _requestTokenVerifier = requestTokenVerifier;
        }

        public Task ChangeConnectionAsync(CrunchAuthorisationParameters crunchAuthorisationParameters)
        {
            return Task.Factory.StartNew(
                () =>
                    {
                        var consumer = CreateConsumer(crunchAuthorisationParameters);

                        string requestToken;
                        var userAuthorisationUri = consumer.RequestUserAuthorization(new Dictionary<string, string>(),
                                                                                     new Dictionary<string, string>(),
                                                                                     out requestToken);

                        var verifierTask = _requestTokenVerifier.Verify(userAuthorisationUri);

                        // wait synchronously for the result. We're already on a background thread, so we won't block the UI
                        var verifier = verifierTask.Result;

                        var result = consumer.ProcessUserAuthorization(requestToken, verifier);

                        StoreParametersAndAccessToken(crunchAuthorisationParameters, result.AccessToken);

                        RaiseConnectionStatucChanged(EventArgs.Empty);
                    });
        }

        private static DesktopConsumer CreateConsumer(CrunchAuthorisationParameters crunchAuthorisationParameters)
        {
            var tokenManager = new InMemoryTokenManager(crunchAuthorisationParameters.ConsumerKey,
                                                        crunchAuthorisationParameters.SharedSecret);

            var serviceProviderDescription = CreateServiceProviderDescription(crunchAuthorisationParameters);

            var consumer = new DesktopConsumer(serviceProviderDescription, tokenManager);
            return consumer;
        }

        public CrunchAuthorisationParameters GetCurrentAuthorisationParameters()
        {
            var crunchAuthorisationParameters = new CrunchAuthorisationParameters(
                sharedSecret: Settings.Default.SharedSecret.IsNullOrWhiteSpace() ? "": Settings.Default.SharedSecret.DecryptBase64EncodedString(),
                accessTokenEndpoint: Settings.Default.AccessTokenEndpoint,
                consumerKey: Settings.Default.ConsumerKey,
                requestTokenEndpoint: Settings.Default.RequestTokenEndpoint,
                userAuthorizationEndpoint: Settings.Default.UserAuthorizationEndpoint
                );

            return crunchAuthorisationParameters;
        }

        public bool IsConnected
        {
            get { return !Settings.Default.AccessToken.IsNullOrWhiteSpace(); }
        }

        public Uri Authority
        {
            get
            {
                var serverSetting = Settings.Default.UserAuthorizationEndpoint;
                if (serverSetting.IsNullOrWhiteSpace())
                {
                    return null;
                }
                else
                {
                    var uri = new Uri(serverSetting);
                    return new Uri(uri.GetLeftPart(UriPartial.Authority));
                }
            }
        }

        public Task<XElement> MakeRequestAsync(Uri resourceUrl, HttpMethod httpMethod, XDocument requestBody)
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("You must connect to crunch first");
            }

            var task = Task.Factory.StartNew(
                () =>
                    {
                        var cap = GetCurrentAuthorisationParameters();
                        var consumer = CreateConsumer(cap);
                        SetAccessToken(consumer);

                        var request = consumer.PrepareAuthorizedRequest(
                            new MessageReceivingEndpoint(new Uri(Authority, resourceUrl),
                                                         TranslateHttpMethod(httpMethod)),
                            Settings.Default.AccessToken.DecryptBase64EncodedString());


                        if (httpMethod == HttpMethod.Post && requestBody != null)
                        {
                            request.SetRequestBody(
                                requestBody.ToString(SaveOptions.DisableFormatting),
                                "application/xml");
                        }

                        var response = request.GetResponse();
                        using (var stream = response.GetResponseStream())
                        {
                            var xmlReader = XmlReader.Create(stream);
                            xmlReader.MoveToContent();

                            var document = (XElement) XNode.ReadFrom(xmlReader);
                            return document;
                        }

                    });

            return task;
        }

        private static HttpDeliveryMethods TranslateHttpMethod(HttpMethod httpMethod)
        {
            if (httpMethod == HttpMethod.Get)
            {
                return HttpDeliveryMethods.GetRequest |
                       HttpDeliveryMethods.AuthorizationHeaderRequest;
            }
            else if (httpMethod == HttpMethod.Post)
            {
                return HttpDeliveryMethods.PostRequest
                    | HttpDeliveryMethods.AuthorizationHeaderRequest;
            }
            else if (httpMethod == HttpMethod.Put)
            {
                return HttpDeliveryMethods.PutRequest
                    | HttpDeliveryMethods.AuthorizationHeaderRequest;
            }
            else if (httpMethod == HttpMethod.Delete)
            {
                return HttpDeliveryMethods.DeleteRequest
                    | HttpDeliveryMethods.AuthorizationHeaderRequest;
            }
            else
            {
                throw new ArgumentException("HttpMethod not supported");
            }
        }

        private void SetAccessToken(DesktopConsumer consumer)
        {
            ((InMemoryTokenManager)consumer.TokenManager).AddAccessTokenAndSecret(
                Settings.Default.AccessToken.DecryptBase64EncodedString(), 
                Settings.Default.SharedSecret.DecryptBase64EncodedString());
        }

        private static ServiceProviderDescription CreateServiceProviderDescription(CrunchAuthorisationParameters crunchAuthorisationParameters)
        {
            return new ServiceProviderDescription()
                       {
                           ProtocolVersion = ProtocolVersion.V10a,
                           RequestTokenEndpoint =
                               new MessageReceivingEndpoint(crunchAuthorisationParameters.RequestTokenEndpoint,
                                                            HttpDeliveryMethods.PostRequest |HttpDeliveryMethods.AuthorizationHeaderRequest),
                           AccessTokenEndpoint =
                               new MessageReceivingEndpoint(crunchAuthorisationParameters.AccessTokenEndpoint,
                                                            HttpDeliveryMethods.PostRequest |HttpDeliveryMethods.AuthorizationHeaderRequest),
                           UserAuthorizationEndpoint =
                               new MessageReceivingEndpoint(crunchAuthorisationParameters.UserAuthorizationEndpoint,
                                                            HttpDeliveryMethods.PostRequest |HttpDeliveryMethods.AuthorizationHeaderRequest),
                           TamperProtectionElements =
                               new ITamperProtectionChannelBindingElement[] {new HmacSha1SigningBindingElement()},
                       };
        }

        private void StoreParametersAndAccessToken(CrunchAuthorisationParameters crunchAuthorisationParameters, string accessToken)
        {
            Settings.Default.AccessTokenEndpoint = crunchAuthorisationParameters.AccessTokenEndpoint;
            Settings.Default.RequestTokenEndpoint = crunchAuthorisationParameters.RequestTokenEndpoint;
            Settings.Default.UserAuthorizationEndpoint = crunchAuthorisationParameters.UserAuthorizationEndpoint;
            Settings.Default.ConsumerKey = crunchAuthorisationParameters.ConsumerKey;
            Settings.Default.SharedSecret = crunchAuthorisationParameters.SharedSecret.EncryptAndEncodeAsBase64();
            Settings.Default.AccessToken = accessToken.EncryptAndEncodeAsBase64();
            Settings.Default.Save();
        }

        protected void RaiseConnectionStatucChanged(EventArgs e)
        {
            EventHandler<EventArgs> handler = ConnectionStatusChanged;
            if (handler != null) handler(this, e);
        }
    }
}
