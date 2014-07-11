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

using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
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
    /// <summary>
    /// Fragment for the confirmation section
    /// </summary>
    public class ConfirmationFragment : Fragment {
        AssignmentViewModel assignmentViewModel;
        PhotoViewModel photoViewModel;
        SignatureDialog signatureDialog;
        PhotoDialog photoDialog;
        ListView photoListView;
        MediaPicker mediaPicker;
        ImageView signatureImage;

        public override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();
            photoViewModel = ServiceContainer.Resolve<PhotoViewModel> ();
            mediaPicker = new MediaPicker (Activity);
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView (inflater, container, savedInstanceState);
            var view = inflater.Inflate (Resource.Layout.ConfirmationsLayout, null, true);
            signatureImage = view.FindViewById<ImageView> (Resource.Id.confirmationsSignature);

            photoListView = view.FindViewById<ListView> (Resource.Id.confirmationPhotoList);
            photoListView.ItemClick += (sender, e) => {
                var image = view.FindViewById<ImageView> (Resource.Id.photoListViewImage);
                if (image != null) {
                    int index = (int)image.Tag;
                    var photo = Photos.ElementAtOrDefault (index);
                    photoDialog = new PhotoDialog (Activity);
                    photoDialog.Activity = Activity;
                    photoDialog.Assignment = Assignment;
                    photoDialog.Photo = photo;
                    photoDialog.Show ();
                }
            };

            var addPhoto = view.FindViewById<Button> (Resource.Id.confirmationsAddPhoto);
            if (Assignment != null) {
                addPhoto.Enabled = !Assignment.IsHistory;
            }
            addPhoto.Click += (sender, e) => {
                var choices = new List<string> ();
                choices.Add (Resources.GetString (Resource.String.Gallery));
                if (mediaPicker.IsCameraAvailable) {
                    choices.Add (Resources.GetString (Resource.String.Camera));
                }
                AlertDialog.Builder takePictureDialog = new AlertDialog.Builder (Activity);
                takePictureDialog.SetTitle ("Select:");
                takePictureDialog.SetItems (choices.ToArray (), (innerSender, innerE) => {
                    if (innerE.Which == 0) {
                        //gallery
                        mediaPicker.PickPhotoAsync ().ContinueWith (t => {
                            if (t.IsCanceled)
                                return;
                            Activity.RunOnUiThread (() => {
                                photoDialog = new PhotoDialog (Activity);
                                photoDialog.Activity = Activity;
                                photoDialog.Assignment = Assignment;
                                photoDialog.PhotoStream = t.Result.GetStream ();
                                photoDialog.Show ();
                            });
                        });
                    } else if (innerE.Which == 1) {
                        //camera
                        StoreCameraMediaOptions options = new StoreCameraMediaOptions ();
                        options.Directory = "FieldService";
                        options.Name = "FieldService.jpg";
                        mediaPicker.TakePhotoAsync (options).ContinueWith (t => {
                            if (t.IsCanceled)
                                return;
                            Activity.RunOnUiThread (() => {
                                photoDialog = new PhotoDialog (Activity);
                                photoDialog.Activity = Activity;
                                photoDialog.Assignment = Assignment;
                                photoDialog.PhotoStream = t.Result.GetStream ();
                                photoDialog.Show ();
                            });
                        });
                    }
                });
                takePictureDialog.Show ();
            };

            var addSignature = view.FindViewById<Button> (Resource.Id.confirmationsAddSignature);
            if (Assignment != null) {
                addSignature.Enabled = !Assignment.IsHistory;
            }
            addSignature.Click += (sender, e) => {
                signatureDialog = new SignatureDialog (Activity);
                signatureDialog.Activity = Activity;
                signatureDialog.Assignment = Assignment;
                signatureDialog.Show ();
            };

            var completeSignature = view.FindViewById<Button> (Resource.Id.confirmationsComplete);
            if (Assignment != null) {
                completeSignature.Enabled = Assignment.CanComplete;
            }
            completeSignature.Click += (sender, e) => {
                if (assignmentViewModel.Signature == null) {
                    AlertDialog.Builder builder = new AlertDialog.Builder (Activity);
                    builder
                        .SetTitle (string.Empty)
                        .SetMessage ("No signature!")
                        .SetPositiveButton ("Ok", (innerSender, innere) => { })
                        .Show ();
                    return;
                }
                completeSignature.Enabled = false;
                Assignment.Status = AssignmentStatus.Complete;
                assignmentViewModel.SaveAssignmentAsync (Assignment)
                    .ContinueWith (_ => {
                        Activity.RunOnUiThread (() => {
                            completeSignature.Enabled = true;
                            Activity.Finish ();
                        });
                    });
            };

            ReloadListView ();

            ReloadSignature ();

            return view;
        }
        
        /// <summary>
        /// Reloads the list view
        /// </summary>
        private void ReloadListView ()
        {
            if (Photos != null) {
                photoListView.Adapter = new PhotosAdapter (Activity, Resource.Layout.PhotoItemLayout, Photos); ;
            }
        }

        private void ReloadSignature ()
        {
            assignmentViewModel.LoadSignatureAsync (Assignment)
                .ContinueWith (_ => {
                    Activity.RunOnUiThread (() => {
                        if (assignmentViewModel.Signature != null) {
                            using (var bmp = BitmapFactory.DecodeByteArray (assignmentViewModel.Signature.Image, 0, assignmentViewModel.Signature.Image.Length)) {
                                signatureImage.SetImageBitmap (bmp);
                            }
                        } else {
                            signatureImage.SetImageBitmap (null);
                        }
                    });
                });
        }

        /// <summary>
        /// List of photos
        /// </summary>
        public List<Photo> Photos
        {
            get;
            set;
        }

        /// <summary>
        /// The current assignment
        /// </summary>
        public Assignment Assignment
        {
            get;
            set;
        }
        
        /// <summary>
        /// Dismiss the signature dialog if shown
        /// </summary>
        public override void OnPause ()
        {
            base.OnPause ();

            if (signatureDialog != null) {
                if (signatureDialog.IsShowing) {
                    signatureDialog.Dismiss ();
                }
            }
        }

        /// <summary>
        /// Reload Confirmation fragment
        /// </summary>
        public void ReloadConfirmation ()
        {
            photoViewModel
                .LoadPhotosAsync (Assignment)
                .ContinueWith (_ => {
                    Activity.RunOnUiThread (() => {
                        Photos = photoViewModel.Photos;
                        ReloadListView ();
                    });
                });
            ReloadSignature ();
        }
    }
}