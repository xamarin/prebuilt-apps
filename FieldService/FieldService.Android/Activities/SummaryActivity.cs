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
using System.Threading;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using FieldService.Android.Dialogs;
using FieldService.Android.Fragments;
using FieldService.Android.Utilities;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;
using Orientation = Android.Content.Res.Orientation;

namespace FieldService.Android {
    /// <summary>
    /// Activity for the summary screen
    /// </summary>
    [Activity (Label = "Summary", Theme = "@android:style/Theme.Holo")]
    public class SummaryActivity : BaseActivity, PopupMenu.IOnMenuItemClickListener {
        readonly ItemViewModel itemViewModel;
        readonly LaborViewModel laborViewModel;
        readonly PhotoViewModel photoViewModel;
        readonly ExpenseViewModel expenseViewModel;
        readonly DocumentViewModel documentViewModel;
        readonly HistoryViewModel historyViewModel;
        readonly AssignmentViewModel assignmentViewModel;
        readonly MenuViewModel menuViewModel;
        NavigationFragment navigationFragment;
        FrameLayout navigationFragmentContainer;
        TextView number, name, phone, address, items;
        Button addItems, addLabor, addExpense;
        ItemsDialog itemDialog;
        AddLaborDialog laborDialog;
        ExpenseDialog expenseDialog;
        LinearLayout mapButton, phoneButton;
        Assignment assignment;

        public SummaryActivity ()
        {
            assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();
            itemViewModel = ServiceContainer.Resolve<ItemViewModel> ();
            laborViewModel = ServiceContainer.Resolve<LaborViewModel> ();
            photoViewModel = ServiceContainer.Resolve<PhotoViewModel> ();
            expenseViewModel = ServiceContainer.Resolve<ExpenseViewModel> ();
            documentViewModel = ServiceContainer.Resolve<DocumentViewModel> ();
            historyViewModel = ServiceContainer.Resolve<HistoryViewModel> ();
            menuViewModel = ServiceContainer.Resolve<MenuViewModel> ();

            assignment = assignmentViewModel.SelectedAssignment;
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

            phoneButton.Click += (sender, e) => {
                AndroidExtensions.MakePhoneCall (this, phone.Text);
            };
            mapButton.Click += (sender, e) => {
                var navFragment = FragmentManager.FindFragmentById<NavigationFragment> (Resource.Id.navigationFragmentContainer);
                var index = Constants.Navigation.IndexOf ("Map");
                navFragment.SetNavigation (index);
            };

            if (assignment != null) {
                ActionBar.Title = string.Format ("#{0} {1} {2}", assignment.JobNumber, "Summary", assignment.StartDate.ToShortDateString ());

                number.Text = assignment.Priority.ToString ();
                name.Text = assignment.ContactName;
                phone.Text = assignment.ContactPhone;
                address.Text = string.Format ("{0}\n{1}, {2} {3}", assignment.Address, assignment.City, assignment.State, assignment.Zip);
            }

            //portrait mode, flip back and forth when selecting the navigation menu.
            if (Resources.Configuration.Orientation == Orientation.Landscape) {
                navigationFragmentContainer.Visibility = ViewStates.Visible;
            } else {
                navigationFragmentContainer.Visibility = ViewStates.Invisible;
            }

            //setting up default fragments
            var transaction = FragmentManager.BeginTransaction ();
            navigationFragment = new NavigationFragment ();
            navigationFragment.Assignment = assignment;
            transaction.SetTransition (FragmentTransit.FragmentOpen);
            transaction.Replace (Resource.Id.navigationFragmentContainer, navigationFragment);
            transaction.Commit ();

            items.Visibility =
                 addItems.Visibility = ViewStates.Invisible;
            addLabor.Visibility = ViewStates.Gone;

            addItems.Click += (sender, e) => {
                itemDialog = new ItemsDialog (this);
                itemDialog.Assignment = assignment;
                itemDialog.Show ();
            };
            addLabor.Click += (sender, e) => {
                laborDialog = new AddLaborDialog (this);
                laborDialog.Assignment = assignment;
                laborDialog.CurrentLabor = new Labor ();
                laborDialog.Show ();
            };
            addExpense.Click += (sender, e) => {
                //show add expense dialog;
                expenseDialog = new ExpenseDialog (this);
                expenseDialog.Assignment = assignment;
                expenseDialog.CurrentExpense = new Expense ();
                expenseDialog.Show ();
            };

            ActionBar.SetLogo (Resource.Drawable.XamarinTitle);
            ActionBar.SetBackgroundDrawable (Resources.GetDrawable (Resource.Drawable.actionbar));
            ActionBar.SetDisplayHomeAsUpEnabled (true);
        }

