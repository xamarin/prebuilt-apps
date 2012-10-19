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
using Orientation = Android.Content.Res.Orientation;

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

            Assignment assignment = null;
            if (bundle != null) {
                var index = bundle.GetInt ("index", -1);
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
            contentFragment = FragmentManager.FindFragmentById<ContentFragment> (Resource.Id.contentFragment);

            if (assignment != null) {
                title.Text = string.Format ("#{0} {1} {2}", assignment.JobNumber, assignment.Title, assignment.StartDate.ToShortDateString ());
            }
        }
    }
}