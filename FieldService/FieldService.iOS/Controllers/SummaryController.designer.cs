// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;

namespace FieldService.iOS
{
	[Register ("SummaryController")]
	partial class SummaryController
	{
		[Outlet]
		UIKit.UILabel itemsLabel { get; set; }

		[Outlet]
		UIKit.UILabel hoursLabel { get; set; }

		[Outlet]
		UIKit.UILabel expensesLabel { get; set; }

		[Outlet]
		UIKit.UIImageView itemsBackground { get; set; }

		[Outlet]
		UIKit.UIImageView hoursBackground { get; set; }

		[Outlet]
		UIKit.UIImageView expensesBackground { get; set; }

		[Outlet]
		UIKit.UILabel items { get; set; }

		[Outlet]
		UIKit.UILabel hours { get; set; }

		[Outlet]
		UIKit.UILabel expenses { get; set; }

		[Outlet]
		UIKit.UILabel description { get; set; }

		[Outlet]
		UIKit.UILabel descriptionTitle { get; set; }

		[Outlet]
		UIKit.UIImageView descriptionBackground { get; set; }

		[Outlet]
		UIKit.UIToolbar toolbar { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (itemsLabel != null) {
				itemsLabel.Dispose ();
				itemsLabel = null;
			}

			if (hoursLabel != null) {
				hoursLabel.Dispose ();
				hoursLabel = null;
			}

			if (expensesLabel != null) {
				expensesLabel.Dispose ();
				expensesLabel = null;
			}

			if (itemsBackground != null) {
				itemsBackground.Dispose ();
				itemsBackground = null;
			}

			if (hoursBackground != null) {
				hoursBackground.Dispose ();
				hoursBackground = null;
			}

			if (expensesBackground != null) {
				expensesBackground.Dispose ();
				expensesBackground = null;
			}

			if (items != null) {
				items.Dispose ();
				items = null;
			}

			if (hours != null) {
				hours.Dispose ();
				hours = null;
			}

			if (expenses != null) {
				expenses.Dispose ();
				expenses = null;
			}

			if (description != null) {
				description.Dispose ();
				description = null;
			}

			if (descriptionTitle != null) {
				descriptionTitle.Dispose ();
				descriptionTitle = null;
			}

			if (descriptionBackground != null) {
				descriptionBackground.Dispose ();
				descriptionBackground = null;
			}

			if (toolbar != null) {
				toolbar.Dispose ();
				toolbar = null;
			}
		}
	}
}
