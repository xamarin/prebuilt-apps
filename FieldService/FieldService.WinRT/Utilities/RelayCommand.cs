using System;
using System.Diagnostics;
using System.Windows.Input;

namespace FieldService.WinRT.Utilities {

    /// <summary>
    /// A command whose sole purpose is to 
    /// relay its functionality to other
    /// objects by invoking delegates. The
    /// default return value for the CanExecute
    /// method is 'true'.
    /// </summary>
    public class RelayCommand : ICommand {

        public event EventHandler CanExecuteChanged;

        readonly Action execute;
        readonly Func<bool> canExecute;

        /// <summary>
        /// Creates a new command that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand (Action execute)
            : this (execute, null)
        {
        }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand (Action execute, Func<bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException ("execute");

            this.execute = execute;
            this.canExecute = canExecute;
        }

        void ICommand.Execute (object parameter)
        {
            execute ();
        }

        public void Invoke (object parameter)
        {
            if (((ICommand)this).CanExecute (parameter))
                ((ICommand)this).Execute (parameter);
        }

        [DebuggerStepThrough]
        bool ICommand.CanExecute (object parameter)
        {
            return canExecute == null ? true : canExecute ();
        }

        public void InvalidateCanExecute ()
        {
            var method = CanExecuteChanged;
            if (method != null) {
                method (this, EventArgs.Empty);
            }
        }
    }
}