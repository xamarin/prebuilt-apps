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

using System;
using System.ComponentModel;
using Android.App;
using Android.Content;
using Android.GoogleMaps;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using FieldService.Android.Utilities;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;

namespace FieldService.Android {
    /// <summary>
    /// Activity for the tab bar between "Assignments" and "Map Overview"
    /// </summary>
    [Activity (Label = "Assignment Tabs", Theme = "@style/CustomHoloTheme")]
    public class AssignmentTabActivity : BaseActivity {
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

            TabHost.TabSpec assignmentsSpec = tabHost.NewTabSpec ("list");
            Intent assignmentIntent = new Intent (this, typeof (AssignmentsActivity));
            assignmentsSpec.SetContent (assignmentIntent);
            assignmentsSpec.SetIndicator ("list");

            TabHost.TabSpec mapViewSpec = tabHost.NewTabSpec ("map");
            Intent mapViewIntent = new Intent (this, typeof (MapViewActivity));
            mapViewSpec.SetContent (mapViewIntent);
            mapViewSpec.SetIndicator ("map");

            tabHost.AddTab (assignmentsSpec);
            tabHost.AddTab (mapViewSpec);

            tabHost.TabChanged += (sender, e) => {
                if (tabHost.CurrentTab == 0) {
                    MapData = null;
                }
            };

            try {
                if (savedInstanceState != null) {
                    if (savedInstanceState.ContainsKey (Constants.CurrentTab)) {
                        var currentTab = savedInstanceState.GetInt (Constants.CurrentTab, 0);
                        tabHost.CurrentTab = currentTab;
                    } else {
                        tabHost.CurrentTab = 0;
                    }
                    if (savedInstanceState.ContainsKey ("mapData")) {
                        MapData = (MapDataWrapper)savedInstanceState.GetSerializable ("mapData");
                    } else {
                        MapData = null;
                    }
                } else {
                    MapData = null;
                    tabHost.CurrentTab = 0;
                }
            } catch (Exception) {
                tabHost.CurrentTab = 0;
            }            
        }

        public TabHost TabHost
        {
            get { return tabHost; }
        }

        public MapDataWrapper MapData
        {
            get;
            set;
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
            localManger.SaveInstanceState ();
            outState.PutSerializable ("mapData", MapData);
            outState.PutInt (Constants.CurrentTab, (int)tabHost.CurrentTab);
            base.OnSaveInstanceState (outState);
        }

        public class MapDataWrapper : Java.Lang.Object, Java.IO.ISerializable {
            public int Zoom { get; set; }
            public View OverlayBubble { get; set; }
            public GeoPoint OverlayPoint { get; set; }
        }
    }
}