using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FieldService.Data {
    /// <summary>
    /// Base class for implementing INotifyPropertyChanged
    /// </summary>
    public class PropertyChangedBase : INotifyPropertyChanged {

        /// <summary>
        /// Event for INotifyPropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Protected method for firing PropertyChanged
        /// </summary>
        /// <param name="propertyName">The name of the property that changed</param>
        protected virtual void OnPropertyChanged (string propertyName)
        {
            var method = PropertyChanged;
            if (method != null)
                method (this, new PropertyChangedEventArgs (propertyName));
        }
    }
}
