using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;
using FieldService.Data;
using FieldService.Utilities;
using MonoTouch.ObjCRuntime;

namespace FieldService.iOS
{
	[Register("LaborTypeTextField")]
	public class LaborTypeTextField : UITextField
	{
		UIToolbar toolbar;
		UIBarButtonItem done;
		UIPickerView picker;
		LaborType laborType;

		public LaborTypeTextField (IntPtr handle)
			: base(handle)
		{
			Initialize ();
		}

		public LaborTypeTextField (RectangleF frame)
			: base (frame)
		{
			Initialize ();
		}

		public LaborTypeTextField ()
		{
			Initialize ();
		}

		private void Initialize ()
		{
			toolbar = new UIToolbar (new RectangleF (0, 0, UIScreen.MainScreen.Bounds.Width, 44));
			done = new UIBarButtonItem (UIBarButtonSystemItem.Done, OnDone);
			toolbar.Items = new UIBarButtonItem[] {
				new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace),
				done
			};
			picker = new UIPickerView {
				ShowSelectionIndicator = true,
			};
			picker.Model = new PickerModel(this);
			
			InputView = picker;
			InputAccessoryView = toolbar;
			TextAlignment = UITextAlignment.Left;
		}

		public LaborType LaborType
		{
			get { return laborType; }
			set
			{
				if (laborType != value)
				{
					laborType = value;
					picker.Select ((int)value, 0, false);
				}
				Text = laborType.ToUserString ();
			}
		}

		private void OnDone (object sender, EventArgs e)
		{
			ResignFirstResponder ();
		}

		protected override void Dispose (bool disposing)
		{
			done.Dispose ();
			toolbar.Dispose ();
			picker.Dispose ();

			base.Dispose (disposing);
		}

		private class PickerModel : UIPickerViewModel
		{
			readonly LaborTypeTextField textField;
			readonly LaborType[] types;

			public PickerModel (LaborTypeTextField textField)
			{
				this.textField = textField;
				types = (LaborType[])Enum.GetValues (typeof(LaborType));
			}

			public override int GetComponentCount (UIPickerView picker)
			{
				return 1;
			}

			public override int GetRowsInComponent (UIPickerView picker, int component)
			{
				return types.Length;
			}

			public override string GetTitle (UIPickerView picker, int row, int component)
			{
				return types[row].ToUserString ();
			}

			public override void Selected (UIPickerView picker, int row, int component)
			{
				textField.LaborType = types[row];
			}
		}
	}
}

