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
using FieldService.Data;

namespace FieldService.iOS
{
	/// <summary>
	/// Table cell for a photo on an assignment
	/// </summary>
	public partial class PhotoCell : UITableViewCell
	{
		UIImage image;

		public PhotoCell (IntPtr handle) : base (handle) { }

		public override void AwakeFromNib ()
		{
			base.AwakeFromNib ();

			if (Theme.IsiOS7) {
				SelectionStyle = UITableViewCellSelectionStyle.Blue;
				SelectedBackgroundView = new UIView { BackgroundColor = UIColor.Clear };
				BackgroundView = new UIView { BackgroundColor = Theme.BackgroundColor };

				date.TextColor =
					description.TextColor = Theme.LabelColor;
				date.Font = Theme.FontOfSize (18);
				description.Font = Theme.FontOfSize (14);

				//Change the image frame
				var frame = photoFrame.Frame;
				frame.Y = 0;
				frame.Height = Frame.Height;
				frame.Width -= 12;
				photo.Frame = frame;

				//Changes to widths on text
				frame = date.Frame;
				frame.Width -= 15;
				date.Frame = frame;

				frame = description.Frame;
				frame.Width -= 15;
				description.Frame = frame;

			} else {
				BackgroundView = new UIImageView { Image = Theme.Inlay };
				photoFrame.Image = Theme.PhotoFrame;
			}
		}

		/// <summary>
		/// Set the photo for this cell
		/// </summary>
		public void SetPhoto(Photo photo)
		{
			//Free up the previous image if there was one
			if (image != null)
				image.Dispose ();

			date.Text = photo.Date.ToShortTimeString () + " " + photo.Date.ToShortDateString ();
			description.Text = photo.Description;
			this.photo.Image = 
				image = photo.Image.ToUIImage ();
		}

		protected override void Dispose (bool disposing)
		{
			if (image != null) {
				image.Dispose ();
				image = null;
			}
			ReleaseDesignerOutlets ();

			base.Dispose (disposing);
		}
	}
}
