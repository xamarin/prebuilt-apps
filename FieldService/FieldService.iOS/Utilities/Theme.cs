using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace FieldService.iOS
{
	/// <summary>
	/// Theme class for appling UIAppearance to the app, and holding app level resources (colors and images)
	/// * Notice use of Lazy&lt;T&gt;, so images are not loaded until requested
	/// </summary>
	public static class Theme
	{
		#region Images

		static Lazy<UIImage> linenPattern = new Lazy<UIImage>(() => UIImage.FromFile("Images/linenpattern.png").CreateResizableImage (new UIEdgeInsets()));

		/// <summary>
		/// Linen pattern background for use throughout the app
		/// </summary>
		public static UIImage LinenPattern
		{
			get { return linenPattern.Value; }
		}

		static Lazy<UIImage> loginBox = new Lazy<UIImage>(() => UIImage.FromFile("Images/login_box.png").CreateResizableImage (new UIEdgeInsets(21, 21, 21, 21)));

		/// <summary>
		/// Login box on first screen
		/// </summary>
		public static UIImage LoginBox
		{
			get { return loginBox.Value; }
		}

		static Lazy<UIImage> loginButton = new Lazy<UIImage>(() => UIImage.FromFile("Images/login_btn.png").CreateResizableImage (new UIEdgeInsets(11, 8, 11, 8)));

		/// <summary>
		/// Login button on first screen
		/// </summary>
		public static UIImage LoginButton
		{
			get { return loginButton.Value; }
		}

		static Lazy<UIImage> loginTextField = new Lazy<UIImage>(() => UIImage.FromFile("Images/login_textfield.png").CreateResizableImage (new UIEdgeInsets(10, 10, 10, 10)));

		/// <summary>
		/// Login text field on first screen
		/// </summary>
		public static UIImage LoginTextField
		{
			get { return loginTextField.Value; }
		}

		static Lazy<UIImage> loginInset = new Lazy<UIImage>(() => UIImage.FromFile("Images/login_inset.png").CreateResizableImage (new UIEdgeInsets(12, 12, 12, 12)));

		/// <summary>
		/// Login box on first screen, used in the logo's inset
		/// </summary>
		public static UIImage LoginInset
		{
			get { return loginInset.Value; }
		}

		#endregion

		#region Colors

		static Lazy<UIColor> labelColor = new Lazy<UIColor>(() => UIColor.FromRGB (0x33, 0x33, 0x33));

		/// <summary>
		/// General label color for the entire app
		/// </summary>
		public static UIColor LabelColor 
		{
			get { return labelColor.Value; }
		}

		static Lazy<UIColor> indicatorColor = new Lazy<UIColor>(() => UIColor.FromRGB (0x77, 0x77, 0x77));

		/// <summary>
		/// General indicator color for the entire app
		/// </summary>
		public static UIColor IndicatorColor 
		{
			get { return indicatorColor.Value; }
		}

		#endregion

		/// <summary>
		/// Apply UIAppearance to this application, this is iOS's version of "styling"
		/// </summary>
		public static void Apply()
		{
			UILabel.Appearance.TextColor = LabelColor;

			UIActivityIndicatorView.Appearance.Color = IndicatorColor;
		}
	}
}

