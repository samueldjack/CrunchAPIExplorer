using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Input;
using CrunchApiExplorer.Framework.Extensions;

namespace CrunchApiExplorer.Framework.MVVM
{
    public class CommandHolder
    {
        Dictionary<string, ICommand> _commands = new Dictionary<string, ICommand>();
 
        public ICommand GetOrCreateCommand(Expression<Func<ICommand>> commandPropertyExpression, Action executeAction)
        {
            var name = commandPropertyExpression.GetPropertyName();
            ICommand command;
            if (!_commands.TryGetValue(name, out command))
            {
                command = new ActionCommand(_ => executeAction());
                _commands.Add(name, command);
            }

            return command;
        }
    }
}
