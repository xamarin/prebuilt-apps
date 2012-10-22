using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace FieldService.Android {
    public class NavigationAdapter : ArrayAdapter<string> {

        List<string> items;
        int resourceId;
        public NavigationAdapter (Context context, int resourceId, List<string> items)
            : base (context, resourceId, items)
        {
            this.items = items;
            this.resourceId = resourceId;
        }

        public override View GetView (int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            var item = string.Empty;

            if (view == null) {
                LayoutInflater inflator = (LayoutInflater)Context.GetSystemService (Context.LayoutInflaterService);
                view = inflator.Inflate (resourceId, null);
            }

            if (items.Count > position) {
                item = items [position];
            }

            if (string.IsNullOrEmpty(item)) {
                return view;
            }

            var text = view.FindViewById<TextView> (Resource.Id.navigationListViewText);
            var image = view.FindViewById<ImageView> (Resource.Id.navigationListViewImage);

            text.Text = item;
            image.Visibility = ViewStates.Invisible;

            return view;
        }
    }
}
