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
using FieldService.Android.Activities;
using FieldService.Android.Adapters;
using FieldService.Android.Utilities;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;

namespace FieldService.Android.Fragments {
    public class HistoryFragment : Fragment {
        ListView historyListView;
        TabHost tabHost;
        LocalActivityManager localManger;
        EditText searchText;
        HistoryListAdapter historySearchAdapter;
        HistoryViewModel historyViewModel;
        MenuViewModel menuViewModel;
        
        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView (inflater, container, savedInstanceState);

            historyViewModel = ServiceContainer.Resolve<HistoryViewModel> ();
            menuViewModel = ServiceContainer.Resolve<MenuViewModel> ();

            var view = inflater.Inflate (Resource.Layout.HistoryFragmentLayout, null, true);

            searchText = view.FindViewById<EditText> (Resource.Id.historySearchText);
            searchText.TextChanged += (sender, e) => {
                if (historySearchAdapter != null) {
                    historySearchAdapter.FilterItems (searchText.Text);
                    historySearchAdapter.NotifyDataSetChanged ();
                }
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
                    historySearchAdapter.Assignment = Assignment;
                    historyListView.Adapter = historySearchAdapter; 
                }
            };
            if (History != null) {
                historySearchAdapter = new HistoryListAdapter (Activity, Resource.Layout.HistoryItemLayout, History.OrderBy (a => a.Date).ToList ());
                historySearchAdapter.Assignment = Assignment;
                historyListView.Adapter = historySearchAdapter;
            }

            historyListView.ItemClick += (sender, e) => {
                var intent = new Intent (Activity, typeof (SummaryHistoryActivity));
                historyViewModel.SelectedAssignmentHistory = History.ElementAtOrDefault (e.Position);
                menuViewModel.MenuIndex = 0;
                StartActivity (intent);
            };

            return view;
        }

        /// <summary>
        /// the selected assignment
        /// </summary>
        public Assignment Assignment
        {
            get;
            set;
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

        /// <summary>
        /// list of history items.
        /// </summary>
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