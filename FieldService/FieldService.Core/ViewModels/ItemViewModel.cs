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
using System.Threading;
using System.Threading.Tasks;
using FieldService.Data;
using FieldService.Utilities;

namespace FieldService.ViewModels {
    /// <summary>
    /// ViewModel for working with items
    /// </summary>
    public class ItemViewModel : ViewModelBase {
        readonly IAssignmentService service;
        List<AssignmentItem> assignmentItems;
        List<Item> items;

        public ItemViewModel ()
        {
            service = ServiceContainer.Resolve<IAssignmentService> ();
        }

        /// <summary>
        /// List of items with an assignment
        /// </summary>
        public List<AssignmentItem> AssignmentItems
        {
            get { return assignmentItems; }
            set { assignmentItems = value; OnPropertyChanged ("AssignmentItems"); }
        }

        /// <summary>
        /// List of items
        /// </summary>
        public List<Item> Items
        {
            get { return items; }
            set { items = value; OnPropertyChanged ("Items"); }
        }

        /// <summary>
        /// Loads all the available items
        /// </summary>
        public Task LoadItemsAsync ()
        {
            return service
                .GetItemsAsync ()
                .ContinueOnCurrentThread (t => Items = t.Result);
        }

        /// <summary>
        /// Loads a list of items for an assignment
        /// </summary>
        public Task LoadAssignmentItemsAsync (Assignment assignment)
        {
            return service
                .GetItemsForAssignmentAsync (assignment)
                .ContinueOnCurrentThread (t => AssignmentItems = t.Result);
        }

        /// <summary>
        /// Saves an assignment item
        /// </summary>
        public Task SaveAssignmentItemAsync (Assignment assignment, AssignmentItem item)
        {
            bool newItem = item.Id == 0;

            return service
                .SaveAssignmentItemAsync (item)
                .ContinueWith (t => {
                    if (newItem)
                        assignment.TotalItems++;
                });
        }

        /// <summary>
        /// Deletes an assignment item
        /// </summary>
        public Task DeleteAssignmentItemAsync (Assignment assignment, AssignmentItem item)
        {
            return service
                .DeleteAssignmentItemAsync (item)
                .ContinueWith (t => assignment.TotalItems--);
        }
    }
}
