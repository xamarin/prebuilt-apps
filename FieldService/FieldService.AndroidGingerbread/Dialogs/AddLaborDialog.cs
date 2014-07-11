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
using System.Globalization;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using FieldService.Android.Fragments;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;

namespace FieldService.Android.Dialogs {
    /// <summary>
    /// Dialog for adding labor entries
    /// </summary>
    public class AddLaborDialog : BaseDialog {
        readonly Activity activity;
        readonly LaborType [] laborTypes;
        readonly LaborViewModel laborViewModel;
        EditText description;
        TextView hours;
        Spinner type;
        Button delete;

        public AddLaborDialog (Activity activity)
            : base (activity)
        {
            this.activity = activity;
            laborViewModel = ServiceContainer.Resolve<LaborViewModel> ();
            laborTypes = new LaborType []
            {
                LaborType.Hourly,
                LaborType.OverTime,
                LaborType.HolidayTime,
            };
        }

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            SetContentView (Resource.Layout.AddLaborPopUpLayout);
            SetCancelable (true);

            var cancel = (Button)FindViewById (Resource.Id.cancelAddLabor);
            cancel.Click += (sender, e) => Dismiss ();

            delete = (Button)FindViewById (Resource.Id.deleteAddLabor);
            delete.Enabled = !Assignment.IsHistory;
            delete.Click += (sender, e) => {
                //delete & reload
                if (CurrentLabor != null && CurrentLabor.Id != -1) {
                    DeleteLabor ();
                } else {
                    Dismiss ();
                }
            };

            var save = (Button)FindViewById (Resource.Id.saveAddLabor);
            save.Enabled = !Assignment.IsHistory;
            save.Click += (sender, e) => SaveLabor ();

            var addHours = (ImageButton)FindViewById (Resource.Id.addLaborHours);
            addHours.Enabled = !Assignment.IsHistory;
            addHours.Click += (sender, e) => {
                //add to the hours
                double total = hours.Text.ToDouble (CultureInfo.InvariantCulture);
                total += .5;
                CurrentLabor.Hours = TimeSpan.FromHours (total);
                hours.Text = total.ToString ("0.0");
            };

            var subtractHours = (ImageButton)FindViewById (Resource.Id.subtractLaborHours);
            subtractHours.Enabled = !Assignment.IsHistory;
            subtractHours.Click += (sender, e) => {
                //subtract the hours
                double total = hours.Text.ToDouble (CultureInfo.InvariantCulture);
                total -= .5;
                total = total < 0 ? 0 : total;
                CurrentLabor.Hours = TimeSpan.FromHours (total);
                hours.Text = total.ToString ("0.0");
            };

            type = (Spinner)FindViewById (Resource.Id.addLaborHoursType);
            type.Enabled = !Assignment.IsHistory;
            description = (EditText)FindViewById (Resource.Id.addLaborDescription);
            description.Enabled = !Assignment.IsHistory;
            hours = (TextView)FindViewById (Resource.Id.addLaborHoursText);
            hours.Enabled = !Assignment.IsHistory;

            var adapter = new LaborTypeSpinnerAdapter (laborTypes, Context, Resource.Layout.SimpleSpinnerItem);
            adapter.TextColor = Color.Black;
            type.Adapter = adapter;

            if (CurrentLabor != null) {
                type.SetSelection (laborTypes.ToList ().IndexOf (CurrentLabor.Type));
            }

            type.ItemSelected += (sender, e) => {
                var laborType = laborTypes [e.Position];
                if (CurrentLabor.Type != laborType)
                    CurrentLabor.Type = laborType;
            };
        }

        public override void OnAttachedToWindow ()
        {
            description.Text = CurrentLabor != null ? CurrentLabor.Description : string.Empty;
            hours.Text = CurrentLabor != null ? CurrentLabor.Hours.TotalHours.ToString ("0.0") : string.Empty;
            if (CurrentLabor != null && CurrentLabor.Id != 0) {
                delete.Visibility = ViewStates.Visible;
            } else {
                delete.Visibility = ViewStates.Gone;
            }
            base.OnAttachedToWindow ();
        }

        /// <summary>
        /// Selected labor entry
        /// </summary>
        public Labor CurrentLabor
        {
            get;
            set;
        }

        /// <summary>
        /// The selected assignment
        /// </summary>
        public Assignment Assignment
        {
            get;
            set;
        }
        
        /// <summary>
        /// Deletes the labor entry
        /// </summary>
        private void DeleteLabor ()
        {
            laborViewModel
                .DeleteLaborAsync (Assignment, CurrentLabor)
                .ContinueWith (_ => {
                    activity.RunOnUiThread (() => {
                        var fragment = activity.FragmentManager.FindFragmentById<LaborHourFragment> (Resource.Id.contentFrame);
                        fragment.ReloadHours ();
                        Dismiss ();
                    });
                });
        }

        /// <summary>
        /// Saves the labor entry
        /// </summary>
        private void SaveLabor ()
        {
            CurrentLabor.Hours = TimeSpan.FromHours (hours.Text.ToDouble (CultureInfo.InvariantCulture));
            CurrentLabor.Description = description.Text;
            CurrentLabor.AssignmentId = Assignment.Id;

            laborViewModel
                .SaveLaborAsync (Assignment, CurrentLabor)
                .ContinueWith (_ => {
                    activity.RunOnUiThread (() => {
                        var fragment = activity.FragmentManager.FindFragmentById<LaborHourFragment> (Resource.Id.contentFrame);
                        fragment.ReloadHours ();
                        Dismiss ();
                    });
                });
        }
    }
}