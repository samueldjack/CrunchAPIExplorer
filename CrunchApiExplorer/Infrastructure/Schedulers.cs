using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrunchApiExplorer.Infrastructure
{
    public static class Schedulers
    {
        public static void Initialise()
        {
            UIThread = TaskScheduler.FromCurrentSynchronizationContext();
        }

        public static TaskScheduler UIThread { get; private set; }
    }
}
