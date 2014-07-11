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
using Android.Content;
using Android.Views;
using Android.Widget;

namespace FieldService.Android {
    /// <summary>
    /// Adapter for the navigation options in the menu
    /// </summary>
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
