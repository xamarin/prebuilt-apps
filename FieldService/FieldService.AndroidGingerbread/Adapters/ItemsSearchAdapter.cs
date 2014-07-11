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
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using FieldService.Data;

namespace FieldService.Android {
    /// <summary>
    /// Adapter for searching through a list of items
    /// </summary>
    public class ItemsSearchAdapter : ArrayAdapter<Item>{
        
        IList<Item> items;
        IList<Item> non_filtered;
        int resourceId;

        public ItemsSearchAdapter (Context context, int resourceId, IList<Item> items)
            : base (context, resourceId, items)
        {
            non_filtered = new List<Item> (items);
            this.items = new List<Item> (items);
            this.resourceId = resourceId;
        }

        public override View GetView (int position, View convertView, ViewGroup parent)
        {
            Item item = null;
            if (items != null && items.Count > position) {
                item = items [position];
            }

            LayoutInflater inflator = (LayoutInflater)Context.GetSystemService (Context.LayoutInflaterService);
            var view = inflator.Inflate (resourceId, null);

            var text = view.FindViewById<TextView> (Resource.Id.itemsListItemText);
            if (item != null) {
                text.Text = string.Format ("#{0} {1}", item.Number, item.Name);
            }

            return view;
        }

        public override int Count
        {
            get
            {
                return items.Count;
            }
        }

        public void FilterItems (string filter)
        {
            var filtered = new List<Item> ();

            foreach (var item in non_filtered) {
                if (item.Name.ToLower().Contains (filter) || item.Number.ToLower().Contains (filter)) {
                    filtered.Add (item);
                }
            }

            this.items = filtered;
        }

        public Item GetAssignmentItem (int position)
        {
            return items.ElementAtOrDefault (position);
        }
    }
}