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
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using FieldService.Android.Fragments;
using FieldService.Android.Utilities;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;
using Orientation = Android.Content.Res.Orientation;

namespace FieldService.Android {
    [Activity (Label = "Summary", Theme = "@style/CustomHoloTheme")]
    public class SummaryActivity : Activity {
        AssignmentViewModel assignmentViewModel;
        NavigationFragment navigationFragment;
        Assignment assignment = null;
        FrameLayout navigationFragmentContainer;
        TextView number,
            name,
            phone,
            address,
            items;
        Button addItems,
            addLabor;
        ImageButton navigationMenu;
        int assignmentIndex = 0,
            navigationIndex = 0;

        public SummaryActivity ()
        {
            assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();
            
        }

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            SetContentView (Resource.Layout.SummaryFragmentLayout);

            if (Intent != null) {
                assignmentIndex = Intent.GetIntExtra ("index", -1);
                if (assignmentIndex != -1) {
                    assignment = assignmentViewModel.Assignments [assignmentIndex];
                } else {
                    assignment = assignmentViewModel.ActiveAssignment;
                }
            }
            
            var title = FindViewById<TextView> (Resource.Id.summaryAssignmentTitle);
            number = FindViewById<TextView> (Resource.Id.selectedAssignmentNumber);
            name = FindViewById<TextView> (Resource.Id.selectedAssignmentContactName);
            phone = FindViewById<TextView> (Resource.Id.selectedAssignmentPhoneNumber);
            address = FindViewById<TextView> (Resource.Id.selectedAssignmentAddress);
            items = FindViewById<TextView> (Resource.Id.selectedAssignmentTotalItems);
            addItems = FindViewById<Button> (Resource.Id.selectedAssignmentAddItem);
            addLabor = FindViewById<Button> (Resource.Id.selectedAssignmentAddLabor);
            navigationMenu = FindViewById<ImageButton> (Resource.Id.navigationMenu);
            navigationFragmentContainer = FindViewById<FrameLayout> (Resource.Id.navigationFragmentContainer);

            if (assignment != null) {
                title.Text = string.Format ("#{0} {1} {2}", assignment.JobNumber, assignment.Title, assignment.StartDate.ToShortDateString ());

                number.Text = assignment.Priority.ToString ();
                name.Text = assignment.ContactName;
                phone.Text = assignment.ContactPhone;
                address.Text = string.Format ("{0}\n{1}, {2} {3}", assignment.Address, assignment.City, assignment.State, assignment.Zip);
            }

            //portrait mode, flip back and forth when selecting the navigation menu.
            if (navigationMenu != null) {
                navigationMenu.Click += (sender, e) => {
                    if (navigationFragmentContainer.Visibility == ViewStates.Visible) {
                        navigationFragmentContainer.Visibility = ViewStates.Invisible;
                    } else {
                        navigationFragmentContainer.Visibility = ViewStates.Visible;
                    }
                };
            } else {
                navigationFragmentContainer.Visibility = ViewStates.Visible;
            }

            //setting up default fragments

            var transaction = FragmentManager.BeginTransaction ();
            var summaryFragment = new SummaryFragment ();
            summaryFragment.Assignment = assignment;
            navigationFragment = new NavigationFragment ();
            transaction.SetTransition (FragmentTransit.FragmentOpen);
            transaction.Add (Resource.Id.contentFrame, summaryFragment);
            transaction.Add (Resource.Id.navigationFragmentContainer, navigationFragment);
            transaction.Commit ();

            items.Visibility =
                 addItems.Visibility = ViewStates.Invisible;
            addLabor.Visibility = ViewStates.Gone;

            if (bundle != null && bundle.ContainsKey (Constants.BUNDLE_INDEX)) {
                navigationIndex = bundle.GetInt (Constants.BUNDLE_INDEX, 0);
            }
        }

        protected override void OnSaveInstanceState (Bundle outState)
        {
            outState.PutInt (Constants.BUNDLE_INDEX, navigationIndex);
            base.OnSaveInstanceState (outState);
        }

        private void NavigationSelected (object sender, EventArgs<int> e)
        {
            SetFrameFragment (e.Value);
            if (navigationMenu != null) {
                navigationFragmentContainer.Visibility = ViewStates.Invisible;
            }
            navigationIndex = e.Value;
        }

        protected override void OnResume ()
        {
            base.OnResume ();
            if (navigationFragment != null) {
                navigationFragment.NavigationSelected += NavigationSelected;
            }
            if (navigationIndex != 0) {
                SetFrameFragment (navigationIndex);
            }
        }

        protected override void OnPause ()
        {
            base.OnPause ();
            if (navigationFragment != null) {
                navigationFragment.NavigationSelected -= NavigationSelected;
            }
        }

        private void SetFrameFragment (int index)
        {
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
                        addLabor.Visibility = ViewStates.Gone;
                    }
                    break;
                case "Map": {
                        var fragment = new MapFragment ();
                        fragment.AssignmentIndex = assignmentIndex;
                        transaction.SetTransition (FragmentTransit.FragmentOpen);
                        transaction.Replace (Resource.Id.contentFrame, fragment);
                        transaction.Commit ();
                        items.Visibility =
                            addItems.Visibility = ViewStates.Invisible;
                        addLabor.Visibility = ViewStates.Gone;
                    }
                    break;
                case "Items": {
                        var itemViewModel = ServiceContainer.Resolve<ItemViewModel> ();
                        var fragment = new ItemFragment ();
                        fragment.Assignment = assignment;
                        itemViewModel.LoadAssignmentItems (assignment).ContinueOnUIThread (_ => {
                            fragment.AssignmentItems = itemViewModel.AssignmentItems;
                            transaction.SetTransition (FragmentTransit.FragmentOpen);
                            transaction.Replace (Resource.Id.contentFrame, fragment);
                            transaction.Commit ();
                            items.Visibility =
                                addItems.Visibility = ViewStates.Visible;
                            addLabor.Visibility = ViewStates.Gone;
                            items.Text = string.Format ("({0}) Items", assignment.TotalItems.ToString ());
                        });
                    }
                    break;
                case "Labor Hours": {
                        var laborViewModel = ServiceContainer.Resolve<LaborViewModel> ();
                        var fragment = new LaborHoursFragment ();
                        laborViewModel.LoadLaborHours(assignment).ContinueOnUIThread (_ => {
                            fragment.LaborHours = laborViewModel.LaborHours;
                            transaction.SetTransition (FragmentTransit.FragmentOpen);
                            transaction.Replace (Resource.Id.contentFrame, fragment);
                            transaction.Commit ();
                            addLabor.Visibility =
                                items.Visibility = ViewStates.Visible;
                            addItems.Visibility = ViewStates.Gone;
                            items.Text = string.Format ("{0} hrs", assignment.TotalHours.ToString ("0.0"));
                        });
                    }
                    break;
                default:
                    break;
            }
        }
    }
}