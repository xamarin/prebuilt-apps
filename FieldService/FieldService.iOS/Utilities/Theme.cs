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
		#region Images

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

		static Lazy<UIImage> loginTextField = new Lazy<UIImage>(() => UIImage.FromFile("Images/login_textfield.png").CreateResizableImage (new UIEdgeInsets(10, 10, 10, 10)));
		
		public static UIImage LoginTextField
		{
			get { return loginTextField.Value; }
		}

		static Lazy<UIImage> loginInset = new Lazy<UIImage>(() => UIImage.FromFile("Images/login_inset.png").CreateResizableImage (new UIEdgeInsets(12, 12, 12, 12)));
		
		public static UIImage LoginInset
		{
			get { return loginInset.Value; }
		}

		#endregion

		#region Colors

		static Lazy<UIColor> labelColor = new Lazy<UIColor>(() => UIColor.FromRGB (0x33, 0x33, 0x33));

		public static UIColor LabelColor 
		{
			get { return labelColor.Value; }
		}

		#endregion

		public static void Apply()
		{
			UILabel.Appearance.TextColor = LabelColor;
		}
	}
}

