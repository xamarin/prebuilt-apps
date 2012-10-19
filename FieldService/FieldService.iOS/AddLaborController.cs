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
using FieldService.Utilities;

namespace FieldService.iOS
{
	/// <summary>
	/// Controller for manually adding labor to an assignment
	/// </summary>
	public partial class AddLaborController : BaseController
	{
		public AddLaborController (IntPtr handle) : base (handle)
		{
			ServiceContainer.Register (this);
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			cancel.SetTitleTextAttributes (new UITextAttributes() { TextColor = UIColor.White }, UIControlState.Normal);
			cancel.SetBackgroundImage (Theme.BarButtonItem, UIControlState.Normal, UIBarMetrics.Default);
			
			var label = new UILabel (new RectangleF(0, 0, 80, 36)) { 
				Text = "Labor",
				TextColor = UIColor.White,
				BackgroundColor = UIColor.Clear,
				Font = Theme.BoldFontOfSize (16),
			};
			var labor = new UIBarButtonItem(label);
			
			toolbar.Items = new UIBarButtonItem[] {
				cancel,
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				labor,
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
			};
		}

		partial void Cancel (NSObject sender)
		{
			DismissViewController (true, delegate {	});
		}
	}
}
