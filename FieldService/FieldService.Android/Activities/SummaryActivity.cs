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
using Android.Widget;
using FieldService.Android.Fragments;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;
using Orientation = Android.Content.Res.Orientation;

namespace FieldService.Android {
    [Activity (Label = "Summary", Theme = "@style/CustomHoloTheme")]
    public class SummaryActivity : Activity {
        AssignmentViewModel assignmentViewModel;
        NavigationFragment navigationFragment;
        SummaryFragment contentFragment;
        Assignment assignment;

        public SummaryActivity ()
        {
            assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();
        }

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            SetContentView (Resource.Layout.SummaryFragmentLayout);

            assignment = null;
            if (Intent != null) {
                var index = Intent.GetIntExtra ("index", -1);
                if (index != -1) {
                    assignment = assignmentViewModel.Assignments [index];
                } else {
                    assignment = assignmentViewModel.ActiveAssignment;
                }
            }

            var title = FindViewById<TextView> (Resource.Id.summaryAssignmentTitle);
            if (Resources.Configuration.Orientation == Orientation.Landscape) {
                navigationFragment = FragmentManager.FindFragmentById<NavigationFragment> (Resource.Id.navigationFragment);
                navigationFragment.Assignment = assignment;
            }
            contentFragment = FragmentManager.FindFragmentById<SummaryFragment> (Resource.Id.contentFragment);
            contentFragment.Assignment = assignment;

            if (assignment != null) {
                title.Text = string.Format ("#{0} {1} {2}", assignment.JobNumber, assignment.Title, assignment.StartDate.ToShortDateString ());
            }
        }

        protected override void OnResume ()
        {
            base.OnResume ();
            contentFragment.Assignment = assignment;
            if (navigationFragment != null) {
                navigationFragment.Assignment = assignment;
            }
        }
    }
}