using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using FieldService.Android.Adapters;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;

namespace FieldService.Android.Fragments {
    public class HistoryFragment : Fragment {
        HistoryViewModel historyViewModel;
        TabHost tabHost;
        LocalActivityManager localManger;
        EditText searchText;

        public override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            historyViewModel = ServiceContainer.Resolve<HistoryViewModel> ();
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView (inflater, container, savedInstanceState);

            var view = inflater.Inflate (Resource.Layout.HistoryFragmentLayout, null, true);

            searchText = view.FindViewById<EditText> (Resource.Id.historySearchText);
            searchText.TextChanged += (sender, e) => {
                };
            var clearSearch = view.FindViewById<ImageButton> (Resource.Id.historyClearSearch);
            clearSearch.Click += (sender, e) => searchText.Text = string.Empty;

            tabHost = view.FindViewById<TabHost> (Resource.Id.historyTabHost);

            localManger = new LocalActivityManager (Activity, true);
            localManger.DispatchCreate (savedInstanceState);
            tabHost.Setup (localManger);

            var indicator = CreateTab ("DATE");
            TabHost.TabSpec dateTab = tabHost.NewTabSpec ("Date");
            dateTab.SetIndicator (indicator);
            dateTab.SetContent (new TabContent (new TextView (tabHost.Context)));
            //var intent = new Intent (Activity, typeof (AssignmentHistoryActivity));
            //dateTab.SetContent (intent);

            TabHost.TabSpec callsTab = tabHost.NewTabSpec ("Calls");
            callsTab.SetIndicator ("CALLS");
            callsTab.SetContent (new Intent (Activity, typeof (AssignmentHistoryActivity)));

            TabHost.TabSpec assignments = tabHost.NewTabSpec ("Assignments");
            assignments.SetIndicator ("ASSIGNMENTS");
            assignments.SetContent (new Intent (Activity, typeof (AssignmentHistoryActivity)));

            tabHost.AddTab (dateTab);
            tabHost.AddTab (callsTab);
            tabHost.AddTab (assignments);

            return view;
        }

        private View CreateTab (string text)
        {
            var view = LayoutInflater.From (tabHost.Context).Inflate (Resource.Layout.HistoryTabs, null);
            var textview = view.FindViewById<TextView> (Resource.Id.tabsText);
            textview.Text = text;
            return view;
        }

        public override void OnResume ()
        {
            //have to clean up the local activity manager in on resume.
            localManger.DispatchResume ();
            base.OnResume ();
        }

        public override void OnPause ()
        {
            //have to clean up the local activity manager in on pause.
            localManger.DispatchPause (Activity.IsFinishing);
            base.OnPause ();
        }

        public override void OnStop ()
        {
            //have to clean up the local activity manager in on stop.
            localManger.DispatchStop ();
            base.OnStop ();
        }

        internal class TabContent : Java.Lang.Object, TabHost.ITabContentFactory {

            View view;
            public TabContent (View view)
            {
                this.view = view;
            }

            public View CreateTabContent (string tag)
            {
                return view;
            }
        }
    }
}