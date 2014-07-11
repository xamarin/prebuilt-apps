//
//  Copyright 2012  Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using System.Windows.Input;

/// <summary>
/// Simple ICommand implementation to help with MVVM design pattern
/// </summary>
public class DelegateCommand : ICommand {
    EventHandler canExecuteChanged;
    readonly Predicate<object> canExecute;
    readonly Action<object> execute;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="execute">An action for when the command is executed</param>
    public DelegateCommand (Action<object> execute)
        : this (execute, null)
    {
    }

    /// <summary>
    /// Constructor providing a second callback for CanExecute
    /// </summary>
    /// <param name="execute">An action for when the command is executed</param>
    /// <param name="canExecute">A predicate to determine if the command can be executed</param>
    public DelegateCommand (Action<object> execute, Predicate<object> canExecute)
    {
        this.execute = execute;
        this.canExecute = canExecute;
    }

    /// <summary>
    /// Invokes the command manually from code, checking CanExecute
    /// </summary>
    /// <param name="parameter">Optional parameter to ICommand</param>
    public void Invoke (object parameter = null)
    {
        var command = (ICommand)this;
        if (command.CanExecute (parameter))
            command.Execute (parameter);
    }

    /// <summary>
    /// Method for invalidating CanExecute
    /// </summary>
    public void RaiseCanExecuteChanged ()
    {
        var method = canExecuteChanged;
        if (method != null)
            method (this, EventArgs.Empty);
    }

    #region ICommand interface

    bool ICommand.CanExecute (object parameter)
    {
        if (canExecute == null) {
            return true;
        }

        return canExecute (parameter);
    }

    event EventHandler ICommand.CanExecuteChanged
    {
        add { canExecuteChanged += value; }
        remove { canExecuteChanged -= value; }
    }

    void ICommand.Execute (object parameter)
    {
        execute (parameter);
    }

    #endregion
}