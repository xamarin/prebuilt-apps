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
using CoreGraphics;
using Foundation;
using UIKit;

namespace FieldService.iOS
{
	/// <summary>
	/// Base navigation controller, used in a couple places throughout our Storyboard file
	/// </summary>
	public partial class BaseNavigationController : UINavigationController
	{
		public BaseNavigationController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//Setting for our navigation bar
			NavigationBar.SetBackgroundImage (Theme.TopNav, UIBarMetrics.Default);
			NavigationBar.TitleTextAttributes = new UIStringAttributes {
				ForegroundColor = UIColor.White,
				Shadow = new NSShadow { ShadowColor = Theme.LabelColor }
			};
		}

		public override UIStatusBarStyle PreferredStatusBarStyle ()
		{
			return UIStatusBarStyle.LightContent;
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
