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
        ListView historyListView;
        HistoryViewModel historyViewModel;
        TabHost tabHost;
        LocalActivityManager localManger;
        EditText searchText;
        HistoryListAdapter historySearchAdapter;

        public override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            historyViewModel = new HistoryViewModel ();
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView (inflater, container, savedInstanceState);

            var view = inflater.Inflate (Resource.Layout.HistoryFragmentLayout, null, true);

            searchText = view.FindViewById<EditText> (Resource.Id.historySearchText);
            searchText.TextChanged += (sender, e) => {
                historySearchAdapter.FilterItems (searchText.Text);
                };
            var clearSearch = view.FindViewById<ImageButton> (Resource.Id.historyClearSearch);
            clearSearch.Click += (sender, e) => searchText.Text = string.Empty;

            tabHost = view.FindViewById<TabHost> (Resource.Id.historyTabHost);
            historyListView = view.FindViewById<ListView> (Resource.Id.historyListView);

            localManger = new LocalActivityManager (Activity, true);
            localManger.DispatchCreate (savedInstanceState);
            tabHost.Setup (localManger);

            var dateIndicator = CreateTab ("DATE");
            TabHost.TabSpec dateTab = tabHost.NewTabSpec ("Date");
            dateTab.SetIndicator (dateIndicator);
            dateTab.SetContent (new TabContent (new TextView (tabHost.Context)));

            var callsIndicator = CreateTab ("CALLS");
            TabHost.TabSpec callsTab = tabHost.NewTabSpec ("Calls");
            callsTab.SetIndicator (callsIndicator);
            callsTab.SetContent (new TabContent(new TextView(tabHost.Context)));

            var assignmentIndicator = CreateTab ("ASSIGNMENTS");
            TabHost.TabSpec assignments = tabHost.NewTabSpec ("Assignments");
            assignments.SetIndicator (assignmentIndicator);
            assignments.SetContent (new TabContent (new TextView(tabHost.Context)));

            tabHost.AddTab (dateTab);
            tabHost.AddTab (callsTab);
            tabHost.AddTab (assignments);

            tabHost.TabChanged += (sender, e) => {
                if (History != null) {
                    switch (tabHost.CurrentTab) {
                        case 0:
                            historySearchAdapter = new HistoryListAdapter (Activity, Resource.Layout.HistoryItemLayout, History.OrderBy (h => h.Date).ToList ());
                            break;
                        case 1:
                            historySearchAdapter = new HistoryListAdapter (Activity, Resource.Layout.HistoryItemLayout, History.Where (h => h.Type == AssignmentHistoryType.PhoneCall).ToList ());
                            break;
                        default:
                            historySearchAdapter = new HistoryListAdapter (Activity, Resource.Layout.HistoryItemLayout, History.Where (h => h.Type == AssignmentHistoryType.Assignment).ToList ());
                            break;
                    }
                    historyListView.Adapter = historySearchAdapter;
                }
            };
            if (History != null) {
                historySearchAdapter = new HistoryListAdapter (Activity, Resource.Layout.HistoryItemLayout, History.OrderBy (a => a.Date).ToList ());
                historyListView.Adapter = historySearchAdapter;
            }

            historyListView.ItemClick += (sender, e) => {
                //load the history item as the selected assignment
                var history = historySearchAdapter.GetAssignmentHistory (e.Position);
                //set history and then go to summary for history
            };

            return view;
        }

        /// <summary>
        /// Method to create the tab indicator
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private View CreateTab (string text)
        {
            var view = LayoutInflater.From (tabHost.Context).Inflate (Resource.Layout.HistoryTabs, null);
            var textview = view.FindViewById<TextView> (Resource.Id.tabsText);
            textview.Text = text;
            return view;
        }

        public List<AssignmentHistory> History
        {
            get;
            set;
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

        /// <summary>
        /// Custom class to create tabs for the history fragment
        /// </summary>
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