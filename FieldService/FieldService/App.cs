using System;
using Xamarin.Forms;

namespace FieldService
{
	public class App
	{
		public static Page GetMainPage ()
		{	
			return new ContentPage { 
				Content = new Label {
					Text = "Hello, Forms!",
					VerticalOptions = LayoutOptions.CenterAndExpand,
					HorizontalOptions = LayoutOptions.CenterAndExpand,
				},
			};
		}
	}
}

