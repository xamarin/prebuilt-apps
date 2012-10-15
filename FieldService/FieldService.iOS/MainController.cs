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

namespace FieldService.iOS
{
	[Register("MainController")]
	public class MainController : UISplitViewController
	{
		bool loggedIn = false;

		public MainController (IntPtr handle) : base(handle)
		{
			//Hook up callback to hide master view on certain controllers
			ShouldHideViewController = (svc, viewController, orientation) => svc.ViewControllers [1] is AssignmentsController;
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			if (!loggedIn) {
				PerformSegue ("Login", this);

				loggedIn = true;
			}
		}

		public override void PerformSegue (string identifier, NSObject sender)
		{
			base.PerformSegue (identifier, sender);

			//Force ShouldHideViewController to refresh
			DidRotate (InterfaceOrientation);
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
	}
}

