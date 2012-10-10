using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace FieldService.iOS
{
	/// <summary>
	/// Helper class with extension methods
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Sets a callback for registering text changed notifications on a UITextField
		/// </summary>
		/// <param name='textField'>
		/// Text field to listen events for
		/// </param>
		/// <param name='callback'>
		/// Callback to handle the event
		/// </param>
		public static void SetDidChangeNotification(this UITextField textField, Action<UITextField> callback)
		{
			if (callback == null)
				throw new ArgumentNullException("callback");

			NSNotificationCenter.DefaultCenter.AddObserver (UITextField.TextFieldTextDidChangeNotification, _ => {
				callback(textField);
			}, textField);
		}
	}
}

