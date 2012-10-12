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
		const int Spacing = 5;
		const int IconBackgroundSize = 30;
		readonly UIImageView iconBackground;
		readonly UIImageView icon;

		public TextButton (IntPtr handle) : base (handle)
		{
			iconBackground = new UIImageView (new RectangleF (Spacing, Spacing, IconBackgroundSize, IconBackgroundSize))
			{
				Image = Theme.SmallGreyButton,
			};
			AddSubview (iconBackground);

			icon = new UIImageView ();
			AddSubview (icon);

			TopLabel = new UILabel { BackgroundColor = UIColor.Clear };
			AddSubview (TopLabel);

			BottomLabel = new UILabel { BackgroundColor = UIColor.Clear };
			AddSubview (BottomLabel);
		}

		public UIImage IconImage {
			get;
			set;
		}

		public UILabel TopLabel {
			get;
			private set;
		}

		public UILabel BottomLabel {
			get;
			private set;
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			if (IconImage != null) {
				var iconBackgroundFrame = iconBackground.Frame;
				var size = IconImage.Size;
				icon.Image = IconImage;
				icon.Frame = new RectangleF (new PointF (
					iconBackgroundFrame.X + (iconBackgroundFrame.Width - size.Width) / 2, 
					iconBackgroundFrame.Y + (iconBackgroundFrame.Height - size.Height) / 2), size);
			}

			var frame = Frame;
			float x = IconBackgroundSize + Spacing * 2,
				width = frame.Width - x - Spacing,
				height = frame.Height / 2 - Spacing * 2;
			TopLabel.Frame = new RectangleF(x, Spacing, width, height);
			BottomLabel.Frame = new RectangleF(x, frame.Height - height - Spacing, width, height);
		}

		protected override void Dispose (bool disposing)
		{
			iconBackground.Dispose ();
			icon.Dispose ();
			TopLabel.Dispose ();
			BottomLabel.Dispose ();

			base.Dispose (disposing);
		}
	}
}
