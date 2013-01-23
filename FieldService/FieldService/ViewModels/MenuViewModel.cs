using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FieldService.ViewModels {
    /// <summary>
    /// A simple ViewModel for coordinating menu changes within the app
    /// </summary>
    public class MenuViewModel : ViewModelBase {

        /// <summary>
        /// Event when the menu index changes
        /// </summary>
        public event EventHandler MenuIndexChanged;

        int menuIndex = 0;

        /// <summary>
        /// The current selected index for the menu in the split controller/fragment
        /// </summary>
        public int MenuIndex
        {
            get { return menuIndex; }
            set { menuIndex = value; OnMenuIndexChanged (); }
        }

        private void OnMenuIndexChanged ()
        {
            OnPropertyChanged ("MenuIndex");

            var method = MenuIndexChanged;
            if (method != null)
                method (this, EventArgs.Empty);
        }
    }
}
