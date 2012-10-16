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
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using FieldService.Utilities;

namespace FieldService.iOS
{
	/// <summary>
	/// Theme class for appling UIAppearance to the app, and holding app level resources (colors and images)
	/// * Notice use of Lazy&lt;T&gt;, so images are not loaded until requested
	/// </summary>
	public static class Theme
	{
		#region Images

		static Lazy<UIImage> accept = new Lazy<UIImage> (() => UIImage.FromFile ("Images/accept.png").CreateResizableImage (new UIEdgeInsets (19, 16, 19, 16)));
		
		/// <summary>
		/// Accept button on assignments
		/// </summary>
		public static UIImage Accept {
			get { return accept.Value; }
		}

		static Lazy<UIImage> addphoto = new Lazy<UIImage> (() => UIImage.FromFile ("Images/addphoto.png").CreateResizableImage (new UIEdgeInsets (18, 19, 21, 19)));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage AddPhoto {
			get { return addphoto.Value; }
		}

		static Lazy<UIImage> arrow_down = new Lazy<UIImage> (() => UIImage.FromFile ("Images/arrow_down.png"));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage Arrow_Down {
			get { return arrow_down.Value; }
		}

		static Lazy<UIImage> arrow_up = new Lazy<UIImage> (() => UIImage.FromFile ("Images/arrow_up.png"));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage Arrow_Up {
			get { return arrow_up.Value; }
		}

		static Lazy<UIImage> arrow = new Lazy<UIImage> (() => UIImage.FromFile ("Images/arrow.png"));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage Arrow {
			get { return arrow.Value; }
		}

		static Lazy<UIImage> arrowwhite = new Lazy<UIImage> (() => UIImage.FromFile ("Images/arrowwhite.png"));
		
		/// <summary>
		/// Image for the active assignment
		/// </summary>
		public static UIImage ArrowWhite {
			get { return arrowwhite.Value; }
		}

		static Lazy<UIImage> assignmentActive = new Lazy<UIImage> (() => UIImage.FromFile ("Images/assignmentactive.png").CreateResizableImage (new UIEdgeInsets ()));
		
		/// <summary>
		/// Active assignment background
		/// </summary>
		public static UIImage AssignmentActive {
			get { return assignmentActive.Value; }
		}

		static Lazy<UIImage> backbutton = new Lazy<UIImage> (() => UIImage.FromFile ("Images/backbutton.png").CreateResizableImage (new UIEdgeInsets (15, 15, 15, 5)));
		
		/// <summary>
		/// Standard back button for UINavigationBar
		/// </summary>
		public static UIImage BackButton {
			get { return backbutton.Value; }
		}

		static Lazy<UIImage> bluebar = new Lazy<UIImage> (() => UIImage.FromFile ("Images/bluebar.png"));
		
		/// <summary>
		/// Image for default UIToolbar
		/// </summary>
		public static UIImage BlueBar {
			get { return bluebar.Value; }
		}

		static Lazy<UIImage> bluenavbutton = new Lazy<UIImage> (() => UIImage.FromFile ("Images/bluenavbutton.png").CreateResizableImage (new UIEdgeInsets (6, 6, 6, 6)));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage BlueNavButton {
			get { return bluenavbutton.Value; }
		}

		static Lazy<UIImage> buttondark = new Lazy<UIImage> (() => UIImage.FromFile ("Images/buttondark.png").CreateResizableImage (new UIEdgeInsets (16, 17, 17, 17)));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage ButtonDark {
			get { return buttondark.Value; }
		}

		static Lazy<UIImage> camera = new Lazy<UIImage> (() => UIImage.FromFile ("Images/camera.png"));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage Camera {
			get { return camera.Value; }
		}

		static Lazy<UIImage> check_empty = new Lazy<UIImage> (() => UIImage.FromFile ("Images/check_empty.png"));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage Check_Empty {
			get { return check_empty.Value; }
		}

		static Lazy<UIImage> check_filled = new Lazy<UIImage> (() => UIImage.FromFile ("Images/check_filled.png"));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage Check_Filled {
			get { return check_filled.Value; }
		}

