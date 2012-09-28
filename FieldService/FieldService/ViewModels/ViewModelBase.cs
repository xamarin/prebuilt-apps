using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FieldService.ViewModels {
    /// <summary>
    /// Base class for all view models
    /// - Implements INotifyPropertyChanged for WinRT
    /// - Implements some basic validation logic
    /// - Implements some IsBusy logic
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged {

        /// <summary>
        /// Event for INotifyPropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        readonly List<string> errors = new List<string> ();
        bool isBusy = false;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ViewModelBase ()
        {
            //Make sure validation is performed on startup
            Validate ();
        }

        /// <summary>
        /// Protected method for firing PropertyChanged
        /// </summary>
        /// <param name="propertyName">The name of the property that changed</param>
        protected virtual void OnPropertyChanged (string propertyName)
        {
            var method = PropertyChanged;
            if (method != null) {
                method (this, new PropertyChangedEventArgs (propertyName));
            }
        }

        /// <summary>
        /// Returns true if the current state of the ViewModel is valid
        /// </summary>
        public bool IsValid
        {
            get { return errors.Count == 0; }
        }

        /// <summary>
        /// A list of errors if IsValid is false
        /// </summary>
        protected List<string> Errors
        {
            get { return errors; }
        }

        /// <summary>
        /// An aggregated error message
        /// </summary>
        public virtual string Error
        {
            get
            {
                return errors.Aggregate (new StringBuilder (), (b, s) => b.AppendLine (s)).ToString ().Trim ();
            }
        }

        /// <summary>
        /// Protected method for validating the ViewModel
        /// - Fires PropertyChanged for IsValid and Errors
        /// </summary>
        protected virtual void Validate ()
        {
            OnPropertyChanged ("IsValid");
            OnPropertyChanged ("Errors");
        }

        protected virtual void ValidateProperty (Func<bool> validate, string error)
        {
            if (validate ()) {
                if (!Errors.Contains (error))
                    Errors.Add (error);
            } else {
                Errors.Remove (error);
            }
        }

        /// <summary>
        /// Value inidicating if a spinner should be shown
        /// </summary>
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                Validate ();
                OnPropertyChanged ("IsBusy");
            }
        }
    }
}