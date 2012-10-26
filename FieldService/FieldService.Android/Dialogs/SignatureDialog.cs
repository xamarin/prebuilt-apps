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

using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;

namespace FieldService.Android.Dialogs {
    /// <summary>
    /// Dialog for capturing a signature
    /// </summary>
    public class SignatureDialog : BaseDialog {
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
            clear.Click += (sender, e) => signature.SetImageBitmap (null);

            var save = (Button)FindViewById (Resource.Id.signatureSaveButton);
            save.Click += (sender, e) => Dismiss ();

            var cancel = (Button)FindViewById (Resource.Id.signatureCancelButton);
            cancel.Click += (sender, e) => Dismiss ();

            signature = (ImageView)FindViewById (Resource.Id.signatureImage);
        }

        /// <summary>
        /// Load the image of the signature
        /// </summary>
        public override void OnAttachedToWindow ()
        {
            base.OnAttachedToWindow ();

            if (Assignment != null && Assignment.Signature != null) {
                using (var bmp = BitmapFactory.DecodeByteArray (Assignment.Signature, 0, Assignment.Signature.Length)) {
                    signature.SetImageBitmap (bmp);
                }
            }
        }

        /// <summary>
        /// The current assignment
        /// </summary>
        public Assignment Assignment
        {
            get;
            set;
        }
    }
}