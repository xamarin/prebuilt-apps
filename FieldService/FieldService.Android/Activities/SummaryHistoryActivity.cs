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
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FieldService.Android.Fragments;
using FieldService.Android.Utilities;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;
using Orientation = Android.Content.Res.Orientation;

namespace FieldService.Android.Activities {
    /// <summary>
    /// History Activity
    /// </summary>
    [Activity (Label = "Summary History", Theme = "@android:style/Theme.Holo")]
    public class SummaryHistoryActivity : Activity, PopupMenu.IOnMenuItemClickListener {
        
        readonly ItemViewModel itemViewModel;
        readonly LaborViewModel laborViewModel;
        readonly PhotoViewModel photoViewModel;
        readonly ExpenseViewModel expenseViewModel;
        readonly DocumentViewModel documentViewModel;
        readonly HistoryViewModel historyViewModel;
        readonly MenuViewModel menuViewModel;
        NavigationFragment navigationFragment;
        FrameLayout navigationFragmentContainer;
        TextView number, name, phone, address, items;
        Button addItems, addLabor, addExpense;
        LinearLayout mapButton, phoneButton, selectedAssignmentLayout;
        AssignmentHistory assignmentHistory;

        public SummaryHistoryActivity ()
        {
            historyViewModel = ServiceContainer.Resolve<HistoryViewModel> ();
            itemViewModel = ServiceContainer.Resolve<ItemViewModel> ();
            laborViewModel = ServiceContainer.Resolve<LaborViewModel> ();
            photoViewModel = ServiceContainer.Resolve<PhotoViewModel> ();
            expenseViewModel = ServiceContainer.Resolve<ExpenseViewModel> ();
            documentViewModel = ServiceContainer.Resolve<DocumentViewModel> ();
            menuViewModel = ServiceContainer.Resolve<MenuViewModel> ();

            assignmentHistory = historyViewModel.SelectedAssignmentHistory;
        }
        
        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            Window.RequestFeature (WindowFeatures.ActionBar);

            SetContentView (Resource.Layout.SummaryFragmentLayout);
            
            number = FindViewById<TextView> (Resource.Id.selectedAssignmentNumber);
            name = FindViewById<TextView> (Resource.Id.selectedAssignmentContactName);
            phone = FindViewById<TextView> (Resource.Id.selectedAssignmentPhoneNumber);
            address = FindViewById<TextView> (Resource.Id.selectedAssignmentAddress);
            items = FindViewById<TextView> (Resource.Id.selectedAssignmentTotalItems);
            addItems = FindViewById<Button> (Resource.Id.selectedAssignmentAddItem);
            addLabor = FindViewById<Button> (Resource.Id.selectedAssignmentAddLabor);
            addExpense = FindViewById<Button> (Resource.Id.selectedAssignmentAddExpense);
            navigationFragmentContainer = FindViewById<FrameLayout> (Resource.Id.navigationFragmentContainer);
            mapButton = FindViewById<LinearLayout> (Resource.Id.summaryMapIconLayout);
            phoneButton = FindViewById<LinearLayout> (Resource.Id.summaryPhoneIconLayout);
            selectedAssignmentLayout = FindViewById<LinearLayout> (Resource.Id.selectedAssignment);

            phoneButton.Click += (sender, e) => {
                AndroidExtensions.MakePhoneCall (this, phone.Text);
            };
            mapButton.Click += (sender, e) => {
                var navFragment = FragmentManager.FindFragmentById<NavigationFragment> (Resource.Id.navigationFragmentContainer);
                var index = Constants.HistoryNavigation.IndexOf ("Map");
                navFragment.SetNavigation (index);
            };

            selectedAssignmentLayout.SetBackgroundColor (Resources.GetColor (Resource.Color.historycolor));

            historyViewModel.LoadAssignmentFromHistory (assignmentHistory)
                .ContinueWith (_ => {
                    RunOnUiThread (() => {
                        //setting up default fragments
                        var transaction = FragmentManager.BeginTransaction ();
                        navigationFragment = new NavigationFragment ();
                        navigationFragment.Assignment = historyViewModel.PastAssignment;
                        transaction.SetTransition (FragmentTransit.FragmentOpen);
                        transaction.Replace (Resource.Id.navigationFragmentContainer, navigationFragment);
                        transaction.Commit ();
                        if (historyViewModel.PastAssignment != null) {
                            ActionBar.Title = string.Format ("#{0} {1} {2}", assignmentHistory.JobNumber, "Summary", historyViewModel.PastAssignment.StartDate.ToShortDateString ());

                            number.Text = historyViewModel.PastAssignment.Priority.ToString ();
                            name.Text = assignmentHistory.ContactName;
                            phone.Text = assignmentHistory.ContactPhone;
                            address.Text = string.Format ("{0}\n{1}, {2} {3}", assignmentHistory.Address, assignmentHistory.City, assignmentHistory.State, assignmentHistory.Zip);
                        }
                        navigationFragment.NavigationSelected += NavigationSelected;
                    });
                });