		static Lazy<UIImage> complete = new Lazy<UIImage> (() => UIImage.FromFile ("Images/complete.png").CreateResizableImage (new UIEdgeInsets (16, 16, 16, 16)));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage Complete {
			get { return complete.Value; }
		}

		static Lazy<UIImage> decline = new Lazy<UIImage> (() => UIImage.FromFile ("Images/decline.png").CreateResizableImage (new UIEdgeInsets (19, 16, 19, 16)));
		
		/// <summary>
		/// Decline button on assignments
		/// </summary>
		public static UIImage Decline {
			get { return decline.Value; }
		}

		static Lazy<UIImage> dropdown = new Lazy<UIImage> (() => UIImage.FromFile ("Images/dropdown.png").CreateResizableImage (new UIEdgeInsets (6, 39, 6, 6)));
		
		/// <summary>
		/// Drop down button image
		/// </summary>
		public static UIImage DropDown {
			get { return dropdown.Value; }
		}

		static Lazy<UIImage> iconPdf = new Lazy<UIImage> (() => UIImage.FromFile ("Images/icon_pdf.png"));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage IconPdf {
			get { return iconPdf.Value; }
		}

		static Lazy<UIImage> iconactive = new Lazy<UIImage> (() => UIImage.FromFile ("Images/iconactive.png"));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage IconActive {
			get { return iconactive.Value; }
		}

		static Lazy<UIImage> iconcomplete = new Lazy<UIImage> (() => UIImage.FromFile ("Images/iconcomplete.png"));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage IconComplete {
			get { return iconcomplete.Value; }
		}

		static Lazy<UIImage> iconhold = new Lazy<UIImage> (() => UIImage.FromFile ("Images/iconhold.png"));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage IconHold {
			get { return iconhold.Value; }
		}

		static Lazy<UIImage> iconPhone = new Lazy<UIImage> (() => UIImage.FromFile ("Images/iconphone.png"));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage IconPhone {
			get { return iconPhone.Value; }
		}

		static Lazy<UIImage> iconPhoneDark = new Lazy<UIImage> (() => UIImage.FromFile ("Images/iconphone_dark.png"));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage IconPhoneDark {
			get { return iconPhoneDark.Value; }
		}

		static Lazy<UIImage> iconsettings = new Lazy<UIImage> (() => UIImage.FromFile ("Images/iconsettings.png"));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage IconSettings {
			get { return iconsettings.Value; }
		}

		static Lazy<UIImage> inlay = new Lazy<UIImage> (() => UIImage.FromFile ("Images/inlay.png").CreateResizableImage (new UIEdgeInsets (10, 10, 10, 10)));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage Inlay {
			get { return inlay.Value; }
		}

		static Lazy<UIImage> leftListEnd = new Lazy<UIImage> (() => UIImage.FromFile ("Images/leftlist_end.png").CreateResizableImage (new UIEdgeInsets (12, 12, 12, 12)));
		
		/// <summary>
		/// Active image for the last item in MenuController's tableView
		/// </summary>
		public static UIImage LeftListEnd {
			get { return leftListEnd.Value; }
		}

		static Lazy<UIImage> leftListEndActive = new Lazy<UIImage> (() => UIImage.FromFile ("Images/leftlist_end_active.png").CreateResizableImage (new UIEdgeInsets (12, 12, 12, 12)));
		
		/// <summary>
		/// Image for the last item in MenuController's tableView
		/// </summary>
		public static UIImage LeftListEndActive {
			get { return leftListEndActive.Value; }
		}

		static Lazy<UIImage> leftListMid = new Lazy<UIImage> (() => UIImage.FromFile ("Images/leftlist_mid.png").CreateResizableImage (new UIEdgeInsets (12, 12, 12, 12)));
		
		/// <summary>
		/// Image for a middle item in MenuController's tableView
		/// </summary>
		public static UIImage LeftListMid {
			get { return leftListMid.Value; }
		}

