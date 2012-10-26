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
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using FieldService.Data;

namespace FieldService.Android {
    /// <summary>
    /// Adapter for a list of labor entries
    /// </summary>
    public class LaborHoursAdapter : ArrayAdapter<Labor> {
        List<Labor> laborHours;
        int resourceId;

        public LaborHoursAdapter (Context context, int resourceId, List<Labor> laborHours)
            : base (context, resourceId, laborHours)
        {
            this.laborHours = laborHours;
            this.resourceId = resourceId;
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

            List<string> laborTypes = new List<string>();
            foreach (var item in Enum.GetValues(typeof(LaborType)))
	    {
		laborTypes.Add(item.ToString()); 
            }

            var adapter = new LaborTypeSpinnerAdapter (laborTypes, Context, Resource.Layout.SimpleSpinnerItem);
            adapter.TextColor = Color.Black;
            laborType.Adapter = adapter;

            laborType.SetSelection (laborTypes.IndexOf (labor.TypeAsString));
            hours.Text = string.Format ("{0} hrs", labor.Hours.TotalHours.ToString("0.0"));
            description.Text = labor.Description;

            hours.Tag = position;

            laborType.Focusable = false;

            return view;
        }
    }
}