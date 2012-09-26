using System;
using MonoTouch.UIKit;
using EmployeeDirectory.Data;
using EmployeeDirectory.ViewModels;
using System.ComponentModel;
using System.Drawing;

namespace EmployeeDirectory.iOS
{
	public class SearchEditorView : UIView
	{
		UITextField search;
		UISegmentedControl segments;

		public event EventHandler Search;

		public string Text {
			get { return search.Text; }
			set { search.Text = value; }
		}

		public SearchProperty Property {
			get { return (SearchProperty)segments.SelectedSegment; }
			set { segments.SelectedSegment = (int)value; }
		}

		public SearchEditorView (string text, SearchProperty property)
		{
			Frame = new System.Drawing.RectangleF (0, 0, 320, 88);

			BackgroundColor = UIColor.FromRGB (177, 191, 213);

			//
			// The search box
			//
			search = new UITextField (new RectangleF (6, 9, 320 - 38, 32)) {
				BorderStyle = UITextBorderStyle.RoundedRect,
				Placeholder = "Search",
				ReturnKeyType = UIReturnKeyType.Search,
				ShouldReturn = tf => {
					OnSearch ();
					return false;
				},
				AutocorrectionType = UITextAutocorrectionType.No,
				AutocapitalizationType = UITextAutocapitalizationType.None,
			};
			Text = text;
			AddSubview (search);

			//
			// Use a segment control to control what we are searching
			//
			segments = new UISegmentedControl (new RectangleF (6, 47, 320 - 38, 32)) {
				ControlStyle = UISegmentedControlStyle.Bar,
			};
			segments.InsertSegment ("Name", (int)SearchProperty.Name, false);
			segments.InsertSegment ("Dept.", (int)SearchProperty.Department, false);
			segments.InsertSegment ("Title", (int)SearchProperty.Title, false);
			segments.InsertSegment ("All", (int)SearchProperty.All, false);
			segments.SelectedSegment = (int)property;
			segments.AllEvents += delegate {
				Property = (SearchProperty)segments.SelectedSegment;
				OnSearch ();
			};
			AddSubview (segments);
		}

		void OnSearch ()
		{
			search.ResignFirstResponder ();

			var ev = Search;
			if (ev != null) {
				ev (this, EventArgs.Empty);
			}
		}
	}
}