        private void NavigationSelected (object sender, EventArgs<int> e)
        {
            SetFrameFragment (e.Value);
            if (Resources.Configuration.Orientation == Orientation.Portrait) {
                navigationFragmentContainer.Visibility = ViewStates.Invisible;
            }
            menuViewModel.MenuIndex = e.Value;
            var screen = Constants.Navigation [e.Value];
            ActionBar.Title = string.Format ("#{0} {1} {2}", assignment.JobNumber, screen, assignment.StartDate.ToShortDateString ());
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

            if (itemDialog != null) {
                if (itemDialog.IsShowing) {
                    itemDialog.Dismiss ();
                }
            }

            if (laborDialog != null) {
                if (laborDialog.IsShowing) {
                    laborDialog.Dismiss ();
                }
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
                    navigationFragment.SetNavigation (Constants.Navigation.IndexOf (item.TitleFormatted.ToString ()));
                    return true;
            }
        }

        /// <summary>
        /// Sets the child fragment for when each navigation item is selected
        /// </summary>
        private void SetFrameFragment (int index)
        {
            assignment = assignmentViewModel.SelectedAssignment;
            var transaction = FragmentManager.BeginTransaction ();
            var screen = Constants.Navigation [index];
            switch (screen) {
                case "Summary": {
                        var fragment = new SummaryFragment ();
                        fragment.Assignment = assignment;
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
                        fragment.Assignment = assignment;
                        itemViewModel.LoadAssignmentItemsAsync (assignment).ContinueWith (_ => {
                            RunOnUiThread (() => {
                                fragment.AssignmentItems = itemViewModel.AssignmentItems;
                                transaction.SetTransition (FragmentTransit.FragmentOpen);
                                transaction.Replace (Resource.Id.contentFrame, fragment);
                                transaction.Commit ();
                                items.Visibility =
                                    addItems.Visibility = ViewStates.Visible;
                                addExpense.Visibility =
                                    addLabor.Visibility = ViewStates.Gone;
                                items.Text = string.Format ("({0}) Items", assignment.TotalItems.ToString ());
                            });
                        });
                    }
                    break;
                case "Labor Hours": {
                        var fragment = new LaborHourFragment ();
                        laborViewModel.LoadLaborHoursAsync (assignment).ContinueWith (_ => {
                            RunOnUiThread (() => {
                                fragment.LaborHours = laborViewModel.LaborHours;
                                fragment.Assignment = assignment;
                                transaction.SetTransition (FragmentTransit.FragmentOpen);
                                transaction.Replace (Resource.Id.contentFrame, fragment);
                                transaction.Commit ();
                                addLabor.Visibility =
                                    items.Visibility = ViewStates.Visible;
                                addExpense.Visibility =
                                    addItems.Visibility = ViewStates.Gone;
                                items.Text = string.Format ("{0} hrs", assignment.TotalHours.TotalHours.ToString ("0.0"));
                            });
                        });
                    }
                    break;
                case "Confirmations": {
                        var fragment = new ConfirmationFragment ();
                        photoViewModel.LoadPhotosAsync (assignment).ContinueWith (_ => {
                            RunOnUiThread (() => {
                                fragment.Photos = photoViewModel.Photos;
                                fragment.Assignment = assignment;
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
                        expenseViewModel.LoadExpensesAsync (assignment).ContinueWith (_ => {
                            RunOnUiThread (() => {
                                fragment.Expenses = expenseViewModel.Expenses;
                                fragment.Assignment = assignment;
                                transaction.SetTransition (FragmentTransit.FragmentOpen);
                                transaction.Replace (Resource.Id.contentFrame, fragment);
                                transaction.Commit ();
                                addLabor.Visibility =
                                    addItems.Visibility = ViewStates.Gone;
                                items.Visibility =
                                    addExpense.Visibility = ViewStates.Visible;
                                items.Text = assignment.TotalExpenses.ToString ("$0.00");
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
                        historyViewModel.LoadHistoryAsync (assignment).ContinueWith (_ => {
                            RunOnUiThread (() => {
                                fragment.History = historyViewModel.History;
                                fragment.Assignment = assignment;
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

        public override void OnBackPressed ()
        {
            Finish ();
        }
    }
}