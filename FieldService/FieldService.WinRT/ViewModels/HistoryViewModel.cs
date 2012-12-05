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

using System.Collections.Generic;
using System.Linq;
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
