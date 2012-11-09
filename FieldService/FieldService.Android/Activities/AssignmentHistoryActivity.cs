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
using FieldService.Android.Adapters;
using FieldService.Android.Fragments;
using FieldService.Utilities;
using FieldService.ViewModels;

namespace FieldService.Android {
    [Activity (Label = "Activity History", Theme = "@style/CustomHoloTheme")]
    public class AssignmentHistoryActivity : Activity {
        ListView historyListView;
        HistoryViewModel historyViewModel;
        SummaryActivity summaryActivity;

        public AssignmentHistoryActivity ()
        {
            historyViewModel = ServiceContainer.Resolve<HistoryViewModel> ();
            summaryActivity = ServiceContainer.Resolve<SummaryActivity> ();
        }

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            SetContentView (Resource.Layout.AssignmentHistoryLayout);

            historyListView = FindViewById<ListView> (Resource.Id.historyListView);
            if (summaryActivity != null) {
                historyViewModel.LoadHistory (summaryActivity.Assignment).ContinueOnUIThread (_ => {
                        historyListView.Adapter = new HistoryListAdapter (this, Resource.Layout.HistoryItemLayout, historyViewModel.History.OrderBy (a => a.Date).ToList ());
                    });

                historyListView.ItemClick += (sender, e) => {
                    //load the history item as the selected assignment
                    var history = ((HistoryListAdapter)historyListView.Adapter).GetAssignmentHistory (e.Position);
                    //set history and then go to summary for history
                };
            }
        }
    }
}