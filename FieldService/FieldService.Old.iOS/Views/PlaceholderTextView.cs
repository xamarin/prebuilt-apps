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

namespace FieldService.iOS
{
	/// <summary>
	/// A quick subclass of UITextView to enable placeholder text
	/// </summary>
	[Register("PlaceholderTextView")]
	public class PlaceholderTextView : UITextView
	{
		public PlaceholderTextView ()
		{
			Initialize ();
		}

		public PlaceholderTextView (RectangleF frame)
			: base(frame)
		{
			Initialize ();
		}

		public PlaceholderTextView (IntPtr handle)
			: base(handle)
		{
			Initialize ();
		}

		private void Initialize()
		{
			Placeholder = "Please enter text";

			ShouldBeginEditing = t => {
				if (Text == Placeholder) {
					Text = string.Empty;
				}
				return true;
			};
			ShouldEndEditing = t => {
				if (string.IsNullOrEmpty (Text)) {
					Text = Placeholder;
				}
				return true;
			};
		}

		/// <summary>
		/// Gets or sets the placeholder to show prior to editing - doesn't exist on UITextView by default
		/// </summary>
		public string Placeholder {
			get;
			set;
		}
	}
}