		static Lazy<UIImage> leftListMidActive = new Lazy<UIImage> (() => UIImage.FromFile ("Images/leftlist_mid_active.png").CreateResizableImage (new UIEdgeInsets (12, 12, 12, 12)));
		
		/// <summary>
		/// Active image for a middle item in MenuController's tableView
		/// </summary>
		public static UIImage LeftListMidActive {
			get { return leftListMidActive.Value; }
		}

		static Lazy<UIImage> leftListTop = new Lazy<UIImage> (() => UIImage.FromFile ("Images/leftlist_top.png").CreateResizableImage (new UIEdgeInsets (12, 12, 12, 12)));
		
		/// <summary>
		/// Image for the first item in MenuController's tableView
		/// </summary>
		public static UIImage LeftListTop {
			get { return leftListTop.Value; }
		}

		static Lazy<UIImage> leftListTopActive = new Lazy<UIImage> (() => UIImage.FromFile ("Images/leftlist_top_active.png").CreateResizableImage (new UIEdgeInsets (12, 12, 12, 12)));
		
		/// <summary>
		/// Active image for the first item in MenuController's tableView
		/// </summary>
		public static UIImage LeftListTopActive {
			get { return leftListTopActive.Value; }
		}

		static Lazy<UIImage> loginBox = new Lazy<UIImage> (() => UIImage.FromFile ("Images/login_box.png").CreateResizableImage (new UIEdgeInsets (21, 21, 21, 21)));

		/// <summary>
		/// Login box on first screen
		/// </summary>
		public static UIImage LoginBox {
			get { return loginBox.Value; }
		}

		static Lazy<UIImage> loginButton = new Lazy<UIImage> (() => UIImage.FromFile ("Images/login_btn.png").CreateResizableImage (new UIEdgeInsets (11, 8, 11, 8)));

		/// <summary>
		/// Login button on first screen
		/// </summary>
		public static UIImage LoginButton {
			get { return loginButton.Value; }
		}

		static Lazy<UIImage> loginTextField = new Lazy<UIImage> (() => UIImage.FromFile ("Images/login_textfield.png").CreateResizableImage (new UIEdgeInsets (10, 10, 10, 10)));

		/// <summary>
		/// Login text field on first screen
		/// </summary>
		public static UIImage LoginTextField {
			get { return loginTextField.Value; }
		}

		static Lazy<UIImage> loginInset = new Lazy<UIImage> (() => UIImage.FromFile ("Images/login_inset.png").CreateResizableImage (new UIEdgeInsets (12, 12, 12, 12)));

		/// <summary>
		/// Login box on first screen, used in the logo's inset
		/// </summary>
		public static UIImage LoginInset {
			get { return loginInset.Value; }
		}

		static Lazy<UIImage> logo = new Lazy<UIImage> (() => UIImage.FromFile ("Images/logo.png"));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage Logo {
			get { return logo.Value; }
		}

		static Lazy<UIImage> map = new Lazy<UIImage> (() => UIImage.FromFile ("Images/map.png"));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage Map {
			get { return map.Value; }
		}

		static Lazy<UIImage> modal = new Lazy<UIImage> (() => UIImage.FromFile ("Images/modal.png").CreateResizableImage (new UIEdgeInsets (65, 43, 38, 43)));
		
		/// <summary>
		///
		/// </summary>
		public static UIImage Modal {
			get { return modal.Value; }
		}

		static Lazy<UIImage> modalInlay = new Lazy<UIImage> (() => UIImage.FromFile ("Images/modal_inlay.png").CreateResizableImage (new UIEdgeInsets (12, 12, 12, 12)));
		
		/// <summary>
		///
		/// </summary>
		public static UIImage ModalInlay {
			get { return modalInlay.Value; }
		}

		static Lazy<UIImage> numberbox = new Lazy<UIImage> (() => UIImage.FromFile ("Images/numberbox.png").CreateResizableImage (new UIEdgeInsets (11, 11, 11, 11)));
		
		/// <summary>
		/// Background image for numbers on assignments
		/// </summary>
		public static UIImage NumberBox {
			get { return numberbox.Value; }
		}

