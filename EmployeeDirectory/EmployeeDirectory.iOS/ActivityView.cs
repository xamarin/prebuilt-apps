//
//  Copyright 2012, Xamarin Inc.
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
//
using System;

using UIKit;
using CoreGraphics;

namespace EmployeeDirectory.iOS
{
	public class ActivityView : UIView
	{
		UIActivityIndicatorView activity;
		UILabel label;

		public string Text {
			get {
				return label.Text;
			}
			set {
				label.Text = value;
			}
		}

		public bool IsRunning { get; private set; }

		public ActivityView ()
			: base (new CGRect (0f, 0f, 88f, 88f))
		{
			BackgroundColor = UIColor.Black.ColorWithAlpha (0.75f);
			Opaque = false;
			IsRunning = false;
			Layer.CornerRadius = 8f;
			AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin;

			var b = Bounds;

			label = new UILabel (new CGRect (8f, b.Height - 28f, b.Width - 16f, 20f)) {
				TextColor = UIColor.White,
				BackgroundColor = UIColor.Clear,
				Font = UIFont.BoldSystemFontOfSize (UIFont.SmallSystemFontSize),
			};
			AddSubview (label);

			activity = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.WhiteLarge);
			var af = activity.Frame;
			af.X = (b.Width - af.Width) / 2f;
			af.Y = 12f;
			activity.Frame = af;
			AddSubview (activity);
		}

		public void StartInView (UIView superview)
		{
			var b = superview.Bounds;
			var f = Frame;
			f.X = (b.Width - f.Width) / 2f;
			f.Y = 33f;
			Frame = f;

			activity.StartAnimating ();
			superview.AddSubview (this);

			IsRunning = true;
		}

		public void Stop ()
		{
			activity.StopAnimating ();
			RemoveFromSuperview ();

			IsRunning = false;
		}
	}
}

