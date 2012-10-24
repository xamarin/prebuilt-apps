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
	/// The main split controller in the app
	/// </summary>
	[Register("MainController")]
	public class MainController : UISplitViewController
	{
		private UIPopoverController popover;

		public MainController (IntPtr handle) : base(handle)
		{
			ServiceContainer.Register (this);
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//Setup our child controllers
			ViewControllers = new UIViewController[]
			{
				ViewControllers[0],
				new UINavigationController(ServiceContainer.Resolve<AssignmentDetailsController>()),
			};

			//Hook up our delegate
			Delegate = new SplitDelegate();
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
		/// This is how orientation is setup on iOS 6
		/// </summary>
		public override bool ShouldAutorotate ()
		{
			return true;
		}
        
		/// <summary>
		/// This is how orientation is setup on iOS 6
		/// </summary>
		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
		{
			return UIInterfaceOrientationMask.All;
		}

		/// <summary>
		/// Delegate for split view controller
		/// </summary>
		private class SplitDelegate : UISplitViewControllerDelegate
		{
			readonly MainController mainController;
			readonly AssignmentDetailsController detailsController;

			public SplitDelegate ()
			{
				mainController = ServiceContainer.Resolve<MainController>();
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