		static Lazy<UIImage> orangebar = new Lazy<UIImage> (() => UIImage.FromFile ("Images/orangebar.png"));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage OrangeBar {
			get { return orangebar.Value; }
		}

		static Lazy<UIImage> photoframe = new Lazy<UIImage> (() => UIImage.FromFile ("Images/photoframe.png").CreateResizableImage (new UIEdgeInsets (10, 12, 15, 12)));
		
		/// <summary>
		///
		/// </summary>
		public static UIImage PhotoFrame {
			get { return photoframe.Value; }
		}

		static Lazy<UIImage> record = new Lazy<UIImage> (() => UIImage.FromFile ("Images/record.png"));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage Record {
			get { return record.Value; }
		}

		static Lazy<UIImage> record_active = new Lazy<UIImage> (() => UIImage.FromFile ("Images/record_active.png"));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage Record_Active {
			get { return record_active.Value; }
		}

		static Lazy<UIImage> rightboxshadow = new Lazy<UIImage> (() => UIImage.FromFile ("Images/rightboxshadow.png"));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage RightBoxShadow {
			get { return rightboxshadow.Value; }
		}

		static Lazy<UIImage> row120 = new Lazy<UIImage> (() => UIImage.FromFile ("Images/row120.png").CreateResizableImage (new UIEdgeInsets (25, 25, 25, 25)));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage Row120 {
			get { return row120.Value; }
		}

		static Lazy<UIImage> row120end = new Lazy<UIImage> (() => UIImage.FromFile ("Images/row120end.png").CreateResizableImage (new UIEdgeInsets (37, 37, 37, 37)));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage Row120End {
			get { return row120end.Value; }
		}

		static Lazy<UIImage> row180 = new Lazy<UIImage> (() => UIImage.FromFile ("Images/row180.png").CreateResizableImage (new UIEdgeInsets (22, 22, 22, 22)));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage Row180 {
			get { return row180.Value; }
		}

		static Lazy<UIImage> row180end = new Lazy<UIImage> (() => UIImage.FromFile ("Images/row180end.png").CreateResizableImage (new UIEdgeInsets (45, 45, 45, 45)));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage Row180End {
			get { return row180end.Value; }
		}

		static Lazy<UIImage> smallGreyButton = new Lazy<UIImage> (() => UIImage.FromFile ("Images/smallgreybtn.png").CreateResizableImage (new UIEdgeInsets (8, 8, 8, 8)));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage SmallGreyButton {
			get { return smallGreyButton.Value; }
		}

		static Lazy<UIImage> timerbg = new Lazy<UIImage> (() => UIImage.FromFile ("Images/timerbg.png"));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage TimerBg {
			get { return timerbg.Value; }
		}

		static Lazy<UIImage> timerfield = new Lazy<UIImage> (() => UIImage.FromFile ("Images/timerfield.png").CreateResizableImage (new UIEdgeInsets (6, 6, 6, 6)));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage TimerField {
			get { return timerfield.Value; }
		}

		static Lazy<UIImage> topnav = new Lazy<UIImage> (() => UIImage.FromFile ("Images/topnav.png").CreateResizableImage (new UIEdgeInsets (0, 0, 0, 0)));
		
		/// <summary>
		/// Image for the top navigation bar
		/// </summary>
		public static UIImage TopNav {
			get { return topnav.Value; }
		}

		static Lazy<UIImage> barButtonItem = new Lazy<UIImage> (() => UIImage.FromFile ("Images/topnavbtn.png").CreateResizableImage (new UIEdgeInsets (6, 6, 6, 6)));
		
		/// <summary>
		/// Background image for UIBarButtonItem
		/// </summary>
		public static UIImage BarButtonItem {
			get { return barButtonItem.Value; }
		}

		static Lazy<UIImage> assignmentBlue = new Lazy<UIImage> (() => UIImage.FromFile ("Images/assignmentblue.png").CreateResizableImage (new UIEdgeInsets ()));
		
