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
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;

namespace FieldService.Android.Dialogs {
    public class AddLaborDialog : BaseDialog, View.IOnClickListener{
        LaborViewModel laborViewModel;
        EditText description;
        TextView hours;
        List<string> laborHourTypes;
        public AddLaborDialog (Context context)
            : base (context)
        {
            laborViewModel = ServiceContainer.Resolve<LaborViewModel> ();
            laborHourTypes = new List<string> ();
            foreach (var item in Enum.GetValues (typeof (LaborType))) {
                laborHourTypes.Add (item.ToString ());
            }
        }

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            SetContentView (Resource.Layout.AddLaborPopUpLayout);
            SetCancelable (true);

            var cancel = (Button)FindViewById (Resource.Id.cancelAddLabor);
            var delete = (Button)FindViewById (Resource.Id.deleteAddLabor);
            var save = (Button)FindViewById (Resource.Id.saveAddLabor);
            var addHours = (ImageButton)FindViewById (Resource.Id.addLaborHours);
            var subtractHours = (ImageButton)FindViewById (Resource.Id.subtractLaborHours);
            var type = (Spinner)FindViewById (Resource.Id.addLaborHoursType);
            description = (EditText)FindViewById (Resource.Id.addLaborDescription);
            hours = (TextView)FindViewById (Resource.Id.addLaborHoursText);

            cancel.SetOnClickListener (this);
            delete.SetOnClickListener (this);
            save.SetOnClickListener (this);
            addHours.SetOnClickListener (this);
            subtractHours.SetOnClickListener (this);

            var adapter = new LaborTypeSpinnerAdapter (laborHourTypes, Context, Resource.Layout.SimpleSpinnerItem);
            adapter.TextColor = Color.Black;
            type.Adapter = adapter;

            if (CurrentLabor != null) {
                type.SetSelection (laborHourTypes.IndexOf (CurrentLabor.TypeAsString));
            }
        }

        public override void OnAttachedToWindow ()
        {
            description.Text = CurrentLabor != null ? CurrentLabor.Description : string.Empty;
            hours.Text = CurrentLabor != null ? CurrentLabor.Hours.TotalHours.ToString ("0.0") : string.Empty;
            base.OnAttachedToWindow ();
        }

        public Labor CurrentLabor
        {
            get;
            set;
        }

        public Assignment Assignment
        {
            get;
            set;
        }

        public void OnClick (View v)
        {
            switch (v.Id) {
                case Resource.Id.cancelAddLabor: {
                        Dismiss ();
                    }
                    break;
                case Resource.Id.saveAddLabor: {
                        //save & reload
                        Dismiss ();
                    }
                    break;
                case Resource.Id.deleteAddLabor: {
                        //delete & reload
                        Dismiss ();
                    }
                    break;
                case Resource.Id.addLaborHours: {
                        //add to the hours
                        var total = hours.Text.ToDouble ();
                        total += .5;
                        CurrentLabor.Hours = TimeSpan.FromHours (total);
                        hours.Text = total.ToString ("0.0");
                    }
                    break;
                case Resource.Id.subtractLaborHours: {
                        //subtract the hours
                        var total = hours.Text.ToDouble ();
                        total -= .5;
                        CurrentLabor.Hours = TimeSpan.FromHours (total);
                        hours.Text = total.ToString ("0.0");
                    }
                    break;
                default:
                    break;
            }
        }
    }
}