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
using Windows.UI.Xaml.Controls.Primitives;
using FieldService.WinRT.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FieldService.WinRT.ViewModels {
    /// <summary>
    /// WinRT version of the ItemViewModel
    /// - We setup ICommand here
    /// </summary>
    public class ItemViewModel : FieldService.ViewModels.ItemViewModel {
        readonly AssignmentViewModel assignmentViewModel;
        readonly DelegateCommand saveAssignmentItemCommand, searchItemsCommand, addItemCommand, cancelAddItemCommand;
        string searchText = string.Empty;
        Popup addItemPopUp;
        IEnumerable<Item> searchItems;
        int popUpWidth = 485;

        public ItemViewModel ()
        {
            assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel>();

            saveAssignmentItemCommand = new DelegateCommand (obj => {
                var item = obj as AssignmentItem;
                if (item != null && assignmentViewModel.SelectedAssignment != null) {
                    SaveAssignmentItem (assignmentViewModel.SelectedAssignment, item);
                }
            });

            searchItemsCommand = new DelegateCommand(_ =>
            {
                var items = new List<Item>();
                LoadItems().ContinueOnUIThread(obj =>
                    {
                        foreach (var item in Items)
                        {
                            if (item.Name.ToLower().StartsWith(SearchText) || item.Number.ToLower().StartsWith(SearchText))
                            {
                                items.Add(item);
                            }
                        }
                        SearchItems = items;
                    });
            });

            addItemCommand = new DelegateCommand(_ =>
            {
                addItemPopUp = new Popup();
                addItemPopUp.Height = Window.Current.Bounds.Height;
                addItemPopUp.Width = popUpWidth;
                AddItemFlyoutPanel flyoutpanel = new AddItemFlyoutPanel();
                flyoutpanel.Width = addItemPopUp.Width;
                flyoutpanel.Height = addItemPopUp.Height;
                addItemPopUp.Child = flyoutpanel;
                addItemPopUp.SetValue(Canvas.LeftProperty, Window.Current.Bounds.Width - popUpWidth);
                addItemPopUp.SetValue(Canvas.TopProperty, 0);
                addItemPopUp.IsOpen = true;
                SearchText = string.Empty;
                SearchItemsCommand.Invoke();
            });

            cancelAddItemCommand = new DelegateCommand(_ =>
                {
                    addItemPopUp.IsOpen = false;
                });
        }

        public DelegateCommand SaveAssignmentItemCommand
        {
            get { return saveAssignmentItemCommand; }
        }

        public DelegateCommand SearchItemsCommand
        {
            get { return searchItemsCommand; }
        }

        public DelegateCommand AddItemCommand
        {
            get { return addItemCommand; }
        }

        public DelegateCommand CancelAddItemCommand
        {
            get { return cancelAddItemCommand; }
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

        public string SearchText
        {
            get { return searchText; }
            set { searchText = value; OnPropertyChanged("SearchText"); }
        }

        public IEnumerable<Item> SearchItems
        {
            get { return searchItems; }
            set { searchItems = value; OnPropertyChanged("SearchItems"); }
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
