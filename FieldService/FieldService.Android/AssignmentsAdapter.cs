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

using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FieldService.Data;
using System.Text;

namespace FieldService.Android {
    public class AssignmentsAdapter : ArrayAdapter<Assignment> {

        IList<Assignment> assignments;
        int resourceId;
        public AssignmentsAdapter (Context context, int resourceId, IList<Assignment> assignments)
            : base (context, resourceId, assignments)
        {
            this.assignments = assignments;
            this.resourceId = resourceId;
        }
                
        public override View GetView (int position, View convertView, ViewGroup parent)
        {
            Assignment assignment = null;
            var view = convertView;
            if (view == null) {
                LayoutInflater inflator = (LayoutInflater)Context.GetSystemService (Context.LayoutInflaterService);
                view = inflator.Inflate (resourceId, null);
            }

            if (assignments.Count > position) {
                assignment = assignments [position];
            }
            if(assignment == null)
            {
                return view;   
            }

            view.SetBackgroundColor (Context.Resources.GetColor (Resource.Color.assignmentoffwhite));
            var number = view.FindViewById<TextView> (Resource.Id.assignmentItemNumber);
            var job = view.FindViewById<TextView> (Resource.Id.assignmentJob);
            var name = view.FindViewById<TextView> (Resource.Id.assignmentName);
            var phone = view.FindViewById<TextView> (Resource.Id.assignmentPhone);
            var address = view.FindViewById<TextView> (Resource.Id.assignmentAddress);
            var buttonLayout = view.FindViewById<LinearLayout> (Resource.Id.assignmentButtonLayout);
            var timerLayout = view.FindViewById<LinearLayout> (Resource.Id.assignmentTimerLayout);
            var spinner = view.FindViewById<Spinner> (Resource.Id.assignmentStatus);
            var spinnerImage = view.FindViewById<ImageView> (Resource.Id.assignmentStatusImage);
            var timer = view.FindViewById<Button> (Resource.Id.assignmentTimer);
            var timerText = view.FindViewById<TextView> (Resource.Id.assignmentTimerText);
            var accept = view.FindViewById<Button> (Resource.Id.assignmentAccept);
            var decline = view.FindViewById<Button> (Resource.Id.assignmentDecline);

            if (assignment.Status == AssignmentStatus.New) {
                buttonLayout.Visibility = ViewStates.Visible;
                timerLayout.Visibility = ViewStates.Gone;
            } else {
                buttonLayout.Visibility = ViewStates.Gone;
                timerLayout.Visibility = ViewStates.Visible;
                timerText.Text = string.Format ("{0} hr {1} min\n{2}", 10, 10, assignment.Status == AssignmentStatus.Active ? "RESUME" : "START");

                List<string> status = new List<string> ();
                foreach (var item in Enum.GetValues (typeof (AssignmentStatus))) {
                    status.Add (item.ToString ());
                }
                spinner.Adapter = new ArrayAdapter<string> (Context, Android.Resource.Layout.SimpleSpinnerItem, status);
                spinner.ItemSelected += (sender, e) => {
                    var selected = status.ElementAtOrDefault (e.Position);
                    if (selected != null) {
                        switch (selected) {
                            case "Enroute":
                                view.SetBackgroundColor (Context.Resources.GetColor (Resource.Color.assignmentblue));
                                spinnerImage.SetImageResource (Resource.Drawable.EnrouteImage);
                                break;
                            default:
                                view.SetBackgroundColor (Context.Resources.GetColor (Resource.Color.assignmentgrey));
                                spinnerImage.SetImageResource (Resource.Drawable.HoldImage);
                                break;
                        }
                    }
                };
            }

            number.Text = assignment.JobNumber;
            job.Text = assignment.Title;
            name.Text = assignment.ContactName;
            phone.Text = assignment.ContactPhone;
            StringBuilder builder = new StringBuilder ();
            builder.AppendLine (assignment.Address);
            builder.Append (assignment.City);
            builder.Append (",");
            builder.Append (assignment.State);
            builder.Append (" ");
            builder.Append (assignment.Zip);
            address.Text = builder.ToString ();

            return view;
        }        
    }
}