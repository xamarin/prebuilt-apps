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
	/// This is a cell used for documents, only subclassed so we could override LayoutSubviews and move things around
	/// </summary>
	public class DocumentCell : UITableViewCell
	{
		const int Spacing = 35;

		public DocumentCell (string identifier)
			: base(UITableViewCellStyle.Default, identifier)
		{
			BackgroundView = new UIImageView { Image = Theme.Row };
			SelectedBackgroundView = new UIImageView { Image = Theme.RowPress };
			TextLabel.TextColor = Theme.LabelColor;
			ImageView.Image = Theme.IconPdf;
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			var frame = ImageView.Frame;
			frame.X = Spacing;
			ImageView.Frame = frame;

			frame = TextLabel.Frame;
			frame.X += Spacing;
			TextLabel.Frame = frame;
		}
	}
}

