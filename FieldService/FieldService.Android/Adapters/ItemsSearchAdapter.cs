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
using Android.Views;
using Android.Widget;
using FieldService.Data;

namespace FieldService.Android {
    /// <summary>
    /// Adapter for searching through a list of items
    /// </summary>
    public class ItemsSearchAdapter : ArrayAdapter<Item> {
        
        IList<Item> items;
        int resourceId;

        public ItemsSearchAdapter (Context context, int resourceId, IList<Item> items)
            : base (context, resourceId, items)
        {
            this.items = items;
            this.resourceId = resourceId;
        }

        public override View GetView (int position, View convertView, ViewGroup parent)
        {
            Item item = null;
            var view = convertView;
            if (view == null) {
                LayoutInflater inflator = (LayoutInflater)Context.GetSystemService (Context.LayoutInflaterService);
                view = inflator.Inflate (resourceId, null);
            }

            if (items != null && items.Count > position) {
                item = items [position];
            }

            if (item == null) {
                return view;
            }

            var text = view.FindViewById<TextView> (Resource.Id.itemsListItemText);
            text.Text = string.Format ("#{0} {1}", item.Number, item.Name);

            return view;
        }

        public Item GetAssignmentItem (int position)
        {
            return items.ElementAtOrDefault (position);
        }
    }
}