            items.Visibility =
                addItems.Visibility = ViewStates.Invisible;
            addLabor.Visibility = ViewStates.Gone;
            
            ActionBar.SetLogo (Resource.Drawable.XamarinTitle);
            ActionBar.SetBackgroundDrawable (Resources.GetDrawable (Resource.Drawable.actionbar));
            ActionBar.SetDisplayHomeAsUpEnabled (true);
        }

        protected override void OnResume ()
        {
            base.OnResume ();

            if (navigationFragment != null) {
                navigationFragment.NavigationSelected += NavigationSelected;
            }
            SetFrameFragment (menuViewModel.MenuIndex);
        }

        protected override void OnPause ()
        {
            base.OnPause ();
            if (navigationFragment != null) {
                navigationFragment.NavigationSelected -= NavigationSelected;
            }
        }

        private void NavigationSelected (object sender, EventArgs<int> e)
        {
            SetFrameFragment (e.Value);
            if (Resources.Configuration.Orientation == Orientation.Portrait) {
                navigationFragmentContainer.Visibility = ViewStates.Invisible;
            }
            menuViewModel.MenuIndex = e.Value;
            var screen = Constants.HistoryNavigation [e.Value];
            ActionBar.Title = string.Format ("#{0} {1} {2}", assignmentHistory.JobNumber, screen, historyViewModel.PastAssignment.StartDate.ToShortDateString ());
        }

