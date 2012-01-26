using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrunchApiExplorer.Framework.MVVM
{
    interface IViewModel
    {
        void NotifyLoaded();
        void NotifyUnloaded();
    }
}
