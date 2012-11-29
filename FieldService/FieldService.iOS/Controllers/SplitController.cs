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
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using FieldService.Utilities;

namespace FieldService.iOS
{
	/// <summary>
	/// The main split controller in the app, we couldn't use UISplitViewController because it must be the very root controller of the app
	/// </summary>
	[Register("SplitController")]
	public partial class SplitController : BaseController
	{
		private UIPopoverController popover;
		private bool wasLandscape = true;
		private const float masterWidth = 321;

		public SplitController (IntPtr handle) : base(handle)
		{
			ServiceContainer.Register (this);
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			SwitchOrientation (InterfaceOrientation, false);
		}

		public override void WillRotate (UIInterfaceOrientation toInterfaceOrientation, double duration)
		{
			base.WillRotate (toInterfaceOrientation, duration);

			SwitchOrientation (toInterfaceOrientation, true, duration);
		}

		private void SwitchOrientation(UIInterfaceOrientation orientation, bool animated, double duration = .5)
		{
			if (animated)
			{
				UIView.BeginAnimations ("SwitchOrientation");
				UIView.SetAnimationDuration (duration);
				UIView.SetAnimationCurve (UIViewAnimationCurve.EaseInOut);
			}

			if (orientation.IsLandscape ())
			{
				if (!wasLandscape)
				{
					//Slide the masterView inward
					var frame = masterView.Frame;
					frame.X = 0;
					masterView.Frame = frame;

					//Shrink the detailView
					frame = detailView.Frame;
					frame.X += masterWidth;
					frame.Width -= masterWidth;
					detailView.Frame = frame;

					wasLandscape = true;
				}
			}
			else
			{
				if (wasLandscape)
				{
					//Slide the masterView off screen
					var frame = masterView.Frame;
					frame.X = -frame.Width;
					masterView.Frame = frame;

					//Grow the detailView
					frame = detailView.Frame;
					frame.X -= masterWidth;
					frame.Width += masterWidth;
					detailView.Frame = frame;

					wasLandscape = false;
				}
			}

			if (animated)
			{
				UIView.CommitAnimations ();
			}
		}

		/// <summary>
		/// Hides the popover if it is present
		/// </summary>
		public void HidePopover()
		{
			if (popover != null)
				popover.Dismiss (true);
		}

		/// <summary>
		/// Delegate for split view controller
		/// </summary>
		private class SplitDelegate : UISplitViewControllerDelegate
		{
			readonly SplitController mainController;
			readonly AssignmentDetailsController detailsController;

			public SplitDelegate ()
			{
				mainController = ServiceContainer.Resolve<SplitController>();
				detailsController = ServiceContainer.Resolve<AssignmentDetailsController>();
			}

			public override void WillHideViewController (UISplitViewController svc, UIViewController aViewController, UIBarButtonItem barButtonItem, UIPopoverController pc)
			{
				//Add a UIBarButtonItem for the master controller in portrait
				mainController.popover = pc;
				barButtonItem.Title = "Menu";
				barButtonItem.SetBackgroundImage (Theme.DarkBarButtonItem, UIControlState.Normal, UIBarMetrics.Default);
				barButtonItem.SetTitleTextAttributes (new UITextAttributes() { TextColor = UIColor.White }, UIControlState.Normal);

				detailsController.NavigationItem.SetLeftBarButtonItem(barButtonItem, true);
			}

			public override void WillShowViewController (UISplitViewController svc, UIViewController aViewController, UIBarButtonItem button)
			{
				//Hide the UIBarButtonItem
				mainController.popover = null;
				detailsController.NavigationItem.SetLeftBarButtonItem(null, true);
			}
		}
	}
}

