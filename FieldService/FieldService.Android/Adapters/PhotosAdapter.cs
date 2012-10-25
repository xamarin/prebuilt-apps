using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FieldService.Data;

namespace FieldService.Android {
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

            dateTime.Text = string.Format ("{0}  {1}", photo.Date.Date.ToShortTimeString (), photo.Date.Date.ToShortDateString ());
            description.Text = photo.Description;
            using (var bmp = BitmapFactory.DecodeByteArray(photo.Image, 0, photo.Image.Length)) {
                image.SetImageBitmap (bmp);
                
            }

            return view;
        }
    }
}