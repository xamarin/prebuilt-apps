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
//    limitations under the License.using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FieldService.Android.Utilities;
using FieldService.Utilities;

namespace FieldService.Android {
    [Activity (Label = "Assignment Tabs", Theme = "@style/CustomHoloTheme")]
    public class AssignmentTabActivity : Activity{
        LocalActivityManager localManger;
        TabHost tabHost;
        public AssignmentTabActivity ()
        {
            ServiceContainer.Register<ISynchronizeInvoke> (() => new SynchronizeInvoke { Activity = this });
        }
        
        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            SetContentView (Resource.Layout.AssignmentsTabsLayout);

            tabHost = FindViewById<TabHost> (Resource.Id.assingmentTabHost);
            //In order to use tabs outside of a TabActivity I have to use this local activity manager and dispatch create the savedInstanceState
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

            if (savedInstanceState != null) {
                var currentTab = savedInstanceState.GetInt (Constants.CurrentTab);
                tabHost.CurrentTab = currentTab;
            } else {
                tabHost.CurrentTab = 0;
            }
        }

        protected override void OnResume ()
        {
            //have to clean up the local activity manager in on resume.
            localManger.DispatchResume ();
            base.OnResume ();
        }

        protected override void OnPause ()
        {
            //have to clean up the local activity manager in on pause.
            localManger.DispatchPause (IsFinishing);
            base.OnPause ();
        }

        protected override void OnStop ()
        {
            //have to clean up the local activity manager in on stop.
            localManger.DispatchStop ();
            base.OnStop ();
        }

        protected override void OnSaveInstanceState (Bundle outState)
        {
            base.OnSaveInstanceState (outState);
            outState.PutInt (Constants.CurrentTab, tabHost.CurrentTab);
        }
    }
}