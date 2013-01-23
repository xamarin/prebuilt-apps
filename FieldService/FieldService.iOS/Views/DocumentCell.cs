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
using System.IO;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using FieldService.Data;

namespace FieldService.iOS
{
	/// <summary>
	/// This is a cell used for documents, only subclassed so we could override LayoutSubviews and move things around
	/// </summary>
	public class DocumentCell : UITableViewCell
	{
		const int Spacing = 35;
		static readonly string tempPath;

		Document document;

		static DocumentCell ()
		{
			//Make a temporary path for copying files to
			tempPath = Path.Combine (Path.GetTempPath (), "temp.pdf");
		}

		public DocumentCell (string identifier)
			: base(UITableViewCellStyle.Default, identifier)
		{
			BackgroundView = new UIImageView { Image = Theme.Row };
			SelectedBackgroundView = new UIImageView { Image = Theme.RowPress };
			TextLabel.TextColor = Theme.LabelColor;
			ImageView.Image = Theme.IconPdf;
			Accessory = UITableViewCellAccessory.DisclosureIndicator;
		}

		public UIDocumentInteractionController DocumentController {
			get;
			set;
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

		public void SetDocument(Document document)
		{
			this.document = document;
			TextLabel.Text = document.Title;
		}

		public void Clicked ()
		{
			if (document != null && DocumentController != null) {

				//We have to copy the file to the temp directory, otherwise the new process can't access the file
				File.Copy (document.Path, tempPath, true);

				DocumentController.Url = NSUrl.FromFilename (tempPath);
				if (!DocumentController.PresentOpenInMenu (Frame, Superview, true))
				{
					new UIAlertView("Error", "Sorry, there is not an app installed that can view PDFs.", null, "Ok").Show ();
				}
			}

			//Deselect the cell, a bug in Apple's UITableView requires BeginInvoke
			BeginInvokeOnMainThread (() => SetSelected (false, true));
		}
	}
}

