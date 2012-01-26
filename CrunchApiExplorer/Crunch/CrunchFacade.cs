using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public CrunchFacade(IVerifyUserRequestToken requestTokenVerifier)
        {
            _requestTokenVerifier = requestTokenVerifier;
        }

        public Task ChangeConnection(CrunchAuthorisationParameters crunchAuthorisationParameters)
        {
            return Task.Factory.StartNew(
                () =>
                    {
                        var tokenManager = new InMemoryTokenManager(crunchAuthorisationParameters.ConsumerKey,
                                                                    crunchAuthorisationParameters.SharedSecret);

                        var serviceProviderDescription = CreateServiceProviderDescription(crunchAuthorisationParameters);

                        var consumer = new DesktopConsumer(serviceProviderDescription, tokenManager);

                        string requestToken;
                        var userAuthorisationUri = consumer.RequestUserAuthorization(new Dictionary<string, string>(),
                                                                                     new Dictionary<string, string>(),
                                                                                     out requestToken);

                        var verifierTask = _requestTokenVerifier.Verify(userAuthorisationUri);

                        // wait synchronously for the result. We're already on a background thread, so we won't block the UI
                        var verifier = verifierTask.Result;

                        var result = consumer.ProcessUserAuthorization(requestToken, verifier);

                        StoreParametersAndAccessToken(crunchAuthorisationParameters, result.AccessToken);
                    });
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
    }
}
