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
using FieldService.Data;

namespace FieldService.Android.Adapters {
    public class HistoryListAdapter : ArrayAdapter<AssignmentHistory> {
        IList<AssignmentHistory> assignments;
        IList<AssignmentHistory> non_filtered;
        int resourceId;

        public HistoryListAdapter (Context context, int resourceId, List<AssignmentHistory> assignments)
            : base (context, resourceId, assignments)
        {
            this.assignments = new List<AssignmentHistory> (assignments);
            non_filtered = new List<AssignmentHistory> (assignments);
            this.resourceId = resourceId;
        }

        public override View GetView (int position, View convertView, ViewGroup parent)
        {
            AssignmentHistory assignment = null;
            if (assignments != null && assignments.Count > position) {
                assignment = assignments [position];
            }

            LayoutInflater inflator = (LayoutInflater)Context.GetSystemService (Context.LayoutInflaterService);
            var view = inflator.Inflate (resourceId, null);

            var title = view.FindViewById<TextView> (Resource.Id.historyItemJobTitle);
            var jobNumber = view.FindViewById<TextView> (Resource.Id.historyItemJobNumber);
            var phoneNumber = view.FindViewById<TextView> (Resource.Id.historyItemPhoneNumber);
            var date = view.FindViewById<TextView> (Resource.Id.historyItemDate);
            var mapIcon = view.FindViewById<ImageView> (Resource.Id.historyItemMapImage);
            var address = view.FindViewById<TextView> (Resource.Id.historyItemAddress);
            var phoneIcon = view.FindViewById<ImageView> (Resource.Id.historyItemPhoneImage);

            if (assignment.Type == AssignmentHistoryType.Assignment) {
                phoneIcon.Visibility = ViewStates.Gone;
                mapIcon.Visibility = 
                    jobNumber.Visibility = ViewStates.Visible;
                address.Text = string.Format ("{0}\n{1}, {2}{3}", assignment.Address, assignment.City, assignment.State, assignment.Zip);
                jobNumber.Text = assignment.JobNumber;
            } else {
                phoneIcon.Visibility = ViewStates.Visible;
                mapIcon.Visibility =
                    jobNumber.Visibility = ViewStates.Gone;
                address.Text = string.Format ("Length: {0}  {1}", assignment.CallLength.ToString (@"hh\:mm\:ss"), assignment.CallDescription);
            }
            title.Text = assignment.Title;
            date.Text = assignment.Date.ToString ("d");
            phoneNumber.Text = assignment.ContactPhone;

            return view;
        }

        public void FilterItems (string filter)
        {
            var filtered = new List<AssignmentHistory> ();

            foreach (var item in non_filtered) {
                if (item.Title.ToLower ().StartsWith (filter) || item.JobNumber.ToLower ().StartsWith (filter)) {
                    filtered.Add (item);
                }
            }

            assignments = filtered;
        }

        public AssignmentHistory GetAssignmentHistory (int position)
        {
            return assignments.ElementAtOrDefault (position);
        }
    }
}