using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace FieldService.iOS
{
	public static class Extensions
	{
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

