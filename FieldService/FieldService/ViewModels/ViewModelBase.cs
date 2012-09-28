using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FieldService.ViewModels {
    public class ViewModelBase : INotifyPropertyChanged, IDataErrorInfo {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        readonly Dictionary<string, string> errors = new Dictionary<string, string> ();

        protected virtual void OnPropertyChanged (string propertyName)
        {
            PropertyChanged (this, new PropertyChangedEventArgs (propertyName));
        }

        public bool IsValid
        {
            get { return errors.Count == 0; }
        }

        protected Dictionary<string, string> Errors
        {
            get { return errors; }
        }

        public virtual string Error
        {
            get
            {
                return errors.Values.Aggregate (new StringBuilder (), (b, s) => b.AppendLine (s)).ToString ();
            }
        }

        public virtual string this [string columnName]
        {
            get
            {
                string error;
                return errors.TryGetValue (columnName, out error) ? error : string.Empty;
            }
        }
    }
}
