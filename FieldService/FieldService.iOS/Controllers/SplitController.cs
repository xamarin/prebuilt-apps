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
		private UIBarButtonItem menu, hide;
		private bool wasLandscape = true, masterPopoverShown = false;
		private const float masterWidth = 321;

		public SplitController (IntPtr handle) : base(handle)
		{
			ServiceContainer.Register (this);
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			NavigationItem.LeftItemsSupplementBackButton = true;
			menu = new UIBarButtonItem("Menu", UIBarButtonItemStyle.Bordered, (sender, e) => ShowPopover ());
			menu.SetBackgroundImage (Theme.DarkBarButtonItem, UIControlState.Normal, UIBarMetrics.Default);
			hide = new UIBarButtonItem("Hide", UIBarButtonItemStyle.Bordered, (sender, e) => HidePopover ());
			hide.SetBackgroundImage (Theme.DarkBarButtonItem, UIControlState.Normal, UIBarMetrics.Default);
			SwitchOrientation (InterfaceOrientation, false);
		}

		public override void WillRotate (UIInterfaceOrientation toInterfaceOrientation, double duration)
		{
			base.WillRotate (toInterfaceOrientation, duration);

			SwitchOrientation (toInterfaceOrientation, true, duration);
		}

		public void ShowPopover()
		{
			if (!masterPopoverShown)
			{
				NavigationItem.SetLeftBarButtonItems(new UIBarButtonItem[] { hide }, true);
				AnimateMasterView (true);
			}
		}

		public void HidePopover()
		{
			if (masterPopoverShown)
			{
				NavigationItem.SetLeftBarButtonItems(new UIBarButtonItem[] { menu }, true);
				AnimateMasterView (false);
			}
		}

		private void AnimateMasterView(bool visible)
		{
			UIView.BeginAnimations ("SwitchOrientation");
			UIView.SetAnimationDuration (.3);
			UIView.SetAnimationCurve (UIViewAnimationCurve.EaseInOut);

			var frame = masterView.Frame;
			frame.X = visible ? 0 : -masterWidth;
			masterView.Frame = frame;

			UIView.CommitAnimations ();
			masterPopoverShown = visible;
		}

		private void SwitchOrientation(UIInterfaceOrientation orientation, bool animated, double duration = .5)
		{
			if (orientation.IsLandscape ())
			{
				if (!wasLandscape)
				{
					//Set the navbar to have only the back button
					NavigationItem.SetLeftBarButtonItems(new UIBarButtonItem[0], true);

					//Hide the master view if needed
					if (masterPopoverShown) {
						AnimateMasterView (false);
					}

					if (animated)
					{
						UIView.BeginAnimations ("SwitchOrientation");
						UIView.SetAnimationDuration (duration);
						UIView.SetAnimationCurve (UIViewAnimationCurve.EaseInOut);
					}

					//Slide the masterView inward
					var frame = masterView.Frame;
					frame.X = 0;
					masterView.Frame = frame;

					//Shrink the detailView
					frame = detailView.Frame;
					frame.X += masterWidth;
					frame.Width -= masterWidth;
					detailView.Frame = frame;

					if (animated)
					{
						UIView.CommitAnimations ();
					}
					wasLandscape = true;
				}
			}
			else
			{
				if (wasLandscape)
				{
					//Set the nav bar to include the menu button
					NavigationItem.SetLeftBarButtonItems(new UIBarButtonItem[] { menu }, true);

					if (animated)
					{
						UIView.BeginAnimations ("SwitchOrientation");
						UIView.SetAnimationDuration (duration);
						UIView.SetAnimationCurve (UIViewAnimationCurve.EaseInOut);
					}

					//Slide the masterView off screen
					var frame = masterView.Frame;
					frame.X = -frame.Width;
					masterView.Frame = frame;

					//Grow the detailView
					frame = detailView.Frame;
					frame.X -= masterWidth;
					frame.Width += masterWidth;
					detailView.Frame = frame;

					if (animated)
					{
						UIView.CommitAnimations ();
					}
					wasLandscape = false;
				}
			}
		}

		public override void TouchesEnded (NSSet touches, UIEvent evt)
		{
			base.TouchesEnded (touches, evt);

			if (masterPopoverShown && evt.TouchesForView (masterView) == null)
			{
				HidePopover ();
			}
		}
	}
}

