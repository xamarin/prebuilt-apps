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
using System.Globalization;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using FieldService.Android.Fragments;
using FieldService.Android.Utilities;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;
using Xamarin.Media;
using Extensions = FieldService.Android.Utilities.AndroidExtensions;

namespace FieldService.Android.Dialogs {
    public class ExpenseDialog : BaseDialog, View.IOnClickListener, AdapterView.IOnItemSelectedListener {
        readonly Activity activity;
        readonly ExpenseViewModel expenseViewModel;
        ExpenseCategory [] expenseTypes;
        Spinner expenseType;
        EditText expenseDescription;
        TextView expenseAmount;
        ImageView expensePhoto;
        Button expenseAddPhoto,
            deleteExpense;
        Bitmap imageBitmap;
        MediaPicker mediaPicker;

        public ExpenseDialog (Activity activity)
            : base (activity)
        {
            this.activity = activity;
            expenseViewModel = ServiceContainer.Resolve<ExpenseViewModel> ();
            expenseTypes = new ExpenseCategory []
            {
                ExpenseCategory.Gas,
                ExpenseCategory.Food,
                ExpenseCategory.Supplies,
                ExpenseCategory.Other,
            };

            mediaPicker = new MediaPicker (activity);
        }

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            SetContentView (Resource.Layout.AddExpensePopUpLayout);
            SetCancelable (true);

            var save = (Button)FindViewById (Resource.Id.addExpenseSave);
            save.Enabled = !Assignment.IsHistory;
            save.SetOnClickListener (this);

            deleteExpense = (Button)FindViewById (Resource.Id.addExpenseDelete);
            deleteExpense.Enabled = !Assignment.IsHistory;
            deleteExpense.SetOnClickListener (this);

            var cancel = (Button)FindViewById (Resource.Id.addExpenseCancel);
            cancel.SetOnClickListener (this);

            expenseType = (Spinner)FindViewById (Resource.Id.addExpenseType);
            expenseDescription = (EditText)FindViewById (Resource.Id.addExpenseDescription);
            expenseDescription.Enabled = !Assignment.IsHistory;
            expenseAmount = (TextView)FindViewById (Resource.Id.addExpenseAmount);
            expenseAmount.Enabled = !Assignment.IsHistory;
            expensePhoto = (ImageView)FindViewById (Resource.Id.addExpenseImage);
            expenseAddPhoto = (Button)FindViewById (Resource.Id.addExpenseAddPhoto);
            expenseAddPhoto.Enabled = !Assignment.IsHistory;
            expenseAddPhoto.SetOnClickListener (this);

            var adapter = new SpinnerAdapter<ExpenseCategory> (expenseTypes, Context, Resource.Layout.SimpleSpinnerItem);
            adapter.TextColor = Color.Black;
            adapter.Background = Color.White;
            expenseType.Adapter = adapter;
            expenseType.Enabled = !Assignment.IsHistory;
            expenseType.OnItemSelectedListener = this;
        }

        public override void OnAttachedToWindow ()
        {
            base.OnAttachedToWindow ();
            if (CurrentExpense != null && CurrentExpense.Id != 0) {
                expenseViewModel.LoadPhotoAsync (CurrentExpense).ContinueWith (_ => {
                    activity.RunOnUiThread (() => {
                        if (expenseViewModel.Photo != null) {
                            if (expenseViewModel.Photo.Image != null) {
                                imageBitmap = BitmapFactory.DecodeByteArray (expenseViewModel.Photo.Image, 0, expenseViewModel.Photo.Image.Length);
                                imageBitmap = Extensions.ResizeBitmap (imageBitmap, Constants.MaxWidth, Constants.MaxHeight);
                            }
                            expensePhoto.SetImageBitmap (imageBitmap);
                            expenseAddPhoto.Visibility = ViewStates.Gone;
                        } else {
                            expensePhoto.SetImageBitmap (null);
                            expenseAddPhoto.Visibility = ViewStates.Visible;
                        }
                    });
                });
                expenseType.SetSelection (expenseTypes.ToList ().IndexOf (CurrentExpense.Category));
                expenseAmount.Text = CurrentExpense.Cost.ToString ("0.00");
                expenseDescription.Text = CurrentExpense.Description;
                deleteExpense.Visibility = CurrentExpense.Id != 0 ? ViewStates.Visible : ViewStates.Gone;
            } else {
                expenseViewModel.Photo = null;
                expensePhoto.SetImageBitmap (null);
                expenseAmount.Text = "0.00";
                expenseDescription.Text = string.Empty;
                expenseType.SetSelection (0);
                expenseAddPhoto.Visibility = ViewStates.Visible;
                deleteExpense.Visibility = ViewStates.Gone;
            }
        }

