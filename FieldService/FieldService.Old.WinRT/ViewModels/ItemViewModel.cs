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
using FieldService.Utilities;
using FieldService.WinRT.Utilities;
using FieldService.WinRT.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace FieldService.WinRT.ViewModels {
    /// <summary>
    /// WinRT version of the ItemViewModel
    /// - We setup ICommand here
    /// </summary>
    public class ItemViewModel : FieldService.ViewModels.ItemViewModel {
        readonly AssignmentViewModel assignmentViewModel;
        readonly DelegateCommand saveAssignmentItemCommand, searchItemsCommand, addItemCommand, cancelAddItemCommand, deleteItemCommand;
        string searchText = string.Empty;
        Popup addItemPopUp;
        IEnumerable<Item> searchItems;
        AssignmentItem selectedItem;

        public ItemViewModel ()
        {
            assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();

            saveAssignmentItemCommand = new DelegateCommand (obj => {
                var item = obj as AssignmentItem;
                if (item != null && assignmentViewModel.SelectedAssignment != null) {
                    SaveAssignmentItemAsync (assignmentViewModel.SelectedAssignment, item);
                }
            });

            searchItemsCommand = new DelegateCommand (async _ => {
                await LoadItemsAsync ();
                var items = new List<Item> ();
                foreach (var item in Items) {
                    if (item.Name.ToLower ().Contains (SearchText.ToLower ()) || item.Number.ToLower ().Contains (SearchText.ToLower ())) {
                        items.Add (item);
                    }
                }
                SearchItems = items;
            });

            addItemCommand = new DelegateCommand (_ => {
                if (addItemPopUp != null && addItemPopUp.IsOpen) {
                    addItemPopUp.IsOpen = false;
                }
                addItemPopUp = new Popup ();
                addItemPopUp.Height = Window.Current.Bounds.Height;
                addItemPopUp.Width = Constants.PopUpWidth;
                AddItemFlyoutPanel flyoutpanel = new AddItemFlyoutPanel ();
                flyoutpanel.Width = addItemPopUp.Width;
                flyoutpanel.Height = addItemPopUp.Height;
                addItemPopUp.Child = flyoutpanel;
                addItemPopUp.SetValue (Canvas.LeftProperty, Window.Current.Bounds.Width - Constants.PopUpWidth);
                addItemPopUp.SetValue (Canvas.TopProperty, 0);
                addItemPopUp.IsOpen = true;
                SearchText = string.Empty;
                SearchItemsCommand.Invoke ();
            });

            cancelAddItemCommand = new DelegateCommand (_ => {
                addItemPopUp.IsOpen = false;
            });

            deleteItemCommand = new DelegateCommand (async _ => {
                if (selectedItem != null) {
                    await DeleteAssignmentItemAsync (assignmentViewModel.SelectedAssignment, selectedItem);
                    await LoadAssignmentItemsAsync (assignmentViewModel.SelectedAssignment);
                }
            });
        }

        /// <summary>
        /// Flyout panel for adding an item
        /// </summary>
        public Popup ItemPopUp
        {
            get { return addItemPopUp; }
        }

        /// <summary>
        /// Command for saving the assignment item
        /// </summary>
        public DelegateCommand SaveAssignmentItemCommand
        {
            get { return saveAssignmentItemCommand; }
        }

        /// <summary>
        /// Command for searching the through the list of items
        /// </summary>
        public DelegateCommand SearchItemsCommand
        {
            get { return searchItemsCommand; }
        }

        /// <summary>
        /// Command for opening the add item flyout panel
        /// </summary>
        public DelegateCommand AddItemCommand
        {
            get { return addItemCommand; }
        }

        /// <summary>
        /// Command to cancel the flyout panel
        /// </summary>
        public DelegateCommand CancelAddItemCommand
        {
            get { return cancelAddItemCommand; }
        }

        /// <summary>
        /// Command to delete an item
        /// </summary>
        public DelegateCommand DeleteItemCommand
        {
            get { return deleteItemCommand; }
        }

        /// <summary>
        /// list of top assignment items
        /// </summary>
        public IEnumerable<AssignmentItem> TopAssignmentItems
        {
            get
            {
                if (AssignmentItems == null)
                    return null;
                return AssignmentItems.Take (5);
            }
        }

        /// <summary>
        /// Search text property for searching throug list of items
        /// </summary>
        public string SearchText
        {
            get { return searchText; }
            set { searchText = value; OnPropertyChanged ("SearchText"); }
        }

        /// <summary>
        /// list of items that have search applied
        /// </summary>
        public IEnumerable<Item> SearchItems
        {
            get { return searchItems; }
            set { searchItems = value; OnPropertyChanged ("SearchItems"); }
        }

        /// <summary>
        /// Value for not is history
        /// </summary>
        public bool IsNotReadonly
        {
            get { return assignmentViewModel.IsNotReadonly; }
        }

        public AssignmentItem SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; OnPropertyChanged ("SelectedItem"); }
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