		/// <summary>
		/// Cell background for selected assignment
		/// </summary>
		public static UIImage AssignmentBlue {
			get { return assignmentBlue.Value; }
		}
		
		static Lazy<UIImage> assignmentGrey = new Lazy<UIImage> (() => UIImage.FromFile ("Images/assignmentgrey.png").CreateResizableImage (new UIEdgeInsets ()));
		
		/// <summary>
		/// Cell background for assignment
		/// </summary>
		public static UIImage AssignmentGrey {
			get { return assignmentGrey.Value; }
		}

		#endregion

		#region Colors

		static Lazy<UIColor> labelColor = new Lazy<UIColor> (() => UIColor.FromRGB (0x33, 0x33, 0x33));

		/// <summary>
		/// General label color for the entire app
		/// </summary>
		public static UIColor LabelColor {
			get { return labelColor.Value; }
		}

		static Lazy<UIColor> indicatorColor = new Lazy<UIColor> (() => UIColor.FromRGB (0x77, 0x77, 0x77));

		/// <summary>
		/// General indicator color for the entire app
		/// </summary>
		public static UIColor IndicatorColor {
			get { return indicatorColor.Value; }
		}

		static Lazy<UIColor> segmentedTintColor = new Lazy<UIColor> (() => UIColor.FromRGB (0x18, 0xa0, 0xd2));
		
		/// <summary>
		/// Tint color for UISegmentedControl
		/// </summary>
		public static UIColor SegmentedTintColor {
			get { return segmentedTintColor.Value; }
		}

		static Lazy<UIColor> linenPattern = new Lazy<UIColor> (() => UIColor.FromPatternImage (UIImage.FromFile ("Images/linenpattern.png")));
		
		/// <summary>
		/// Linen pattern background for use throughout the app
		/// </summary>
		public static UIColor LinenPattern {
			get { return linenPattern.Value; }
		}

		static Lazy<UIColor> linenLeft = new Lazy<UIColor> (() => UIColor.FromPatternImage (UIImage.FromFile ("Images/linen_left.png")));
		
		/// <summary>
		/// Linen pattern background for the left menu
		/// </summary>
		public static UIColor LinenLeft {
			get { return linenLeft.Value; }
		}

		#endregion

		/// <summary>
		/// Apply UIAppearance to this application, this is iOS's version of "styling"
		/// </summary>
		public static void Apply ()
		{
			UILabel.Appearance.TextColor = LabelColor;

			UIActivityIndicatorView.Appearance.Color = IndicatorColor;

			UINavigationBar.Appearance.SetBackgroundImage (TopNav, UIBarMetrics.Default);

			UIToolbar.Appearance.SetBackgroundImage (BlueBar, UIToolbarPosition.Any, UIBarMetrics.Default);

			UISegmentedControl.Appearance.TintColor = SegmentedTintColor;

			UIBarButtonItem.Appearance.SetBackgroundImage (BarButtonItem, UIControlState.Normal, UIBarMetrics.Default);

			UIBarButtonItem.Appearance.SetBackButtonBackgroundImage (BackButton, UIControlState.Normal, UIBarMetrics.Default);

			UIButton.Appearance.SetTitleColor (Theme.LabelColor, UIControlState.Normal);
		}

		const string FontName = "HelveticaNeue-Medium";
		const string BoldFontName = "HelveticaNeue-Bold";

		/// <summary>
		/// Returns the default font with a certain size
		/// </summary>
		public static UIFont FontOfSize (float size)
		{
			return UIFont.FromName (FontName, size);
		}

		/// <summary>
		/// Returns the default font with a certain size
		/// </summary>
		public static UIFont BoldFontOfSize (float size)
		{
			return UIFont.FromName (BoldFontName, size);
		}

		/// <summary>
		/// Transitions the window, for a fullscreen transition
		/// </summary>
		public static void TransitionWindow (UIViewAnimationOptions options = UIViewAnimationOptions.TransitionCrossDissolve)
		{
			UIView.Transition (ServiceContainer.Resolve <UIWindow> (), .3, options, delegate {
			}, delegate {
			});
		}
	}
}

