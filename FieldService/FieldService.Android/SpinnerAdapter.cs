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

namespace FieldService.Android {
    public class SpinnerAdapter : BaseAdapter, ISpinnerAdapter {
        List<string> items;
        Activity activity;
        public SpinnerAdapter (List<string> items, Activity activity)
            : base ()
        {
            this.items = items;
            this.activity = activity;
        }

        public override Java.Lang.Object GetItem (int position)
        {
            return items [position];
        }

        public override int Count
        {
            get { return items.Count; }
        }

        public override long GetItemId (int position)
        {
            return (long)position;
        }

        public override View GetView (int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            if (view == null) {
                LayoutInflater inflator = (LayoutInflater)activity.GetSystemService (Context.LayoutInflaterService);
                view = inflator.Inflate (Resource.Layout.SimpleSpinnerItem, null);
            }

            var item = items.ElementAtOrDefault (position);
            if (item == null) {
                return view;
            }

            var textView = view.FindViewById<TextView> (Resource.Id.simpleSpinnerTextView);
            textView.Text = item;

            return view;
        }
    }
}