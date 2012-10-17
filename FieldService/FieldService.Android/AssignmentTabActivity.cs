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

namespace FieldService.Android {
    [Activity (Label = "Assignment Tabs", Theme = "@style/CustomHoloTheme")]
    public class AssignmentTabActivity : Activity{
        LocalActivityManager localManger;
        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            SetContentView (Resource.Layout.AssignmentsTabsLayout);

            var tabHost = FindViewById<TabHost> (Resource.Id.assingmentTabHost);
            localManger = new LocalActivityManager (this, true);
            localManger.DispatchCreate (savedInstanceState);
            tabHost.Setup (localManger);

            TabHost.TabSpec assignmentsSpec = tabHost.NewTabSpec ("Priority");
            Intent assignmentIntent = new Intent (this, typeof (AssignmentsActivity));
            assignmentsSpec.SetContent (assignmentIntent);
            assignmentsSpec.SetIndicator ("Priority");

            TabHost.TabSpec mapViewSpec = tabHost.NewTabSpec ("Map View");
            Intent mapViewIntent = new Intent (this, typeof (MapViewActivity));
            mapViewSpec.SetContent (mapViewIntent);
            mapViewSpec.SetIndicator ("Map View");

            tabHost.AddTab (assignmentsSpec);
            tabHost.AddTab (mapViewSpec);

            tabHost.CurrentTab = 0;
        }

        protected override void OnResume ()
        {
            localManger.DispatchResume ();
            base.OnResume ();
        }

        protected override void OnPause ()
        {
            localManger.DispatchPause (IsFinishing);
            base.OnPause ();
        }
    }
}