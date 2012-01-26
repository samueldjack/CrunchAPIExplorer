using System.Threading.Tasks;

namespace CrunchApiExplorer.Crunch
{
    public interface ICrunchFacade
    {
        Task ChangeConnection(string consumerKey, string sharedSecret, string requestTokenEndpoint, string accessTokenEndpoint, string userAuthorizationEndpoint);
    }
}