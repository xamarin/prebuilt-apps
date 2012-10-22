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
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Signature.iPhone;
using FieldService.Utilities;

namespace FieldService.iOS
{
	public class SignatureController : UIPopoverController
	{
		public SignatureController ()
			: base(new UINavigationController(new ContentController()))
		{
			PopoverContentSize = new SizeF(665, 400);
		}

		private class ContentController : UIViewController
		{
			SignatureView signatureView;
			UIBarButtonItem cancel;
			UIBarButtonItem save;

			public ContentController ()
			{
				Title = "Add Signature";
			}

			public override void ViewDidLoad ()
			{
				base.ViewDidLoad ();

				cancel = new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Bordered, (sender, e) => {
					var signatureController = ServiceContainer.Resolve<SignatureController>();
					signatureController.Dismiss (true);
				});
				cancel.SetTitleTextAttributes (new UITextAttributes { TextColor = UIColor.White }, UIControlState.Normal);
				cancel.SetBackgroundImage (Theme.DarkBarButtonItem, UIControlState.Normal, UIBarMetrics.Default);

				save = new UIBarButtonItem("Save", UIBarButtonItemStyle.Bordered, (sender, e) => {
					var signatureController = ServiceContainer.Resolve<SignatureController>();
					signatureController.Dismiss (true);
				});
				save.SetTitleTextAttributes (new UITextAttributes { TextColor = UIColor.White }, UIControlState.Normal);
				save.SetBackgroundImage (Theme.DarkBarButtonItem, UIControlState.Normal, UIBarMetrics.Default);

				NavigationItem.LeftBarButtonItem = cancel;
				NavigationItem.RightBarButtonItem = save;
				NavigationController.NavigationBar.SetBackgroundImage (null, UIBarMetrics.Default);

				signatureView = new SignatureView(View.Frame)
				{
					AutoresizingMask = UIViewAutoresizing.All,
				};
				View.AddSubview (signatureView);
			}

			public override void ViewWillAppear (bool animated)
			{
				base.ViewWillAppear (animated);

				signatureView.Clear ();
			}
		}
	}
}