        /// <summary>
        /// Sets the child fragment for when each navigation item is selected
        /// </summary>
        private void SetFrameFragment (int index)
        {
            var transaction = FragmentManager.BeginTransaction ();
            var screen = Constants.HistoryNavigation [index];
            switch (screen) {
                case "Summary": {
                        var fragment = new SummaryFragment ();
                        fragment.Assignment = historyViewModel.PastAssignment;
                        transaction.SetTransition (FragmentTransit.FragmentOpen);
                        transaction.Replace (Resource.Id.contentFrame, fragment);
                        transaction.Commit ();
                        items.Visibility =
                            addItems.Visibility = ViewStates.Invisible;
                        addExpense.Visibility =
                            addLabor.Visibility = ViewStates.Gone;
                    }
                    break;
                case "Map": {
                        var fragment = new MapFragment ();
                        transaction.SetTransition (FragmentTransit.FragmentOpen);
                        transaction.Replace (Resource.Id.contentFrame, fragment);
                        transaction.Commit ();
                        items.Visibility =
                            addItems.Visibility = ViewStates.Invisible;
                        addExpense.Visibility =
                            addLabor.Visibility = ViewStates.Gone;
                    }
                    break;
                case "Items": {
                        var fragment = new ItemFragment ();
                        fragment.Assignment = historyViewModel.PastAssignment;
                        itemViewModel.LoadAssignmentItemsAsync (historyViewModel.PastAssignment).ContinueWith (_ => {
                            RunOnUiThread (() => {
                                fragment.AssignmentItems = itemViewModel.AssignmentItems;
                                transaction.SetTransition (FragmentTransit.FragmentOpen);
                                transaction.Replace (Resource.Id.contentFrame, fragment);
                                transaction.Commit ();
                                items.Visibility = ViewStates.Visible;
                                addItems.Visibility = ViewStates.Invisible;
                                addExpense.Visibility =
                                    addLabor.Visibility = ViewStates.Gone;
                                items.Text = string.Format ("({0}) Items", historyViewModel.PastAssignment.TotalItems.ToString ());
                            });
                        });
                    }
                    break;
                case "Labor Hours": {
                        var fragment = new LaborHourFragment ();
                        laborViewModel.LoadLaborHoursAsync (historyViewModel.PastAssignment).ContinueWith (_ => {
                            RunOnUiThread (() => {
                                fragment.LaborHours = laborViewModel.LaborHours;
                                fragment.Assignment = historyViewModel.PastAssignment;
                                transaction.SetTransition (FragmentTransit.FragmentOpen);
                                transaction.Replace (Resource.Id.contentFrame, fragment);
                                transaction.Commit ();
                                items.Visibility = ViewStates.Visible;
                                addItems.Visibility = ViewStates.Invisible;
                                addExpense.Visibility =
                                    addLabor.Visibility = ViewStates.Gone;
                                items.Text = string.Format ("{0} hrs", historyViewModel.PastAssignment.TotalHours.TotalHours.ToString ("0.0"));
                            });
                        });
                    }
                    break;
                case "Confirmations": {
                        var fragment = new ConfirmationFragment ();
                        photoViewModel.LoadPhotosAsync (historyViewModel.PastAssignment).ContinueWith (_ => {
                            RunOnUiThread (() => {
                                fragment.Photos = photoViewModel.Photos;
                                fragment.Assignment = historyViewModel.PastAssignment;
                                transaction.SetTransition (FragmentTransit.FragmentOpen);
                                transaction.Replace (Resource.Id.contentFrame, fragment);
                                transaction.Commit ();
                                addLabor.Visibility =
                                    items.Visibility = ViewStates.Invisible;
                                addExpense.Visibility =
                                    addItems.Visibility = ViewStates.Gone;
                            });
                        });
                    }
                    break;
                case "Expenses": {
                        var fragment = new ExpenseFragment ();
                        expenseViewModel.LoadExpensesAsync (historyViewModel.PastAssignment).ContinueWith (_ => {
                            RunOnUiThread (() => {
                                fragment.Expenses = expenseViewModel.Expenses;
                                fragment.Assignment = historyViewModel.PastAssignment;
                                transaction.SetTransition (FragmentTransit.FragmentOpen);
                                transaction.Replace (Resource.Id.contentFrame, fragment);
                                transaction.Commit ();
                                items.Visibility = ViewStates.Visible;
                                addItems.Visibility = ViewStates.Invisible;
                                addExpense.Visibility =
                                    addLabor.Visibility = ViewStates.Gone;
                                items.Text = historyViewModel.PastAssignment.TotalExpenses.ToString ("$0.00");
                            });
                        });
                    }
                    break;
                case "Documents": {
                        var fragment = new DocumentFragment ();
                        documentViewModel.LoadDocumentsAsync ().ContinueWith (_ => {
                            RunOnUiThread (() => {
                                fragment.Documents = documentViewModel.Documents;
                                transaction.SetTransition (FragmentTransit.FragmentOpen);
                                transaction.Replace (Resource.Id.contentFrame, fragment);
                                transaction.Commit ();
                                items.Visibility =
                                    addItems.Visibility = ViewStates.Invisible;
                                addExpense.Visibility =
                                    addLabor.Visibility = ViewStates.Gone;
                            });
                        });
                    }
                    break;
                case "History": {
                        var fragment = new HistoryFragment ();
                        historyViewModel.LoadHistoryAsync (historyViewModel.PastAssignment).ContinueWith (_ => {
                            RunOnUiThread (() => {
                                fragment.History = historyViewModel.History;
                                fragment.Assignment = historyViewModel.PastAssignment;
                                transaction.SetTransition (FragmentTransit.FragmentOpen);
                                transaction.Replace (Resource.Id.contentFrame, fragment);
                                transaction.Commit ();
                                items.Visibility =
                                    addItems.Visibility = ViewStates.Invisible;
                                addExpense.Visibility =
                                    addLabor.Visibility = ViewStates.Gone;
                            });
                        });
                    }
                    break;
                default:
                    break;
            }
        }
        
        public override bool OnCreateOptionsMenu (IMenu menu)
        {
            if (Resources.Configuration.Orientation == Orientation.Portrait) {
                var inflater = MenuInflater;
                inflater.Inflate (Resource.Menu.SummaryMenu, menu);
                return true;
            }
            return false;
        }

        public override bool OnOptionsItemSelected (IMenuItem item)
        {
            switch (item.ItemId) {
                case Resource.Id.navigationMenu:
                    var popup = new PopupMenu (this, FindViewById<TextView> (Resource.Id.selectedAssignmentAnchor));
                    MenuInflater.Inflate (Resource.Menu.FragmentNavigationMenu, popup.Menu);
                    popup.SetOnMenuItemClickListener (this);
                    popup.Show ();
                    return true;
                default:
                    OnBackPressed ();
                    return true;
            }
        }

        public bool OnMenuItemClick (IMenuItem item)
        {
            switch (item.ItemId) {
                default:
                    navigationFragment.SetNavigation (Constants.HistoryNavigation.IndexOf (item.TitleFormatted.ToString ()));
                    return true;
            }
        }

        public override void OnBackPressed ()
        {
            Finish ();
        }
    }
}