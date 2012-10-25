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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FieldService.Data;
using FieldService.Utilities;

namespace FieldService.WinRT.ViewModels {
    /// <summary>
    /// WinRT version of the ItemViewModel
    /// - We setup ICommand here
    /// </summary>
    public class ItemViewModel : FieldService.ViewModels.ItemViewModel {
        readonly AssignmentViewModel assignmentViewModel;
        readonly DelegateCommand saveAssignmentItemCommand;

        public ItemViewModel ()
        {
            assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel>();

            saveAssignmentItemCommand = new DelegateCommand (obj => {
                var item = obj as AssignmentItem;
                if (item != null && assignmentViewModel.SelectedAssignment != null) {
                    SaveAssignmentItem (assignmentViewModel.SelectedAssignment, item);
                }
            });
        }

        public DelegateCommand SaveAssignmentItemCommand
        {
            get { return saveAssignmentItemCommand; }
        }

        public IEnumerable<AssignmentItem> TopAssignmentItems
        {
            get
            {
                if (AssignmentItems == null)
                    return null;
                return AssignmentItems.Take (5);
            }
        }

        protected override void OnPropertyChanged (string propertyName)
        {
            base.OnPropertyChanged (propertyName);

            //Make sure property changed is raised for new properties
            if (propertyName == "AssignmentItems") {
                OnPropertyChanged ("TopAssignmentItems");
            }
        }
    }
}
