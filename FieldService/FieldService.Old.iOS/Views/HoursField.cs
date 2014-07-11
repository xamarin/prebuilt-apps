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
using System.Drawing;
using System.Globalization;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using FieldService.Utilities;

namespace FieldService.iOS
{
	/// <summary>
	/// Custom view for entering hours
	/// </summary>
	[Register("HoursField")]
	public class HoursField : UIView
	{
		const int Spacing = 6;
		UITextField textField;
		UIStepper stepper;

		public event EventHandler EditingDidBegin;
		public event EventHandler EditingDidEnd;
		public event EventHandler ValueChanged;

		private double value = 0;
		private bool enabled = true;

		public HoursField (IntPtr handle)
			: base(handle)
		{
			Initialize ();
		}

		public HoursField ()
		{
			Initialize ();
		}

		public HoursField (RectangleF frame)
			: base(frame)
		{
			Initialize ();
		}

		public override bool BecomeFirstResponder ()
		{
			return textField.BecomeFirstResponder ();
		}

		public override bool ResignFirstResponder ()
		{
			textField.ResignFirstResponder ();
			return base.ResignFirstResponder ();
		}

		/// <summary>
		/// Gets or sets the TextColor for the underlying UITextField
		/// </summary>
		public UIColor TextColor {
			get { return  textField.TextColor; }
			set { textField.TextColor = value; }
		}

		/// <summary>
		/// Gets or sets the step at which the up/down arrows work
		/// </summary>
		public double Step {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="FieldService.iOS.HoursField"/> is enabled.
		/// </summary>
		public bool Enabled {
			get { return enabled; }
			set {
				if (enabled != value) {
					enabled = value;

					textField.Enabled =
						stepper.Enabled = enabled;
				}
			}
		}

		/// <summary>
		/// Gets or sets the hours
		/// </summary>
		public double Value {
			get { return value; }
			set {
				if (value < 0) {
					value = 0;
				}

				textField.Text = value.ToString ("0.##");
				stepper.Value = value;

				if (this.value != value) {
					this.value = value;
					var method = ValueChanged;
					if (method != null)
						method (this, EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Called by all constructor overloads
		/// </summary>
		private void Initialize ()
		{
			Step = 0.5f;

			textField = new UITextField
			{
				TextAlignment = UITextAlignment.Right,
				VerticalAlignment = UIControlContentVerticalAlignment.Center,
				TextColor = Theme.BlueTextColor,
				KeyboardType = UIKeyboardType.NumberPad,
			};
			textField.EditingDidBegin += (sender, e) => {
				var method = EditingDidBegin;
				if (method != null)
					method (this, EventArgs.Empty);
			};
			textField.EditingDidEnd += (sender, e) => {
				Value = textField.Text.ToDouble (CultureInfo.InvariantCulture);

				var method = EditingDidEnd;
				if (method != null)
					method (this, EventArgs.Empty);
			};

			stepper = new UIStepper();
			stepper.StepValue = Step;
			stepper.Value = 0;
			stepper.ValueChanged += (sender, e) => Value = stepper.Value;

			AddSubview (stepper);
			AddSubview (textField);
		}

		/// <summary>
		/// We've overridden this to auto-size the textField and stepper
		/// </summary>
		public override void LayoutSubviews ()
		{
			var frame = Frame;
			frame.X =
				frame.Y = 0;
			frame.Width -= stepper.Frame.Width;
			frame.Width -= Spacing * 2;
			textField.Frame = frame;

			var stepperFrame = stepper.Frame;
			stepperFrame.X = frame.Width + Spacing;
			stepperFrame.Y = (Frame.Height - stepperFrame.Height) / 2;
			stepper.Frame = stepperFrame;
		}
	}
}

