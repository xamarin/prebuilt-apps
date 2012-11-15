using System;
using MonoTouch.UIKit;
using System.Drawing;
using System.Collections.Generic;

namespace Xamarin.Themes
{
	public class PrebuiltAppTheme
	{
		/// <summary>
		/// Apply this theme to everything that it can.
		/// </summary>
		public static void Apply (string options = null)
		{
			Apply (UINavigationBar.Appearance, options);
			Apply (UITabBar.Appearance, options);
			Apply (UIToolbar.Appearance, options);
			Apply (UIBarButtonItem.Appearance, options);
			Apply (UISlider.Appearance, options);
			Apply (UISegmentedControl.Appearance, options);
			Apply (UIProgressView.Appearance, options);
			Apply (UISearchBar.Appearance, options);
			Apply (UISwitch.Appearance, options);
			Apply (UIRefreshControl.Appearance, options);
		}

		#region UIView

		/// <summary>
		/// Apply this theme to a specific view.
		/// </summary>
		public static void Apply (UIView view, string options = null)
		{
		}

		/// <summary>
		/// Apply this theme to all views with the given appearance.
		/// </summary>
		/// <param name='options'>
		/// Can be "gunmetal", "mesh", or null
		/// </param>
		public static void Apply (UIView.UIViewAppearance appearance, string options = null)
		{
		}

		#endregion

		#region UINavigationBar

		static Lazy<UIImage> navigationBarBackground = new Lazy<UIImage> (
			() => UIImage.FromBundle ("PrebuiltAppImages/topnav.png"));

		/// <summary>
		/// Apply this theme to a specific view.
		/// </summary>
		public static void Apply (UINavigationBar view, string options = null)
		{
			view.SetBackgroundImage (navigationBarBackground.Value, UIBarMetrics.Default);
			view.SetTitleTextAttributes (new UITextAttributes {
				TextColor = UIColor.FromRGBA (255, 255, 255, 255),
				TextShadowColor = UIColor.FromRGBA (0, 0, 0, 0.8f),
				TextShadowOffset = new UIOffset (0, -1),
			});
		}

		/// <summary>
		/// Apply this theme to all views with the given appearance.
		/// </summary>
		public static void Apply (UINavigationBar.UINavigationBarAppearance appearance, string options = null)
		{
			appearance.SetBackgroundImage (navigationBarBackground.Value, UIBarMetrics.Default);
			appearance.SetTitleTextAttributes (new UITextAttributes {
				TextColor = UIColor.FromRGBA (255, 255, 255, 255),
				TextShadowColor = UIColor.FromRGBA (0, 0, 0, 0.8f),
				TextShadowOffset = new UIOffset (0, -1),
			});
		}

		#endregion

		#region UITabBar

		static Lazy<UIImage> tabBarBackground = new Lazy<UIImage> (() => new UIImage ("SocialBooImages/tab-bar.png"));
		static Lazy<UIImage> tabBarSelectionIndicator = new Lazy<UIImage> (() => new UIImage ("SocialBooImages/menu-bar-item-bg.png"));

		/// <summary>
		/// Apply this theme to a specific view.
		/// </summary>
		public static void Apply (UITabBar view, string options = null)
		{
			view.BackgroundImage = tabBarBackground.Value;
			view.SelectionIndicatorImage = tabBarSelectionIndicator.Value;
		}

		/// <summary>
		/// Apply this theme to all views with the given appearance.
		/// </summary>
		public static void Apply (UITabBar.UITabBarAppearance appearance, string options = null)
		{
			appearance.BackgroundImage = tabBarBackground.Value;
			appearance.SelectionIndicatorImage = tabBarSelectionIndicator.Value;
		}

		#endregion

		#region UIToolbar

		/// <summary>
		/// Apply this theme to a specific view.
		/// </summary>
		public static void Apply (UIToolbar view, string options = null)
		{
			view.SetBackgroundImage (navigationBarBackground.Value, UIToolbarPosition.Any, UIBarMetrics.Default);
		}

		/// <summary>
		/// Apply this theme to all views with the given appearance.
		/// </summary>
		public static void Apply (UIToolbar.UIToolbarAppearance appearance, string options = null)
		{
			appearance.SetBackgroundImage (navigationBarBackground.Value, UIToolbarPosition.Any, UIBarMetrics.Default);
		}

		#endregion

		#region UIBarButtonItem

		static Lazy<UIImage> barButtonBackground = new Lazy<UIImage> (() => new UIImage ("SocialBooImages/menu-bar-item-bg.png").CreateResizableImage (new UIEdgeInsets (0, 4, 0, 4)));
		static Lazy<UIImage> backButtonBackBackground = new Lazy<UIImage> (() => new UIImage ("SocialBooImages/back-btn-big.png").CreateResizableImage (new UIEdgeInsets (0, 14, 0, 4)));

		/// <summary>
		/// Apply this theme to a specific view.
		/// </summary>
		public static void Apply (UIBarButtonItem view, string options = null)
		{
			view.SetBackgroundImage (barButtonBackground.Value, UIControlState.Normal, UIBarMetrics.Default);
			view.SetBackButtonBackgroundImage (backButtonBackBackground.Value, UIControlState.Normal, UIBarMetrics.Default);
		}

