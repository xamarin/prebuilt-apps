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

using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using FieldService.Data;
using FieldService.Utilities;

namespace FieldService.Android {
    /// <summary>
    /// Adapter for a spinner for selecting LaborType
    /// </summary>
    public class LaborTypeSpinnerAdapter : BaseAdapter, ISpinnerAdapter {
        List<string> items;
        Context context;
        int resourceId;
        public LaborTypeSpinnerAdapter (LaborType[] items, Context context, int resourceId)
            : base ()
        {
            this.items = new List<string> ();
            foreach (var item in items) {
                this.items.Add (item.ToUserString ());
            }
            this.context = context;
            this.resourceId = resourceId;
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

        /// <summary>
        /// Text color for the item
        /// </summary>
        public Color TextColor
        {
            get;
            set;
        }

        /// <summary>
        /// Background color of the text view
        /// </summary>
        public Color Background
        {
            get;
            set;
        }

        public override View GetView (int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            if (view == null) {
                LayoutInflater inflator = (LayoutInflater)context.GetSystemService (Context.LayoutInflaterService);
                view = inflator.Inflate (resourceId, null);
            }

            var item = items.ElementAtOrDefault (position);
            if (item == null) {
                return view;
            }

            var textView = view.FindViewById<TextView> (Resource.Id.simpleSpinnerTextView);
            textView.Text = item;
            textView.SetTextColor (TextColor);
            textView.SetBackgroundColor (Background);

            return view;
        }

        protected override void Dispose (bool disposing)
        {
            context = null;
            base.Dispose (disposing);
        }
    }
}