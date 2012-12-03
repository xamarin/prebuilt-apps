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
using System.IO;
using System.Threading.Tasks;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using FieldService.Utilities;
using Xamarin.Media;

namespace FieldService.iOS
{
	/// <summary>
	/// Alert sheet for choosing a photo
	/// </summary>
	public class PhotoAlertSheet : UIActionSheet
	{
		readonly MediaPicker picker;

		public PhotoAlertSheet ()
		{
			picker = new MediaPicker ();

			AddButton ("Photo Library");
			if (picker.IsCameraAvailable)
				AddButton ("Camera");

			Dismissed += (sender, e) => {
				switch (e.ButtonIndex) {
				case 0:
					picker.PickPhotoAsync ()
						.ContinueWith (t => {

							if (t.IsCanceled)
								return null;

							SetImage(t.Result.GetStream());
							return t.Result;
						});
					break;
				case 1:
					picker.TakePhotoAsync (new StoreCameraMediaOptions())
						.ContinueWith (t => {

							if (t.IsCanceled)
								return t.Result;

							SetImage(t.Result.GetStream());
							return t.Result;
						});
					break;
				default:
					break;
				}
			};
		}

		/// <summary>
		/// Gets or sets the size to scale the image to, SizeF.Zero turns off sizing
		/// </summary>
		public SizeF DesiredSize {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the callback for when an image is selected and resized
		/// </summary>
		public Action<UIImage> Callback {
			get;
			set;
		}

		/// <summary>
		/// Helper method to read the stream coming back from Xamarin.Mobile
		/// </summary>
		private void SetImage(Stream stream)
		{
			using (stream) {
				using (var data = NSData.FromStream (stream)) {
					var image = UIImage.LoadFromData (data);
					//Check if we should scale anything or not
					if (DesiredSize.Width == 0 || DesiredSize.Height == 0 || (image.Size.Width < DesiredSize.Width && image.Size.Height < DesiredSize.Height)) {
						BeginInvokeOnMainThread (() => Callback(image));
					} else {
						//Some math to scale the image to the DesiredSize
						float scale = 1;
						if (image.Size.Width / image.Size.Height < DesiredSize.Width / DesiredSize.Height)
						{
							scale = DesiredSize.Height / image.Size.Height;
						}
						else
						{
							scale = DesiredSize.Width / image.Size.Width;
						}

						//Scale the image
						var newSize = image.Size;
						newSize.Width *= scale;
						newSize.Height *= scale;
						
						BeginInvokeOnMainThread (() => Callback(image.Scale (newSize)));
					}
				}
			}
		}
	}
}

