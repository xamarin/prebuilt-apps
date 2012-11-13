using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FieldService.Data;

namespace FieldService.WinRT.ViewModels {
    public class HistoryViewModel : FieldService.ViewModels.HistoryViewModel {

        public AssignmentHistory RecentHistory
        {
            get
            {
                if (History != null)
                    return History.FirstOrDefault ();
                else
                    return null;
            }
        }

        public IEnumerable<AssignmentHistory> TopHistory
        {
            get
            {
                if (History != null)
                    return History.Skip (1).Take (3);
                else
                    return null;
            }
        }

        public AssignmentHistory FirstHistory
        {
            get
            {
                if (TopHistory != null) {
                    return TopHistory.ElementAtOrDefault (0);
                }
                return null;
            }
        }

        public AssignmentHistory SecondHistory
        {
            get
            {
                if (TopHistory != null) {
                    return TopHistory.ElementAtOrDefault (1);
                }
                return null;
            }
        }

        public AssignmentHistory ThirdHistory
        {
            get
            {
                if (TopHistory != null) {
                    return TopHistory.ElementAtOrDefault (2);
                }
                return null;
            }
        }


        protected override void OnPropertyChanged (string propertyName)
        {
            base.OnPropertyChanged (propertyName);

            //Make sure property changed is raised for new properties
            if (propertyName == "History") {
                OnPropertyChanged ("TopHistory");
                OnPropertyChanged ("RecentHistory");
                OnPropertyChanged ("FirstHistory");
                OnPropertyChanged ("SecondHistory");
                OnPropertyChanged ("ThirdHistory");
            }
        }
    }
}
