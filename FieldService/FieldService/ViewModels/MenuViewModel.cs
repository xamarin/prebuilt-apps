using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FieldService.ViewModels {
    /// <summary>
    /// A simple ViewModel for coordinating menu changes within the app
    /// </summary>
    public class MenuViewModel : ViewModelBase {
        int menuIndex = 0;

        /// <summary>
        /// The current selected index for the menu in the split controller/fragment
        /// </summary>
        public int MenuIndex
        {
            get { return menuIndex; }
            set { menuIndex = value; OnPropertyChanged ("MenuIndex"); }
        }
    }
}
