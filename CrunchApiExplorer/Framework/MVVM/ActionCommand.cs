﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrunchApiExplorer.Framework.MVVM
{
    public class ActionCommand : CommandBase
    {
        private readonly Action<object> _executeAction;

        public ActionCommand(Action<object> executeAction)
        {
            _executeAction = executeAction;
        }

        public override void Execute(object parameter)
        {
            _executeAction(parameter);
        }
    }
}
