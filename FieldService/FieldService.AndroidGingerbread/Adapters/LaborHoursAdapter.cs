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
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using FieldService.Android.Fragments;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;
using Android.App;

namespace FieldService.Android {
    /// <summary>
    /// Adapter for a list of labor entries
    /// </summary>
    public class LaborHoursAdapter : ArrayAdapter<Labor>, AdapterView.IOnItemSelectedListener {
        readonly Activity activity;
        readonly LaborViewModel laborViewModel;
        List<Labor> laborHours;
        LaborType [] laborTypes;
        int resourceId;

        public LaborHoursAdapter (Activity activity, int resourceId, List<Labor> laborHours)
            : base (activity, resourceId, laborHours)
        {
            laborViewModel = ServiceContainer.Resolve<LaborViewModel> ();
            this.activity = activity;
            this.laborHours = laborHours;
            this.resourceId = resourceId;
            laborTypes = new LaborType []
            {
                LaborType.Hourly,
                LaborType.OverTime,
                LaborType.HolidayTime,
            };
        }

        public Assignment Assignment
        {
            get;
            set;
        }

        public LaborHourFragment Fragment
        {
            get;
            set;
        }

        public override View GetView (int position, View convertView, ViewGroup parent)
        {
            Labor labor = null;
            var view = convertView;
            if (view == null) {
                LayoutInflater inflator = (LayoutInflater)Context.GetSystemService (Context.LayoutInflaterService);
                view = inflator.Inflate (resourceId, null);
            }

            if (laborHours != null && laborHours.Count > position) {
                labor = laborHours [position];
            }

            if (labor == null) {
                return view;
            }

            var description = view.FindViewById<TextView> (Resource.Id.laborDescription);
            var hours = view.FindViewById<TextView> (Resource.Id.laborHours);
            var laborType = view.FindViewById<Spinner> (Resource.Id.laborType);

            var adapter = new LaborTypeSpinnerAdapter (laborTypes, Context, Resource.Layout.SimpleSpinnerItem);
            adapter.TextColor = Color.Black;
            adapter.Background = Color.White;
            laborType.Adapter = adapter;

            laborType.OnItemSelectedListener = this;
            laborType.Tag = position;
            laborType.SetSelection (laborTypes.ToList ().IndexOf (labor.Type));
            hours.Text = string.Format ("{0} hrs", labor.Hours.TotalHours.ToString ("0.0"));
            description.Text = labor.Description;

            hours.Tag = position;

            laborType.Enabled = !Assignment.IsHistory;
            laborType.Focusable = false;

            return view;
        }

        public void OnItemSelected (AdapterView parent, View view, int position, long id)
        {
            var status = laborTypes [position];
            var index = (int)parent.Tag;
            var currentLabor = GetItem (index);
            if (status != currentLabor.Type) {
                currentLabor.Type = status;
                laborViewModel.SaveLaborAsync (Assignment, currentLabor).ContinueWith (_ => {
                    activity.RunOnUiThread (() => {
                        Fragment.ReloadSingleListItem (position);
                    });
                });
            }
        }

        public void OnNothingSelected (AdapterView parent)
        {
            //do nothing
        }
    }
}