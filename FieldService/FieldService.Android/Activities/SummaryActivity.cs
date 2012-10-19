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
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;

namespace FieldService.Android {
    [Activity (Label = "Summary", Theme = "@style/CustomHoloTheme")]
    public class SummaryActivity : Activity {
        AssignmentViewModel assignmentViewModel;
        NavigationFragment navigationFragment;
        ContentFragment contentFragment;

        public SummaryActivity ()
        {
            assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();
        }

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            SetContentView (Resource.Layout.SummaryLayout);
            var title = FindViewById<TextView> (Resource.Id.summaryAssignmentTitle);
            navigationFragment = FragmentManager.FindFragmentById<NavigationFragment> (Resource.Id.navigationFragment);
            contentFragment = FragmentManager.FindFragmentById<ContentFragment> (Resource.Id.contentFragment);
            Assignment assignment = null;
            if (bundle != null) {
                var index = bundle.GetInt ("index", -1);
                if (index != -1) {
                    assignment = assignmentViewModel.Assignments [index];
                } else {
                    assignment = assignmentViewModel.ActiveAssignment;
                }
            }

            navigationFragment.Assignment = assignment;

            if (assignment != null) {
                title.Text = string.Format ("#{0} {1} {2}", assignment.JobNumber, assignment.Title, assignment.StartDate.ToShortDateString ());
            }
        }
    }
}