		/// <summary>
		/// Apply this theme to all views with the given appearance.
		/// </summary>
		public static void Apply (UIBarButtonItem.UIBarButtonItemAppearance appearance, string options = null)
		{
			appearance.SetBackgroundImage (barButtonBackground.Value, UIControlState.Normal, UIBarMetrics.Default);
			appearance.SetBackButtonBackgroundImage (backButtonBackBackground.Value, UIControlState.Normal, UIBarMetrics.Default);
		}

		#endregion

		#region UIButton

		class ButtonStyle
		{
			public float Height { get; private set; }

			Lazy<UIImage> image;
			public UIImage BackgroundImage { get { return image.Value; } }

			public ButtonStyle (string name, float height)
			{
				Height = height;
				image = new Lazy<UIImage> (() => new UIImage ("SocialBooImages/" + name + ".png").CreateResizableImage (new UIEdgeInsets (0, 8, 0, 8)));
			}
		}

		static readonly Dictionary<string, ButtonStyle> buttonStyles = new Dictionary<string, ButtonStyle> {
			{ "", new ButtonStyle ("standard-big", 47) },
			{ "normal", new ButtonStyle ("standard-big", 47) },
			{ "standard-big", new ButtonStyle ("standard-big", 47) },
			{ "standard-big-over", new ButtonStyle ("standard-big-over", 47) },
			{ "confirm", new ButtonStyle ("confirm-big", 47) },
			{ "confirm-big", new ButtonStyle ("confirm-big", 47) },
			{ "confirm-small", new ButtonStyle ("confirm-small", 47) },
			{ "cancel", new ButtonStyle ("cancel-big", 47) },
			{ "cancel-big", new ButtonStyle ("cancel-big", 47) },
			{ "cancel-small", new ButtonStyle ("cancel-small", 47) },
		};

		/// <summary>
		/// Apply this theme to a specific view.
		/// </summary>
		/// <param name='options'>
		/// Can be null, "normal", "standard-big", "standard-big-over", "confirm", "confirm-big", "confirm-small", "cancel", "cancel-big", "cancel-small"
		/// </param>
		public static void Apply (UIButton view, string options = null)
		{
			var appearance = view;

			var key = string.IsNullOrEmpty (options) ? "" : options;
			ButtonStyle style;
			if (!buttonStyles.TryGetValue (key, out style)) {
				style = buttonStyles [""];
			}

			appearance.SetBackgroundImage (style.BackgroundImage, UIControlState.Normal);
			appearance.SetTitleColor (UIColor.White, UIControlState.Normal);
			appearance.SetTitleShadowColor (UIColor.DarkGray, UIControlState.Normal);
			appearance.SetTitleColor (UIColor.White, UIControlState.Highlighted);
			appearance.SetTitleShadowColor (UIColor.DarkGray, UIControlState.Highlighted);

			var f = view.Frame;
			f.Height = style.Height;
			view.Frame = f;
		}

		/// <summary>
		/// Apply this theme to all views with the given appearance.
		/// </summary>
		/// <param name='options'>
		/// Can be null, "normal", "standard-big", "standard-big-over", "confirm", "confirm-big", "confirm-small", "cancel", "cancel-big", "cancel-small"
		/// </param>
		public static void Apply (UIButton.UIButtonAppearance appearance, string options = null)
		{
			var key = string.IsNullOrEmpty (options) ? "" : options;
			ButtonStyle style;
			if (!buttonStyles.TryGetValue (key, out style)) {
				style = buttonStyles [""];
			}

			appearance.SetBackgroundImage (style.BackgroundImage, UIControlState.Normal);
			appearance.SetTitleColor (UIColor.White, UIControlState.Normal);
			appearance.SetTitleShadowColor (UIColor.DarkGray, UIControlState.Normal);
			appearance.SetTitleColor (UIColor.White, UIControlState.Highlighted);
			appearance.SetTitleShadowColor (UIColor.DarkGray, UIControlState.Highlighted);
		}

		#endregion

		#region UISlider

		static Lazy<UIImage> sliderMinTrack = new Lazy<UIImage> (() => new UIImage ("SocialBooImages/slider-bg.png").CreateResizableImage (new UIEdgeInsets (0, 5, 0, 5)));
		static Lazy<UIImage> sliderMaxTrack = new Lazy<UIImage> (() => new UIImage ("SocialBooImages/slider-fill.png").CreateResizableImage (new UIEdgeInsets (0, 5, 0, 5)));
		static Lazy<UIImage> sliderThumb = new Lazy<UIImage> (() => new UIImage ("SocialBooImages/slider-cap.png"));

		/// <summary>
		/// Apply this theme to a specific view.
		/// </summary>
		public static void Apply (UISlider view, string options = null)
		{
			view.SetMaxTrackImage (sliderMaxTrack.Value, UIControlState.Normal);
			view.SetMinTrackImage (sliderMinTrack.Value, UIControlState.Normal);
			view.SetThumbImage (sliderThumb.Value, UIControlState.Normal);
			view.SetThumbImage (sliderThumb.Value, UIControlState.Selected);
		}

