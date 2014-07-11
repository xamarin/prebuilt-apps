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
using FieldService.Android.Utilities;

namespace FieldService.Android.Fragments {
    /// <summary>
    /// Fragment for the maps section
    /// </summary>
    public class MapFragment : Fragment {
        LocalActivityManager localManager;

        public override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            //Like the tabs issue, we have to create a local activity manager to enable us to use a mapview outside of mapactivity.
            //credits go to BahaiResearch.com on stackoverflow http://stackoverflow.com/questions/5109336/mapview-in-a-fragment-honeycomb
            localManager = new LocalActivityManager (Activity, true);
            localManager.DispatchCreate (savedInstanceState);
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var intent = new Intent (Activity, typeof (MapFragmentActivity));
            //pass the index of the assignment through to the actual map activity
            var window = localManager.StartActivity ("MapFragmentActivity", intent);
            View currentView = window.DecorView;
            currentView.Visibility = ViewStates.Visible;
            currentView.FocusableInTouchMode = true;
            ((ViewGroup)currentView).DescendantFocusability = DescendantFocusability.AfterDescendants;
            return currentView;
        }

        /// <summary>
        /// Resume the LocalActivityManager
        /// </summary>
        public override void OnResume ()
        {
            base.OnResume ();
            localManager.DispatchResume ();
        }

        /// <summary>
        /// Pause the LocalActivityManager
        /// </summary>
        public override void OnPause ()
        {
            base.OnPause ();
            localManager.DispatchPause (Activity.IsFinishing);
        }

        /// <summary>
        /// Stop the LocalActivityManager
        /// </summary>
        public override void OnStop ()
        {
            base.OnStop ();
            localManager.DispatchStop ();
        }
    }
}