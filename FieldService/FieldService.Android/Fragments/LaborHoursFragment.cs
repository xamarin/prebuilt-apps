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
    public class LaborHoursFragment : Fragment {
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
            laborListView.Adapter = new LaborHoursAdapter (this.Activity, Resource.Layout.LaborHoursListItemLayout, LaborHours);

            return view;
        }

        public List<Labor> LaborHours
        {
            get;
            set;
        }
    }
}