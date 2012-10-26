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
using FieldService.Utilities;
using FieldService.ViewModels;

namespace FieldService.Android.Dialogs {
    public class SignatureDialog : BaseDialog, View.IOnClickListener {
        AssignmentViewModel assignmentViewModel;
        ImageView signature;
        public SignatureDialog (Context context)
            : base (context)
        {
            assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();
        }
        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            SetContentView (Resource.Layout.AddSignatureLayout);

            var clear = (Button)FindViewById (Resource.Id.signatureClearButton);
            var save = (Button)FindViewById (Resource.Id.signatureSaveButton);
            var cancel = (Button)FindViewById (Resource.Id.signatureCancelButton);

            signature = (ImageView)FindViewById (Resource.Id.signatureImage);

            clear.SetOnClickListener (this);
            save.SetOnClickListener (this);
            cancel.SetOnClickListener (this);
        }

        public override void OnAttachedToWindow ()
        {
            base.OnAttachedToWindow ();
            if (Assignment != null && Assignment.Signature != null) {
                using (var bmp = BitmapFactory.DecodeByteArray (Assignment.Signature, 0, Assignment.Signature.Length)) {
                    signature.SetImageBitmap (bmp);
                }
            }
        }

        public Assignment Assignment
        {
            get;
            set;
        }

        public void OnClick (View v)
        {
            switch (v.Id) {
                case Resource.Id.signatureCancelButton:
                    Dismiss ();
                    break;
                case Resource.Id.signatureClearButton:
                    signature.SetImageBitmap (null);
                    break;
                case Resource.Id.signatureSaveButton:
                    //save
                    Dismiss ();
                    break;
                default:
                    break;
            }
        }
    }
}