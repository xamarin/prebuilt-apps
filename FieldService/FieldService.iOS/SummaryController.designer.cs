// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace FieldService.iOS
{
	[Register ("SummaryController")]
	partial class SummaryController
	{
		[Outlet]
		MonoTouch.UIKit.UILabel itemsLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel hoursLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel expensesLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView itemsBackground { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView hoursBackground { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView expensesBackground { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel items { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel hours { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel expenses { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel description { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel descriptionTitle { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView descriptionBackground { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIToolbar toolbar { get; set; }
		
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
