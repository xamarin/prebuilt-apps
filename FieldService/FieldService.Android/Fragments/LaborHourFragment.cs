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
//    limitations under the License.using System;

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
using FieldService.Utilities;
using FieldService.ViewModels;

namespace FieldService.Android.Fragments {
    public class LaborHourFragment : Fragment, AdapterView.IOnItemClickListener {
        ListView laborListView;
        LaborViewModel laborViewModel;

        public override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            laborViewModel = ServiceContainer.Resolve<LaborViewModel> ();
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView (inflater, container, savedInstanceState);
            var view = inflater.Inflate (Resource.Layout.LaborHoursLayout, null, true);

            laborListView = view.FindViewById<ListView> (Resource.Id.laborListViewFragment);

            ReloadLaborHours ();

            return view;
        }

        public void ReloadLaborHours ()
        {
            if (LaborHours != null) {
                laborListView.Adapter = new LaborHoursAdapter (Activity, Resource.Layout.LaborHoursListItemLayout, LaborHours);
            }
        }

        public List<Labor> LaborHours
        {
            get;
            set;
        }
        
        public void OnItemClick (AdapterView parent, View view, int position, long id)
        {
            throw new NotImplementedException ();
        }
    }
}