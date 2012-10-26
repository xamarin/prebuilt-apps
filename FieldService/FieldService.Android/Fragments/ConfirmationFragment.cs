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
//    limitations under the License.using System;

using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using FieldService.Android.Dialogs;
using FieldService.Android.Utilities;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;
using Xamarin.Media;

namespace FieldService.Android.Fragments {
    public class ConfirmationFragment : Fragment, AdapterView.IOnItemClickListener, View.IOnClickListener, IDialogInterfaceOnClickListener {
        PhotoViewModel photoViewModel;
        SignatureDialog signatureDialog;
        PhotoDialog photoDialog;
        ListView photoListView;
        MediaPicker mediaPicker;
        public override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            photoViewModel = ServiceContainer.Resolve<PhotoViewModel> ();
            mediaPicker = new MediaPicker (Activity);
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView (inflater, container, savedInstanceState);
            var view = inflater.Inflate (Resource.Layout.ConfirmationsLayout, null, true);

            photoListView = view.FindViewById<ListView> (Resource.Id.confirmationPhotoList);
            var addPhoto = view.FindViewById<Button> (Resource.Id.confirmationsAddPhoto);
            var addSignature = view.FindViewById<Button> (Resource.Id.confirmationsAddSignature);
            var completeSignature = view.FindViewById<Button> (Resource.Id.confirmationsComplete);

            if (Photos != null) {
                photoListView.Adapter = new PhotosAdapter (Activity, Resource.Layout.PhotoItemLayout, Photos);
            }

            photoListView.OnItemClickListener = this;
            addSignature.SetOnClickListener (this);
            addPhoto.SetOnClickListener (this);
            completeSignature.SetOnClickListener (this);

            return view;
        }

        public List<Photo> Photos
        {
            get;
            set;
        }

        public Assignment Assignment
        {
            get;
            set;
        }

        public void OnItemClick (AdapterView parent, View view, int position, long id)
        {
            var image = view.FindViewById<ImageView> (Resource.Id.photoListViewImage);
            if (image != null) {
                var index = image.Tag.ToString ().ToInt ();
                var photo = Photos.ElementAtOrDefault (index);
                photoDialog = new PhotoDialog (Activity);
                photoDialog.Activity = Activity;
                photoDialog.Assignment = Assignment;
                photoDialog.Photo = photo;
                photoDialog.Show ();
            }
        }

        public override void OnPause ()
        {
            base.OnPause ();
            if (signatureDialog != null) {
                if (signatureDialog.IsShowing) {
                    signatureDialog.Dismiss ();
                }
            }
        }

        public void OnClick (View v)
        {
            switch (v.Id) {
                case Resource.Id.confirmationsAddSignature: {
                        signatureDialog = new SignatureDialog (Activity);
                        signatureDialog.Assignment = Assignment;
                        signatureDialog.Show ();
                    }
                    break;
                case Resource.Id.confirmationsAddPhoto:
                    var choices = new List<string> ();
                    choices.Add (Resources.GetString (Resource.String.Gallery));
                    if (mediaPicker.IsCameraAvailable) {
                        choices.Add (Resources.GetString (Resource.String.Camera));
                    }
                    AlertDialog.Builder takePictureDialog = new AlertDialog.Builder (Activity);
                    takePictureDialog.SetTitle ("Select:");
                    takePictureDialog.SetItems (choices.ToArray (), this);
                    takePictureDialog.Show ();
                    break;
                case Resource.Id.confirmationsComplete:
                    break;
                default:
                    break;
            }
        }

        public void OnClick (IDialogInterface dialog, int which)
        {
            if (which == 0) {
                //gallery
                mediaPicker.PickPhotoAsync ().ContinueWith (t => {
                    if (t.IsCanceled)
                        return;
                    photoDialog = new PhotoDialog (Activity);
                    photoDialog.Activity = Activity;
                    photoDialog.Assignment = Assignment;
                    photoDialog.PhotoStream = t.Result.GetStream ();
                    photoDialog.Show ();
                    });
            } else if (which == 1) {
                //camera
                StoreCameraMediaOptions options = new StoreCameraMediaOptions ();
                options.Directory = "FieldService";
                options.Name = "FieldService.jpg";
                mediaPicker.TakePhotoAsync (options).ContinueWith (t => {
                    if (t.IsCanceled)
                        return;
                    photoDialog = new PhotoDialog (Activity);
                    photoDialog.Activity = Activity;
                    photoDialog.Assignment = Assignment;
                    photoDialog.PhotoStream = t.Result.GetStream ();
                    photoDialog.Show ();
                });
            }
        }
    }
}