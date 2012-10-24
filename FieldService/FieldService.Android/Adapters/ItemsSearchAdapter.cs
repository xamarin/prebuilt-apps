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

namespace FieldService.Android {
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