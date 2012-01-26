﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace CrunchApiExplorer.Framework.MVVM
{
    public class CommandBase : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public CommandBase()
        {
        }

        public virtual void Execute(object parameter)
        {
        }

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        protected void OnCanExecuteChanged(EventArgs e)
        {
            EventHandler handler = CanExecuteChanged;
            if (handler != null) handler(this, e);
        }
    }
}
