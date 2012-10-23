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

using Android.App;
using Android.Content;
using Android.GoogleMaps;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace FieldService.Android.Utilities {
    public class MapOverlayItem : ItemizedOverlay {
        OverlayItem item;
        Context context;

        public MapOverlayItem (Context context, Drawable overlayDrawable, OverlayItem overlay)
            : base (overlayDrawable)
        {
            item = overlay;
            this.context = context;

            BoundCenterBottom (overlayDrawable);
            Populate ();
        }

        protected override Java.Lang.Object CreateItem (int i)
        {
            return item;
        }

        public override int Size ()
        {
            return 1;
        }

        protected override bool OnTap (int index)
        {
            var dialog = new AlertDialog.Builder (context);
            dialog.SetTitle (item.Title);
            dialog.SetMessage (item.Snippet);
            dialog.Show ();
            return true;
        }
    }
}