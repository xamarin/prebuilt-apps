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

		public Action<byte[]> Callback {
			get;
			set;
		}

		/// <summary>
		/// Helper method to read the stream coming back from Xamarin.Mobile
		/// </summary>
		private void SetImage(Stream stream)
		{
			using (stream) {
				byte[] buffer = new byte[stream.Length];
				stream.Read (buffer, 0, buffer.Length);
				BeginInvokeOnMainThread (() => Callback(buffer));
			}
		}
	}
}

