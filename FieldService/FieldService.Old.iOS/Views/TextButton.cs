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
	/// This is a button for providing 2 lines of text and a nice icon to the left
	/// </summary>
	public partial class TextButton : UIButton
	{
		const int Spacing = 4;
		const int IconBackgroundSize = 30;
		readonly UIButton icon;

		public TextButton (IntPtr handle) : base (handle)
		{
			icon = UIButton.FromType (UIButtonType.Custom);
			icon.Frame = new RectangleF (Spacing, Spacing, IconBackgroundSize, IconBackgroundSize);
			icon.SetBackgroundImage (Theme.SmallGreyButton, UIControlState.Normal);
			icon.UserInteractionEnabled = false;
			AddSubview (icon);

			TopLabel = new UILabel { 
				BackgroundColor = UIColor.Clear, 
				Font = Theme.BoldFontOfSize (11), 
				TextColor = Theme.LabelColor,
				HighlightedTextColor = UIColor.Black,
			};
			AddSubview (TopLabel);

			BottomLabel = new UILabel { 
				BackgroundColor = UIColor.Clear, 
				Font = Theme.FontOfSize (11), 
				TextColor = Theme.LabelColor,
				HighlightedTextColor = UIColor.Black,
			};
			AddSubview (BottomLabel);
		}

		/// <summary>
		/// Icon image used on the button to the left
		/// </summary>
		public UIImage IconImage {
			get { return icon.CurrentImage; }
			set { icon.SetImage (value, UIControlState.Normal); }
		}

		/// <summary>
		/// Top label text
		/// </summary>
		public UILabel TopLabel {
			get;
			private set;
		}

		/// <summary>
		/// Second label text
		/// </summary>
		public UILabel BottomLabel {
			get;
			private set;
		}

		/// <summary>
		/// Override Highlighted so that the child controls function correctly
		/// </summary>
		public override bool Highlighted {
			get {
				return base.Highlighted;
			}
			set {
				base.Highlighted = 
					icon.Highlighted =
					TopLabel.Highlighted =
					BottomLabel.Highlighted = value;
			}
		}

		/// <summary>
		/// Set correct spacing for subviews
		/// </summary>
		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			var frame = Frame;
			float x = IconBackgroundSize + Spacing * 2,
				width = frame.Width - x - Spacing,
				height = frame.Height / 2 - Spacing;
			TopLabel.Frame = new RectangleF(x, Spacing, width, height);
			BottomLabel.Frame = new RectangleF(x, frame.Height - height - Spacing / 2, width, height);
		}

		protected override void Dispose (bool disposing)
		{
			icon.Dispose ();
			TopLabel.Dispose ();
			BottomLabel.Dispose ();

			base.Dispose (disposing);
		}
	}
}
