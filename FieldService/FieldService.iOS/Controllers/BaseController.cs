using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace FieldService.iOS
{
	/// <summary>
	/// Base controller for all controllers in app
	/// </summary>
	public class BaseController : UIViewController
	{
		/// <summary>
		/// Required constructor for Storyboard to work
		/// </summary>
		/// <param name='handle'>
		/// Handle to Obj-C instance of object
		/// </param>
		public BaseController (IntPtr handle) : base (handle)
		{
			NSNotificationCenter.DefaultCenter.AddObserver (UIKeyboard.WillHideNotification, notification => {
				OnKeyboardNotification(false, notification);
			});
			
			NSNotificationCenter.DefaultCenter.AddObserver (UIKeyboard.WillShowNotification, notification => {
				OnKeyboardNotification(true, notification);
			});
		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}

		public override bool ShouldAutorotate ()
		{
			return true;
		}

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
		{
			return UIInterfaceOrientationMask.All;
		}

		private void OnKeyboardNotification(bool visible, NSNotification notification)
		{
			if (IsViewLoaded) {

				UIView.BeginAnimations ("AnimateForKeyboard");
				UIView.SetAnimationBeginsFromCurrentState (true);
				UIView.SetAnimationDuration (UIKeyboard.AnimationDurationFromNotification (notification));
				UIView.SetAnimationCurve ((UIViewAnimationCurve)UIKeyboard.AnimationCurveFromNotification (notification));

				bool landscape = InterfaceOrientation == UIInterfaceOrientation.LandscapeLeft || InterfaceOrientation == UIInterfaceOrientation.LandscapeRight;
				if (visible) 
				{
					var keyboardFrame = UIKeyboard.FrameEndFromNotification (notification);
					
					OnKeyboardChanged (visible, landscape ? keyboardFrame.Width : keyboardFrame.Height);
				}
				else
				{
					var keyboardFrame = UIKeyboard.FrameBeginFromNotification (notification);

					OnKeyboardChanged (visible, landscape ? keyboardFrame.Width : keyboardFrame.Height);
				}
				
				UIView.CommitAnimations ();	
			}
		}

		/// <summary>
		/// Override this method to apply custom logic when the keyboard is shown/hidden
		/// </summary>
		/// <param name='visible'>
		/// If the keyboard is visible
		/// </param>
		/// <param name='height'>
		/// Calculated height of the keyboard (width not generally needed here)
		/// </param>
		protected virtual void OnKeyboardChanged(bool visible, float height)
		{

		}
	}
}

