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
    public class RelayCommand<T> : ICommand {
        #region Constructors

        public RelayCommand (Action<T> execute)
            : this (execute, null)
        {
        }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand (Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException ("execute");

            this.execute = execute;
            this.canExecute = canExecute;
        }

        #endregion // Constructors

        #region ICommand Members

        [DebuggerStepThrough]
        public bool CanExecute (object parameter)
        {
            return canExecute == null ? true : canExecute ((T)parameter);
        }

        public void InvalidateCanExecute ()
        {
            var method = CanExecuteChanged;
            if (method != null) {
                method (this, EventArgs.Empty);
            }
        }

        public event EventHandler CanExecuteChanged;

        public void Execute (object parameter)
        {
            execute ((T)parameter);
        }

        #endregion // ICommand Members

        #region Fields

        readonly Action<T> execute = null;
        readonly Predicate<T> canExecute = null;

        #endregion // Fields
    }

    /// <summary>
    /// A command whose sole purpose is to 
    /// relay its functionality to other
    /// objects by invoking delegates. The
    /// default return value for the CanExecute
    /// method is 'true'.
    /// </summary>
    public class RelayCommand : ICommand {
        #region Constructors

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

        #endregion // Constructors

        #region ICommand Members

        [DebuggerStepThrough]
        public bool CanExecute (object parameter)
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

        public event EventHandler CanExecuteChanged;

        public void Execute (object parameter)
        {
            execute ();
        }

        #endregion // ICommand Members

        #region Fields

        readonly Action execute;
        readonly Func<bool> canExecute;

        #endregion // Fields
    }
}