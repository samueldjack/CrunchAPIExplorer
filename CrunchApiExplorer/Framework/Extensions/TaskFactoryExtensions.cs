using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CrunchApiExplorer.Framework.Extensions
{
    public static class TaskFactoryExtensions
    {
        public static Task StartNew(this TaskFactory taskFactory, Action action, TaskScheduler scheduler)
        {
            return taskFactory.StartNew(action, CancellationToken.None, TaskCreationOptions.None, scheduler);
        }
    }
}
