using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CrunchApiExplorer.Crunch
{
    public interface ICrunchFacade
    {
        Task<bool> ChangeConnectionAsync(CrunchAuthorisationParameters crunchAuthorisationParameters);
        CrunchAuthorisationParameters GetCurrentAuthorisationParameters();
        Task<XElement> MakeRequestAsync(Uri resourceUri, HttpMethod httpMethod, XDocument requestBody);
        Uri Authority { get; }
        bool IsConnected { get; }
        event EventHandler<EventArgs> ConnectionStatusChanged;
    }
}