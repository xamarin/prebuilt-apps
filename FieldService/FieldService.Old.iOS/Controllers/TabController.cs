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
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace FieldService.iOS
{
	/// <summary>
	/// Tab controller on the assignments page
	/// </summary>
	public partial class TabController : UITabBarController
	{
		UIActionSheet actionSheet;

		public TabController (IntPtr handle) : base (handle)
		{
			Title = "Assignments";

			//Hook up a "fade" animation between tabs
			ShouldSelectViewController = (tabController, controller) =>
			{
				if (SelectedViewController == null || controller == SelectedViewController)
					return true;

				UIView fromView = SelectedViewController.View;
				UIView toView = controller.View;

				UIView.Transition (fromView, toView, .3f, UIViewAnimationOptions.TransitionCrossDissolve, null);
				return true;
			};
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			TabBar.TintColor = UIColor.FromRGB (0x28, 0x2b, 0x30);
			settings.SetBackgroundImage (Theme.DarkBarButtonItem, UIControlState.Normal, UIBarMetrics.Default);

			//Setup tab bar icons
			ViewControllers[0].TabBarItem.Image = Theme.ListIcon;
			ViewControllers[1].TabBarItem.Image = Theme.MapIcon;
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
		/// Event when the settings toolbar item is clicked
		/// </summary>
		partial void Settings (NSObject sender)
		{
			if (actionSheet == null) {
				actionSheet = new UIActionSheet();
				actionSheet.AddButton ("Logout");
				actionSheet.Dismissed += (s, e) => {
					if (e.ButtonIndex == 0) {
						var loginController = Storyboard.InstantiateViewController<LoginController>();
						Theme.TransitionController(loginController);
					}
					
					actionSheet.Dispose ();
					actionSheet = null;
				};
				actionSheet.ShowFrom (sender as UIBarButtonItem, true);
			} else {
				actionSheet.DismissWithClickedButtonIndex (-1, true);
			}
		}
	}
}
