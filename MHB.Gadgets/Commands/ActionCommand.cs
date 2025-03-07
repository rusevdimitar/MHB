// -----------------------------------------------------------------------
// <copyright file="BlockUserCommand.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MHB.Gadgets.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Input;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ActionCommand : ICommand
    {
        private Action<object> _executeDelegate;

        public ActionCommand(Action<object> executeDelegate)
        {
            _executeDelegate = executeDelegate;
        }

        public void Execute(object parameter)
        {
            _executeDelegate(parameter);
        }

        public bool CanExecute(object parameter) { return true; }

#pragma warning disable 0067

        public event EventHandler CanExecuteChanged;

#pragma warning restore 0067
    }
}