		/// <summary>
		/// Apply this theme to all views with the given appearance.
		/// </summary>
		public static void Apply (UISlider.UISliderAppearance appearance, string options = null)
		{
			appearance.SetMaxTrackImage (sliderMaxTrack.Value, UIControlState.Normal);
			appearance.SetMinTrackImage (sliderMinTrack.Value, UIControlState.Normal);
			appearance.SetThumbImage (sliderThumb.Value, UIControlState.Normal);
			appearance.SetThumbImage (sliderThumb.Value, UIControlState.Highlighted);
		}

		#endregion

		#region UILabel

		/// <summary>
		/// Apply this theme to a specific view.
		/// </summary>
		public static void Apply (UILabel view, string options = null)
		{
			view.TextColor = UIColor.White;
			view.BackgroundColor = UIColor.Clear;
			view.Font = UIFont.SystemFontOfSize (17);
			view.ShadowColor = UIColor.Clear;
			view.ShadowOffset = new SizeF (0, 1);
		}

		/// <summary>
		/// Apply this theme to all views with the given appearance.
		/// </summary>
		public static void Apply (UILabel.UILabelAppearance appearance, string options = null)
		{
			appearance.TextColor = UIColor.White;
			appearance.BackgroundColor = UIColor.Clear;
			appearance.Font = UIFont.SystemFontOfSize (17);
			appearance.ShadowColor = UIColor.Clear;
			appearance.ShadowOffset = new SizeF (0, 1);
		}

		#endregion

		#region UITextField

		static Lazy<UIImage> textBackground = new Lazy<UIImage> (() => new UIImage ("SocialBooImages/text-input.png").CreateResizableImage (new UIEdgeInsets (0, 5, 0, 5)));

		/// <summary>
		/// Apply this theme to a specific view.
		/// </summary>
		public static void Apply (UITextField view, string options = null)
		{
			view.Background = textBackground.Value;
			view.BorderStyle = UITextBorderStyle.None;
			view.TextColor = UIColor.White;
			view.LeftView = new UIView (new RectangleF (0, 0, 8, 8));
			view.LeftViewMode = UITextFieldViewMode.Always;
		}

		/// <summary>
		/// Apply this theme to all views with the given appearance.
		/// </summary>
		public static void Apply (UITextField.UITextFieldAppearance appearance, string options = null)
		{
		}

		#endregion

		#region UISegmentedControl

		/// <summary>
		/// Apply this theme to a specific view.
		/// </summary>
		public static void Apply (UISegmentedControl view, string options = null)
		{
			view.TintColor = UIColor.FromRGB (49, 53, 67);
		}

		/// <summary>
		/// Apply this theme to all views with the given appearance.
		/// </summary>
		public static void Apply (UISegmentedControl.UISegmentedControlAppearance appearance, string options = null)
		{
			appearance.TintColor = UIColor.FromRGB (49, 53, 67);
		}

		#endregion

		#region UIProgressView

		/// <summary>
		/// Apply this theme to a specific view.
		/// </summary>
		public static void Apply (UIProgressView view, string options = null)
		{
		}

		/// <summary>
		/// Apply this theme to all views with the given appearance.
		/// </summary>
		public static void Apply (UIProgressView.UIProgressViewAppearance appearance, string options = null)
		{
		}

		#endregion

		#region UISearchBar

		/// <summary>
		/// Apply this theme to a specific view.
		/// </summary>
		public static void Apply (UISearchBar view, string options = null)
		{
			view.TintColor = UIColor.FromRGB (49, 53, 67);
		}

		/// <summary>
		/// Apply this theme to all views with the given appearance.
		/// </summary>
		public static void Apply (UISearchBar.UISearchBarAppearance appearance, string options = null)
		{
			appearance.TintColor = UIColor.FromRGB (49, 53, 67);
		}

		#endregion

		#region UISwitch

		/// <summary>
		/// Apply this theme to a specific view.
		/// </summary>
		public static void Apply (UISwitch view, string options = null)
		{
			view.OnTintColor = UIColor.FromRGB (0, 136, 233);
		}

		/// <summary>
		/// Apply this theme to all views with the given appearance.
		/// </summary>
		public static void Apply (UISwitch.UISwitchAppearance appearance, string options = null)
		{
			appearance.OnTintColor = UIColor.FromRGB (0, 136, 233);
		}

		#endregion

		#region UIRefreshControl

		/// <summary>
		/// Apply this theme to a specific view.
		/// </summary>
		public static void Apply (UIRefreshControl view, string options = null)
		{
			view.TintColor = UIColor.FromRGB (94, 111, 125);
		}

		/// <summary>
		/// Apply this theme to all views with the given appearance.
		/// </summary>
		public static void Apply (UIRefreshControl.UIRefreshControlAppearance appearance, string options = null)
		{
			appearance.TintColor = UIColor.FromRGB (94, 111, 125);
		}

		#endregion
	}
}
