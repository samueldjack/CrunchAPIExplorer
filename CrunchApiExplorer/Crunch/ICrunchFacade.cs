using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CrunchApiExplorer.Crunch
{
    public interface ICrunchFacade
    {
        Task ChangeConnectionAsync(CrunchAuthorisationParameters crunchAuthorisationParameters);
        CrunchAuthorisationParameters GetCurrentAuthorisationParameters();
        Task<XElement> MakeRequestAsync(string requestUrl, HttpMethod httpMethod, XDocument requestBody);
    }
}