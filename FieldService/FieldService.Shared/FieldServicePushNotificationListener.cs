using System;
using PushNotification.Plugin;
using System.Diagnostics;
using System.Collections.Generic;
using Oracle.Cloud.Mobile.MobileBackend;
using Oracle.Cloud.Mobile.Notifications;
using System.Threading.Tasks;
using PushNotification.Plugin.Abstractions;

namespace FieldService.Shared
{
	public class FieldServicePushNotificationListener : IPushNotificationListener
	{	

		//Here you will receive all push notification messages
		//Messages arrives as a dictionary, the device type is also sent in order to check specific keys correctly depending on the platform.
		public void OnMessage(IDictionary<string, object> Parameters, DeviceType deviceType)
		{
			Debug.WriteLine("Message Arrived");
		}

		//Gets the registration token after push registration
		public async void OnRegistered(string Token, DeviceType deviceType)
		{
			Debug.WriteLine (string.Format ("Push Notification - Device Registered - Token : {0}", Token));
			var notificationsService = MobileBackendManager.Manager.DefaultMobileBackend.GetService<Notifications>();

			await notificationsService.RegisterForNotificationsAsync (Token);
		}

		//Fires when device is unregistered
		public async void OnUnregistered(DeviceType deviceType)
		{
			Debug.WriteLine("Push Notification - Device Unnregistered");
			//await notificationsService.RegisterForNotificationsAsync("[Push TOKEN]");
		}

		//Fires when error
		public void OnError(string message, DeviceType deviceType)
		{
			Debug.WriteLine(string.Format("Push notification error - {0}",message));
		}

		//Enable/Disable Showing the notification
		public bool ShouldShowNotification()
		{
			return true;
		}

	}

}