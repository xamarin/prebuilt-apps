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
using Android.Graphics;
using Android.Views;
using Android.Widget;
using FieldService.Data;
using FieldService.Android.Utilities;

namespace FieldService.Android {
    /// <summary>
    /// Adapter for a list of photos
    /// </summary>
    public class PhotosAdapter : ArrayAdapter<Photo> {
        List<Photo> photos;
        int resourceId;

        public PhotosAdapter (Context context, int resourceId, List<Photo> photos)
            : base (context, resourceId, photos)
        {
            this.photos = photos;
            this.resourceId = resourceId;
        }

        public override View GetView (int position, View convertView, ViewGroup parent)
        {
            Photo photo = null;
            var view = convertView;
            if (view == null) {
                LayoutInflater inflator = (LayoutInflater)Context.GetSystemService (Context.LayoutInflaterService);
                view = inflator.Inflate (resourceId, null);
            }

            if (photos != null && photos.Count > position) {
                photo = photos [position];
            }

            if (photo == null) {
                return view;
            }

            var image = view.FindViewById<ImageView> (Resource.Id.photoListViewImage);
            var dateTime = view.FindViewById<TextView> (Resource.Id.photoListViewDateTime);
            var description = view.FindViewById<TextView> (Resource.Id.photoListViewDescription);

            dateTime.Text = string.Format ("{0}  {1}", photo.Date.ToString("t"), photo.Date.ToString("d"));
            description.Text = photo.Description;
            if (photo.Image != null) {
                var options = new BitmapFactory.Options {
                    InScaled = false,
                    InDither = false,
                    InJustDecodeBounds = false,
                    InPurgeable = true,
                    InInputShareable = true,
                };
                options.InSampleSize = photo.Image.ToSampleSize ();
                using (var bmp = BitmapFactory.DecodeByteArray (photo.Image, 0, photo.Image.Length, options)) {
                    image.SetImageBitmap (bmp);
                }
            }
            image.Tag = position;

            return view;
        }
    }
}