using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace FieldService.iOS
{
	/// <summary>
	/// Theme class for appling UIAppearance to the app
	/// Also contains any images used throughout the app
	/// </summary>
	public static class Theme
	{
		static Lazy<UIImage> linenPattern = new Lazy<UIImage>(() => UIImage.FromFile("Images/linenpattern.png").CreateResizableImage (new UIEdgeInsets()));

		public static UIImage LinenPattern
		{
			get { return linenPattern.Value; }
		}

		static Lazy<UIImage> loginBox = new Lazy<UIImage>(() => UIImage.FromFile("Images/login_box.png").CreateResizableImage (new UIEdgeInsets(21, 21, 21, 21)));
		
		public static UIImage LoginBox
		{
			get { return loginBox.Value; }
		}

		static Lazy<UIImage> loginButton = new Lazy<UIImage>(() => UIImage.FromFile("Images/login_btn.png").CreateResizableImage (new UIEdgeInsets(11, 8, 11, 8)));
		
		public static UIImage LoginButton
		{
			get { return loginButton.Value; }
		}

		public static void Apply()
		{
			//TODO: make any appearance changes here
		}
	}
}

