using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrunchApiExplorer.Crunch
{
    public class CrunchFacade : ICrunchFacade
    {
        public Task ChangeConnection(string consumerKey, string sharedSecret, string requestTokenEndpoint, string accessTokenEndpoint, string userAuthorizationEndpoint)
        {
            return new TaskCompletionSource<object>().Task;
        }
    }
}