        public override void OnDetachedFromWindow ()
        {
            base.OnDetachedFromWindow ();
            base.OnDetachedFromWindow ();
            expensePhoto.SetImageBitmap (null);
            if (imageBitmap != null) {
                imageBitmap.Recycle ();
                imageBitmap.Dispose ();
                imageBitmap = null;
            }
            Assignment = null;
            CurrentExpense = null;
            //PhotoStream = null;
        }

        /// <summary>
        /// save current expense
        /// </summary>
        private void SaveExpense ()
        {
            CurrentExpense.Description = expenseDescription.Text;
            CurrentExpense.Cost = expenseAmount.Text.ToDecimal (CultureInfo.InvariantCulture);
            CurrentExpense.AssignmentId = Assignment.Id;

            var task = expenseViewModel.SaveExpenseAsync (Assignment, CurrentExpense);
            if (expenseViewModel.Photo != null) {
                task = task
                    .ContinueWith (_ => {
                        expenseViewModel.Photo.ExpenseId = CurrentExpense.Id;
                        expenseViewModel.Photo.Image = imageBitmap.ToByteArray ();
                        System.Console.WriteLine (expenseViewModel.Photo.Image.Length);
                    })
                    .ContinueWith (_ => expenseViewModel.SavePhotoAsync ());
            }
            task.ContinueWith (_ => {
                activity.RunOnUiThread (() => {
                    var fragment = activity.FragmentManager.FindFragmentById<ExpenseFragment> (Resource.Id.contentFrame);
                    fragment.ReloadExpenseData ();
                    Dismiss ();
                });
            });
        }

        /// <summary>
        /// delete current expense
        /// </summary>
        private void DeleteExpense ()
        {
            expenseViewModel.DeleteExpenseAsync (Assignment, CurrentExpense)
                .ContinueWith (_ => {
                    activity.RunOnUiThread (() => {
                        var fragment = activity.FragmentManager.FindFragmentById<ExpenseFragment> (Resource.Id.contentFrame);
                        fragment.ReloadExpenseData ();
                        Dismiss ();
                    });
                });
        }

        /// <summary>
        /// fragment's current assignment
        /// </summary>
        public Assignment Assignment
        {
            get;
            set;
        }

        /// <summary>
        /// current expense that is being edited
        /// </summary>
        public Expense CurrentExpense
        {
            get;
            set;
        }

        public void OnClick (View v)
        {
            switch (v.Id) {
                case Resource.Id.addExpenseDelete:
                    if (CurrentExpense != null && CurrentExpense.Id != -1) {
                        DeleteExpense ();
                    } else {
                        Dismiss ();
                    }
                    break;
                case Resource.Id.addExpenseSave:
                    SaveExpense ();
                    break;
                case Resource.Id.addExpenseCancel:
                    Dismiss ();
                    break;
                case Resource.Id.addExpenseAddPhoto: {
                        var choices = new List<string> ();
                        choices.Add (activity.Resources.GetString (Resource.String.Gallery));
                        if (mediaPicker.IsCameraAvailable) {
                            choices.Add (activity.Resources.GetString (Resource.String.Camera));
                        }
                        AlertDialog.Builder takePictureDialog = new AlertDialog.Builder (activity);
                        takePictureDialog.SetTitle ("Select:");
                        takePictureDialog.SetItems (choices.ToArray (), (innerSender, innerE) => {
                            if (innerE.Which == 0) {
                                //gallery
                                mediaPicker.PickPhotoAsync ().ContinueWith (t => {
                                    if (t.IsCanceled)
                                        return;
                                    activity.RunOnUiThread (() => {
                                        expenseAddPhoto.Visibility = ViewStates.Gone;
                                        imageBitmap = BitmapFactory.DecodeStream (t.Result.GetStream ());
                                        imageBitmap = Extensions.ResizeBitmap (imageBitmap, Constants.MaxWidth, Constants.MaxHeight);
                                        expensePhoto.SetImageBitmap (imageBitmap);
                                        expenseViewModel.Photo = new ExpensePhoto { ExpenseId = CurrentExpense.Id };
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
                                    activity.RunOnUiThread (() => {
                                        expenseAddPhoto.Visibility = ViewStates.Gone;
                                        imageBitmap = BitmapFactory.DecodeStream (t.Result.GetStream ());
                                        imageBitmap = Extensions.ResizeBitmap (imageBitmap, Constants.MaxWidth, Constants.MaxHeight);
                                        expensePhoto.SetImageBitmap (imageBitmap);
                                        expenseViewModel.Photo = new ExpensePhoto { ExpenseId = CurrentExpense.Id };
                                    });
                                });
                            }
                        });
                        takePictureDialog.Show ();
                    }
                    break;
                default:
                    break;
            }
        }

        public void OnItemSelected (AdapterView parent, View view, int position, long id)
        {
            var category = expenseTypes [position];
            if (CurrentExpense.Category != category)
                CurrentExpense.Category = category;
        }

        public void OnNothingSelected (AdapterView parent)
        {
            //do nothing
        }
    }
}