using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrunchApiExplorer.Crunch
{
    interface IVerifyUserRequestToken
    {
        Task<string> Verify(Uri userAuthorisationUri);
    }
}
