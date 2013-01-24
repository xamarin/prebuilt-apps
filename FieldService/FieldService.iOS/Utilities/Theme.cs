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

		static Lazy<UIImage> accept = new Lazy<UIImage> (() => UIImage.FromFile ("Images/accept.png").CreateResizableImage (new UIEdgeInsets (9, 9, 9, 9)));
		
		/// <summary>
		/// Accept button on assignments
		/// </summary>
		public static UIImage Accept {
			get { return accept.Value; }
		}

		static Lazy<UIImage> addphoto = new Lazy<UIImage> (() => UIImage.FromFile ("Images/addphoto.png").CreateResizableImage (new UIEdgeInsets (18, 19, 21, 19)));
		
		/// <summary>
		/// Image for add photo button on expenses
		/// </summary>
		public static UIImage AddPhoto {
			get { return addphoto.Value; }
		}

		static Lazy<UIImage> arrowDown = new Lazy<UIImage> (() => UIImage.FromFile ("Images/arrow_down.png"));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage ArrowDown {
			get { return arrowDown.Value; }
		}

		static Lazy<UIImage> arrowUp = new Lazy<UIImage> (() => UIImage.FromFile ("Images/arrow_up.png"));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage ArrowUp {
			get { return arrowUp.Value; }
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

		static Lazy<UIImage> assignmentActiveBlue = new Lazy<UIImage> (() => UIImage.FromFile ("Images/assignmentactiveblue.png").CreateResizableImage (new UIEdgeInsets ()));
		
		/// <summary>
		/// Active assignment background when pressed
		/// </summary>
		public static UIImage AssignmentActiveBlue {
			get { return assignmentActiveBlue.Value; }
		}

		static Lazy<UIImage> dot = new Lazy<UIImage> (() => UIImage.FromFile ("Images/dot.png"));
		
		/// <summary>
		/// Dot image for selected state of menu
		/// </summary>
		public static UIImage Dot {
			get { return dot.Value; }
		}

		static Lazy<UIImage> transparentDot = new Lazy<UIImage> (() => {
			//This is some quick code to create a transparent image the same size as Theme.Dot
			UIGraphics.BeginImageContext (Dot.Size);
			try
			{
				using (var context = UIGraphics.GetCurrentContext ())
				{
					return UIGraphics.GetImageFromCurrentImageContext ();
				}
			}
			finally
			{
				UIGraphics.EndImageContext ();
			}
		});
		
		/// <summary>
		/// Dot image for unselected state of menu
		/// </summary>
		public static UIImage TransparentDot {
			get { return transparentDot.Value; }
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

		static Lazy<UIImage> blueBarButtonItem = new Lazy<UIImage> (() => UIImage.FromFile ("Images/bluenavbutton.png").CreateResizableImage (new UIEdgeInsets (6, 6, 6, 6)));
		
		/// <summary>
		/// Button image for blue toolbar buttons
		/// </summary>
		public static UIImage BlueBarButtonItem {
			get { return blueBarButtonItem.Value; }
		}

		static Lazy<UIImage> orangeBarButtonItem = new Lazy<UIImage> (() => UIImage.FromFile ("Images/orangenavbutton.png").CreateResizableImage (new UIEdgeInsets (6, 6, 6, 6)));
		
		/// <summary>
		/// Button image for orange toolbar buttons
		/// </summary>
		public static UIImage OrangeBarButtonItem {
			get { return orangeBarButtonItem.Value; }
		}

		static Lazy<UIImage> buttondark = new Lazy<UIImage> (() => UIImage.FromFile ("Images/buttondark.png").CreateResizableImage (new UIEdgeInsets (16, 17, 17, 17)));
		
		/// <summary>
		/// Button image used for buttons like "Add Photo" or "Add Signature"
		/// </summary>
		public static UIImage ButtonDark {
			get { return buttondark.Value; }
		}

		static Lazy<UIImage> camera = new Lazy<UIImage> (() => UIImage.FromFile ("Images/camera.png"));
		
		/// <summary>
		/// Camera icon for expenses, etc.
		/// </summary>
		public static UIImage Camera {
			get { return camera.Value; }
		}

		static Lazy<UIImage> complete = new Lazy<UIImage> (() => UIImage.FromFile ("Images/complete.png").CreateResizableImage (new UIEdgeInsets (16, 16, 16, 16)));
		
		/// <summary>
		/// Image for the complete button
		/// </summary>
		public static UIImage Complete {
			get { return complete.Value; }
		}

		static Lazy<UIImage> completeInactive = new Lazy<UIImage> (() => UIImage.FromFile ("Images/complete_inactive.png").CreateResizableImage (new UIEdgeInsets (16, 16, 16, 16)));
		
		/// <summary>
		/// Image for the complete button when disabled
		/// </summary>
		public static UIImage CompleteInactive {
			get { return completeInactive.Value; }
		}

		static Lazy<UIImage> deleteButton = new Lazy<UIImage> (() => UIImage.FromFile ("Images/deletebutton.png").CreateResizableImage (new UIEdgeInsets (9, 9, 9, 9)));
		
		/// <summary>
		/// Image for the delete button
		/// </summary>
		public static UIImage DeleteButton {
			get { return deleteButton.Value; }
		}

		static Lazy<UIImage> decline = new Lazy<UIImage> (() => UIImage.FromFile ("Images/decline.png").CreateResizableImage (new UIEdgeInsets (19, 16, 19, 16)));
		
		/// <summary>
		/// Decline button on assignments
		/// </summary>
		public static UIImage Decline {
			get { return decline.Value; }
		}

		static Lazy<UIImage> dropdown = new Lazy<UIImage> (() => UIImage.FromFile ("Images/dropdown.png").CreateResizableImage (new UIEdgeInsets (9, 9, 9, 9)));
		
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
		/// Dark phone icon used in assignment history
		/// </summary>
		public static UIImage IconPhoneDark {
			get { return iconPhoneDark.Value; }
		}

		static Lazy<UIImage> iconsettings = new Lazy<UIImage> (() => UIImage.FromFile ("Images/iconsettings.png"));
		
		/// <summary>
		/// Settings icon image
		/// </summary>
		public static UIImage IconSettings {
			get { return iconsettings.Value; }
		}

		static Lazy<UIImage> inlay = new Lazy<UIImage> (() => UIImage.FromFile ("Images/inlay.png").CreateResizableImage (new UIEdgeInsets (10, 10, 10, 10)));
		
		/// <summary>
		/// An image for the bevel on the assignment summary page
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

		static Lazy<UIImage> logo = new Lazy<UIImage> (() => UIImage.FromFile ("Images/logo.png"));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage Logo {
			get { return logo.Value; }
		}

		static Lazy<UIImage> map = new Lazy<UIImage> (() => UIImage.FromFile ("Images/map.png"));
		
		/// <summary>
		/// Map image
		/// </summary>
		public static UIImage Map {
			get { return map.Value; }
		}

		static Lazy<UIImage> mapIcon = new Lazy<UIImage> (() => UIImage.FromFile ("Images/mapicon.png"));
		
		/// <summary>
		/// Map icon for tab bar
		/// </summary>
		public static UIImage MapIcon {
			get { return mapIcon.Value; }
		}

		static Lazy<UIImage> listIcon = new Lazy<UIImage> (() => UIImage.FromFile ("Images/listicon.png"));
		
		/// <summary>
		/// Assignment list icon for tab bar
		/// </summary>
		public static UIImage ListIcon {
			get { return listIcon.Value; }
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
		/// Background image for sections on modal controllers
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
		/// Background image for history-theme toolbars
		/// </summary>
		public static UIImage OrangeBar {
			get { return orangebar.Value; }
		}

		static Lazy<UIImage> photoframe = new Lazy<UIImage> (() => UIImage.FromFile ("Images/photoframe.png").CreateResizableImage (new UIEdgeInsets (35, 35, 35, 35)));
		
		/// <summary>
		/// Photo border image that looks like a picture frame
		/// </summary>
		public static UIImage PhotoFrame {
			get { return photoframe.Value; }
		}

		static Lazy<UIImage> record = new Lazy<UIImage> (() => UIImage.FromFile ("Images/record.png"));
		
		/// <summary>
		/// Image for the record button
		/// </summary>
		public static UIImage Record {
			get { return record.Value; }
		}

		static Lazy<UIImage> recordActive = new Lazy<UIImage> (() => UIImage.FromFile ("Images/record_active.png"));
		
		/// <summary>
		/// Image for the record button when toggled on
		/// </summary>
		public static UIImage RecordActive {
			get { return recordActive.Value; }
		}

		static Lazy<UIImage> rightBoxShadow = new Lazy<UIImage> (() => UIImage.FromFile ("Images/rightboxshadow.png"));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage RightBoxShadow {
			get { return rightBoxShadow.Value; }
		}

		static Lazy<UIImage> toolbarShadow = new Lazy<UIImage> (() => UIImage.FromFile ("Images/toolbarshadow.png"));
		
		/// <summary>
		/// Shadow for placing under toolbars
		/// </summary>
		public static UIImage ToolbarShadow {
			get { return toolbarShadow.Value; }
		}

		static Lazy<UIImage> shortRowPress = new Lazy<UIImage> (() => UIImage.FromFile ("Images/shortrowpress.png").CreateResizableImage (new UIEdgeInsets (25, 25, 25, 25)));
		
		/// <summary>
		/// Image for row in history
		/// </summary>
		public static UIImage ShortRowPress {
			get { return shortRowPress.Value; }
		}

		static Lazy<UIImage> rowPress = new Lazy<UIImage> (() => UIImage.FromFile ("Images/rowpress.png").CreateResizableImage (new UIEdgeInsets (25, 25, 25, 25)));
		
		/// <summary>
		/// Image for row in labor hours
		/// </summary>
		public static UIImage RowPress {
			get { return rowPress.Value; }
		}

		static Lazy<UIImage> row = new Lazy<UIImage> (() => UIImage.FromFile ("Images/row.png").CreateResizableImage (new UIEdgeInsets (22, 22, 22, 22)));
		
		/// <summary>
		/// Background image for a table row
		/// </summary>
		public static UIImage Row {
			get { return row.Value; }
		}

		static Lazy<UIImage> rowEnd = new Lazy<UIImage> (() => UIImage.FromFile ("Images/rowend.png").CreateResizableImage (new UIEdgeInsets (45, 45, 45, 45)));
		
		/// <summary>
		/// Background image for the end of a table row
		/// </summary>
		public static UIImage RowEnd {
			get { return rowEnd.Value; }
		}

		static Lazy<UIImage> rowShadow = new Lazy<UIImage> (() => UIImage.FromFile ("Images/rowshadow.png").CreateResizableImage (new UIEdgeInsets (0, 90, 0, 90)));
		
		/// <summary>
		/// Bottom shadow for a row
		/// </summary>
		public static UIImage RowShadow {
			get { return rowShadow.Value; }
		}

		static Lazy<UIImage> smallGreyButton = new Lazy<UIImage> (() => UIImage.FromFile ("Images/smallgreybtn.png").CreateResizableImage (new UIEdgeInsets (8, 8, 8, 8)));
		
		/// <summary>
		/// 
		/// </summary>
		public static UIImage SmallGreyButton {
			get { return smallGreyButton.Value; }
		}

		static Lazy<UIImage> timerBackground = new Lazy<UIImage> (() => UIImage.FromFile ("Images/timerbg.png"));
		
		/// <summary>
		/// Background image for the large timer
		/// </summary>
		public static UIImage TimerBackground {
			get { return timerBackground.Value; }
		}

		static Lazy<UIImage> timerfield = new Lazy<UIImage> (() => UIImage.FromFile ("Images/timerfield.png").CreateResizableImage (new UIEdgeInsets (6, 6, 6, 6)));
		
		/// <summary>
		/// Background for the timer field on the active assignment
		/// </summary>
		public static UIImage TimerField {
			get { return timerfield.Value; }
		}

		static Lazy<UIImage> topnav = new Lazy<UIImage> (() => UIImage.FromFile ("Images/topnav.png").CreateResizableImage (new UIEdgeInsets (9, 9, 9, 9)));
		
		/// <summary>
		/// Image for the top navigation bar
		/// </summary>
		public static UIImage TopNav {
			get { return topnav.Value; }
		}

		static Lazy<UIImage> darkBarButtonItem = new Lazy<UIImage> (() => UIImage.FromFile ("Images/topnavbtn.png").CreateResizableImage (new UIEdgeInsets (6, 6, 6, 6)));
		
		/// <summary>
		/// Background image for UIBarButtonItem
		/// </summary>
		public static UIImage DarkBarButtonItem {
			get { return darkBarButtonItem.Value; }
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

		static Lazy<UIImage> questionMark = new Lazy<UIImage> (() => UIImage.FromFile ("Images/questionmark.png").CreateResizableImage (new UIEdgeInsets ()));
		
		/// <summary>
		/// Question mark image on login screen
		/// </summary>
		public static UIImage QuestionMark {
			get { return questionMark.Value; }
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

		static Lazy<UIColor> blueTextColor = new Lazy<UIColor> (() => UIColor.FromRGB (0.22f, 0.33f, 0.53f));
		
		/// <summary>
		/// Default blue text color for editable text - same as seen in Settings app
		/// </summary>
		public static UIColor BlueTextColor {
			get { return blueTextColor.Value; }
		}

		static Lazy<UIColor> indicatorColor = new Lazy<UIColor> (() => UIColor.FromRGB (0x77, 0x77, 0x77));

		/// <summary>
		/// General indicator color for the entire app
		/// </summary>
		public static UIColor IndicatorColor {
			get { return indicatorColor.Value; }
		}

		static Lazy<UIColor> linenPattern = new Lazy<UIColor> (() => UIColor.FromPatternImage (UIImage.FromFile ("Images/linenpattern.png")));
		
		/// <summary>
		/// Linen pattern background for use throughout the app
		/// </summary>
		public static UIColor LinenPattern {
			get { return linenPattern.Value; }
		}

		static Lazy<UIColor> checkboxTextColor = new Lazy<UIColor> (() => UIColor.FromRGB (0x7f, 0x7f, 0x7f));
		
		/// <summary>
		/// Color for the checkbox on items
		/// </summary>
		public static UIColor CheckboxTextColor {
			get { return checkboxTextColor.Value; }
		}

		static Lazy<UIColor> leftMenuColor = new Lazy<UIColor> (() => UIColor.FromPatternImage (UIImage.FromFile ("Images/bg_left.png")));
		
		/// <summary>
		/// Color for the left menu on the assignment details screen
		/// </summary>
		public static UIColor LeftMenuColor {
			get { return leftMenuColor.Value; }
		}

		static Lazy<UIColor> backgroundColor = new Lazy<UIColor> (() => UIColor.FromRGB (0x55, 0x55, 0x55));
		
		/// <summary>
		/// General background color for the app
		/// </summary>
		public static UIColor BackgroundColor {
			get { return backgroundColor.Value; }
		}

		#endregion

		/// <summary>
		/// Apply UIAppearance to this application, this is iOS's version of "styling"
		/// </summary>
		public static void Apply ()
		{
			UIActivityIndicatorView.Appearance.Color = IndicatorColor;

			UIToolbar.Appearance.SetBackgroundImage (BlueBar, UIToolbarPosition.Any, UIBarMetrics.Default);

			UIBarButtonItem.Appearance.SetBackButtonBackgroundImage (BackButton, UIControlState.Normal, UIBarMetrics.Default);
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
		/// Transitions a controller to the rootViewController, for a fullscreen transition
		/// </summary>
		public static void TransitionController (UIViewController controller, bool animated = true)
		{
			var window = ServiceContainer.Resolve<UIWindow>();

			//Return if it's already the root controller
			if (window.RootViewController == controller)
				return;

			//Set the root controller
			window.RootViewController = controller;

			//Peform an animation, note that null is not allowed as a callback, so I use delegate { }
			if (animated)
				UIView.Transition (window, .3, UIViewAnimationOptions.TransitionCrossDissolve, delegate { }, delegate { });
		}
	}
}

