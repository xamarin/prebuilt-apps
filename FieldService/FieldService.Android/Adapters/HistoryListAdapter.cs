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
//    limitations under the License.using System;using System;
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

namespace FieldService.Android.Adapters
{
	public class HistoryListAdapter : ArrayAdapter<AssignmentHistory>
	{
		IList<AssignmentHistory> assignments;
		IList<AssignmentHistory> non_filtered;
		int resourceId;

		public Assignment Assignment { get; set; }

		public override int Count {
			get {
				return assignments.Count;
			}
		}

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
			if (assignments != null && assignments.Count > position)
				assignment = assignments [position];

			LayoutInflater inflator = (LayoutInflater)Context.GetSystemService (Context.LayoutInflaterService);
			var view = inflator.Inflate (resourceId, null);

			var title = view.FindViewById<TextView> (Resource.Id.historyItemJobTitle);
			var jobNumber = view.FindViewById<TextView> (Resource.Id.historyItemJobNumber);
			var phoneNumber = view.FindViewById<TextView> (Resource.Id.historyItemPhoneNumber);
			var date = view.FindViewById<TextView> (Resource.Id.historyItemDate);
			var mapIcon = view.FindViewById<ImageView> (Resource.Id.historyItemMapImage);
			var address = view.FindViewById<TextView> (Resource.Id.historyItemAddress);
			var phoneImage = view.FindViewById<ImageView> (Resource.Id.historyItemPhoneImage);
			var phoneIcon = view.FindViewById<ImageView> (Resource.Id.historyItemPhoneIcon);
            

			if (assignment != null) {
				if (assignment.Type == AssignmentHistoryType.Assignment) {
					phoneImage.Visibility = ViewStates.Gone;
					mapIcon.Visibility = jobNumber.Visibility = ViewStates.Visible;
					address.Text = string.Format ("{0}\n{1}, {2}{3}", assignment.Address, assignment.City, assignment.State, assignment.Zip);
					jobNumber.Text = assignment.JobNumber;
				} else {
					phoneImage.Visibility = ViewStates.Visible;
					mapIcon.Visibility = ViewStates.Invisible;
					jobNumber.Visibility = ViewStates.Gone;
					address.Text = string.Format ("Length: {0}  {1}", assignment.CallLength.ToString (@"hh\:mm\:ss"), assignment.CallDescription);
				}
				title.Text = assignment.CompanyName;
				date.Text = assignment.Date.ToString ("d");
				phoneNumber.Text = assignment.ContactPhone;

				if (Assignment != null)
					title.Focusable = Assignment.IsHistory;
			} else {
				phoneIcon.Visibility = phoneImage.Visibility = mapIcon.Visibility = ViewStates.Gone;
			}

			return view;
		}

		public void FilterItems (string filter)
		{
			var filtered = new List<AssignmentHistory> ();

			foreach (var item in non_filtered) {
				if (item.CompanyName.ToLower ().StartsWith (filter) || item.JobNumber.ToLower ().StartsWith (filter))
					filtered.Add (item);
			}

			this.assignments = filtered;
		}

		public AssignmentHistory GetAssignmentHistory (int position)
		{
			return assignments.ElementAtOrDefault (position);
		}
	}
}