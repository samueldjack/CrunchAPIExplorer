using System.Threading.Tasks;

namespace CrunchApiExplorer.Crunch
{
    public interface ICrunchFacade
    {
        Task ChangeConnection(CrunchAuthorisationParameters crunchAuthorisationParameters);
        CrunchAuthorisationParameters GetCurrentAuthorisationParameters();
    }
}